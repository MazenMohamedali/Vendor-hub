using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace VendorHub.Application.Common.Behaviors
{
    public sealed class RequestPerformanceBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RequestPerformanceBehavior<TRequest, TResponse>> _logger;

        public RequestPerformanceBehavior(ILogger<RequestPerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var actionName = typeof(TRequest).Name;

            _logger.LogInformation("Initiating request: {RequestName}", actionName);

            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            if (timer.ElapsedMilliseconds > 500)
                _logger.LogWarning("Long-running execution: {RequestName} took {ElapsedMs} ", actionName, timer.ElapsedMilliseconds);
            else
                _logger.LogInformation("Finished request: {RequestName} in {ElapsedMs} ms", actionName, timer.ElapsedMilliseconds);

            return response;
        }
    }
}