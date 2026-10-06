# Order Management System

A robust, scalable .NET API and background processing service designed to handle order lifecycles. Built with Clean Architecture and CQRS principles, this system ensures clear separation of concerns, high-performance asynchronous processing, and deep system observability.

## 🚀 Features

- **Command Query Responsibility Segregation (CQRS):**
  - `CreateOrderCommand`: Handles the creation and initial validation of new orders.
  - `GetOrderByIdQuery`: Retrieves specific order details efficiently.
  - `GetOrdersQuery`: Fetches a paginated/filtered list of all system orders.
- **Asynchronous Background Processing:**
  - Dedicated `OrderProcessingBackgroundService` to handle long-running order state transitions and background tasks without blocking the main API threads.
- **Comprehensive Observability:**
  - **Distributed Tracing:** Full request tracing across `Create Order`, `Get Order By ID`, and `Get Orders` operations.
  - **Metrics Collection:** Real-time tracking of order volumes, processing queues, and background worker health.
  - **Operational Alerts:** Configured thresholds for worker downtime and order processing failure rates.

## 🛠 Tech Stack

- **Framework:** .NET (C#)
- **Architecture:** Clean Architecture, CQRS, Domain-Driven Design (DDD) principles
- **Libraries & Tools:** MediatR, Entity Framework Core, OpenTelemetry
- **Infrastructure:** SQL Server / PostgreSQL (Configurable), Docker, Redis (Caching)

## 📂 Project Structure

```text
├── src
│   ├── OrderSystem.Api
│   ├── OrderSystem.Application
│   ├── OrderSystem.Domain
│   ├── OrderSystem.Infrastructure
│   └── OrderSystem.Worker
```

- **`Api`**: REST endpoints, Middleware, and Dependency Injection setup.
- **`Application`**: CQRS Handlers, DTOs, and Business Use Cases.
- **`Domain`**: Core Entities (Order), Value Objects, and Domain Interfaces.
- **`Infrastructure`**: EF Core DbContext, Repositories, and External Integrations.
- **`Worker`**: BackgroundServices for asynchronous processing.

## 📊 Observability & Monitoring

This project is instrumented for deep operational visibility.

**Active Traces:**

- API Request Pipeline -> Command/Query Handler -> Database Transaction
- Specific tracking spans for `CreateOrder`, `GetOrderById`, and `GetOrders`.

**Key Metrics Tracked:**

- Total orders created (Counter)
- Active orders in processing queue (Gauge)
- Background worker execution time and health status (Histogram/Status)

## 🚦 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Local Development Setup

1. **Clone the repository:**

   ```bash
   git clone <repository-url>
   cd OrderSystem
   ```

2. **Spin up dependencies:**
   Start the required databases and observability tools via Docker Compose.

   ```bash
   docker-compose up -d
   ```

3. **Apply Database Migrations:**

   ```bash
   dotnet ef database update \
     --project src/OrderSystem.Infrastructure \
     --startup-project src/OrderSystem.Api
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project src/OrderSystem.Api
   ```

## 📝 Next Steps (Development Phase)

- [ ] Implement `Order` Domain Entity and `IOrderRepository`.
- [ ] Complete MediatR Handlers for `GetOrders` query.
- [ ] Integrate `OrderProcessingBackgroundService`.
- [ ] Instrument code with OpenTelemetry for the Observability phase.
