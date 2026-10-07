# VendorHub API 🛒

> 🚧 **WORK IN PROGRESS:** This project is currently in active development. The core Clean Architecture MVP, authentication, order workflows, and background workers are fully functional and compiling.

VendorHub is a robust, enterprise-grade multi-vendor e-commerce backend. It provides a scalable foundation for customers to browse and purchase products, vendors to manage their inventory and orders, and administrators to oversee the platform.

The system is built using **ASP.NET Core (.NET 10)** and strictly adheres to **Clean Architecture** principles combined with **Vertical Slice Architecture** for feature encapsulation, CQRS pattern, and deep system observability.

## 🚀 Technologies & Stack

* **Framework:** .NET 10 / ASP.NET Core Web API
* **Architecture:** Clean Architecture, Vertical Slice Architecture, Domain-Driven Design (DDD), CQRS
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First)
* **Caching:** Redis (Distributed Caching & Cache-Aside)
* **Authentication & Authorization:** ASP.NET Core Identity + JWT (Bearer Tokens)
* **Background Jobs:** .NET Hosted Background Services (`PeriodicTimer`)
* **Observability (In Progress):** Serilog, OpenTelemetry, Prometheus, Grafana, ASP.NET Core Health Checks
* **Containerization:** Docker & Docker Compose

## 🏗 Architecture Overview

The solution is partitioned into four decoupled layers:

    VendorHub/
    ├── VendorHub.Domain/         # Enterprise logic, Entities, Value Objects, Domain Events, Domain Services
    ├── VendorHub.Application/    # Use cases, CQRS Slices, Validation Pipeline, DTOs
    ├── VendorHub.Infrastructure/ # EF Core, SQL Server, Redis Cache, ASP.NET Core Identity, Hosted Services
    └── VendorHub.Api/            # Entry point: Controllers, Filters, Middleware, OpenAPI / Swagger

### Key Architectural Highlights
* **Domain Service (`OrderFulfillmentService`):** Encapsulates cross-aggregate stock verification and deduction between `Order` and `Product` without leaking database logic into entities.
* **Generic Pagination Engine:** Reusable `PagedList<T>` container and EF Core `ToPagedListAsync()` queryable extension.
* **Resilient Background Worker:** `OrderProcessingBackgroundService` runs every 15 seconds using `PeriodicTimer`, creates isolated DI scopes per cycle, and fulfills pending orders via batch commits.
* **Cache-Aside Pattern:** Redis-backed order and product caching with hit/miss telemetry.

## 🚧 Current Status & Roadmap

- [x] Clean Architecture & CQRS Pipeline initialization
- [x] Pure Guid Domain Entities, Value Objects, and Domain Events
- [x] SQL Server Database Migration (`VendorHub_CleanDb`) & EF Core Configurations
- [x] Decoupled ASP.NET Core Identity & JWT Provider (`Login`, `RegisterCustomer`, `RegisterVendor`)
- [x] Core CQRS Slices (`CreateProduct`, `ApproveProduct`, `CreateOrder`, `GetOrderById`, `CancelOrder`)
- [x] Generic Pagination (`PagedList<T>`, `ToPagedListAsync`) & `GetOrders` Query
- [x] Domain Service (`OrderFulfillmentService`) for stock deduction
- [x] Production-ready Background Worker (`OrderProcessingBackgroundService`)
- [ ] **Phase 1:** Health Checks endpoint (`/health` for API, SQL Server, Redis)
- [ ] **Phase 2:** Structured Logging (Serilog + Seq integration)
- [ ] **Phase 3:** Application Metrics (.NET Meter & Prometheus `/metrics`)
- [ ] **Phase 4:** Distributed Tracing (`ActivitySource` & OpenTelemetry)
- [ ] **Phase 5:** Grafana Dashboard (5 panels) & Alerting Rules

## 📦 Core Modules

* **User Management:** Role-based access control supporting `Admin`, `Vendor`, and `Customer` profiles.
* **Catalog Management:** Vendors propose products; Admins review and approve them.
* **Order Processing:** State-machine-driven status transitions (`Pending` ➔ `Processing` ➔ `PartiallyShipped` / `Shipped` / `Failed`).
* **Inventory Management:** Automatic stock deduction during order fulfillment with out-of-stock event triggers.
* **Observability Suite:** Traces for Order creation and queries, request latency tracking, and pending-order gauges.

## 🛠️ Getting Started

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/download) (Version 10.0 or 8.0+)
* SQL Server & Redis (Local instances or Docker containers)

### Local Development Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/MazenMohamedali/Vendor-hub.git
   cd Vendor-hub
   ```

2. **Configure Connection Strings:**
   Verify `appsettings.json` in `VendorHub.Api`:
   ```json
   "ConnectionStrings": {
     "sqlServerCs": "Data Source=.; Initial Catalog=VendorHub_CleanDb; Integrated Security=True; Encrypt=False;",
     "RedisConnection": "localhost:6379"
   }
   ```

3. **Run the API:**
   ```bash
   dotnet run --project VendorHub.Api
   ```

4. **Explore the Endpoints:**
   Open your browser and navigate to `https://localhost:<port>/swagger`.

## 👤 Author
**Mazen Mohamed**
* GitHub: [@MazenMohamedali](https://github.com/MazenMohamedali)
* LinkedIn: [Mazen Mohamed](https://www.linkedin.com/in/mazen-mohamed-100ab92a9/)