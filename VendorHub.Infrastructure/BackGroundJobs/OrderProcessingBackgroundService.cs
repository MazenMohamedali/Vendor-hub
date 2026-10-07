using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Domain.Entities.Products;
using VendorHub.Domain.Repositories;
using VendorHub.Domain.Services;

namespace VendorHub.Infrastructure.BackGroundJobs
{
    public class OrderProcessingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderProcessingBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(15);

        public OrderProcessingBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<OrderProcessingBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderProcessingBackgroundService started. Running every {Seconds}s.", _period.TotalSeconds);

            using var timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await ProcessPendingOrdersBatchAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error occurred during background order processing cycle.");
                }
            }

            _logger.LogInformation("OrderProcessingBackgroundService is shutting down.");
        }

        private async Task ProcessPendingOrdersBatchAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var fulfillmentService = scope.ServiceProvider.GetRequiredService<OrderFulfillmentService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var pendingOrders = await orderRepository.GetPendingOrdersAsync(batchSize: 20, cancellationToken);

            if (!pendingOrders.Any())
            {
                return;
            }

            _logger.LogInformation("Background worker picked up {Count} pending orders to process.", pendingOrders.Count);

            foreach (var order in pendingOrders)
            {
                try
                {
                    _logger.LogInformation("Processing order {OrderId}...", order.Id);

                    // Load required products for this order
                    var products = new List<Product>();
                    foreach (var item in order.Items)
                    {
                        var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                        if (product != null)
                        {
                            products.Add(product);
                        }
                    }

                    // Delegate cross-aggregate stock deduction and completion to domain service
                    fulfillmentService.FulfillOrder(order, products);

                    _logger.LogInformation("Order {OrderId} processed successfully. New Status: {Status}", order.Id, order.Status);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to fulfill order {OrderId}. Reason: {Message}", order.Id, ex.Message);

                    try
                    {
                        order.FailProcessing();
                    }
                    catch (Exception failEx)
                    {
                        _logger.LogError(failEx, "Could not set failed status on order {OrderId}.", order.Id);
                    }
                }
            }

            // Batch Save: Commit all orders and product stock updates in a single round-trip
            await unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Batch of {Count} orders committed to database successfully.", pendingOrders.Count);
        }
    }
}
