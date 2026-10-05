# VendorHub API 🛒

> 🚧 **WORK IN PROGRESS:** This project is currently in active development. The architecture has been initialized, and core features are actively being built.

VendorHub is a robust, enterprise-grade multi-vendor e-commerce backend. It provides a scalable foundation for customers to browse and purchase products, vendors to manage their inventory and orders, and administrators to oversee the platform.

The system is built using **ASP.NET Core** and strictly adheres to **Clean Architecture** principles combined with **Vertical Slice Architecture** for feature encapsulation.

## 🚀 Technologies & Stack

- **Framework:** .NET (C#) / ASP.NET Core Web API
- **Architecture:** Clean Architecture, Vertical Slice Architecture, Domain-Driven Design (DDD), CQRS pattern
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Caching:** Redis (Distributed Caching) & In-Memory Cache
- **Authentication/Authorization:** ASP.NET Core Identity + JWT (JSON Web Tokens)
- **Real-time:** SignalR (for live notifications and order updates)
- **Containerization:** Docker & Docker Compose

## 🏗 Architecture Overview

This project is structured into four main layers to separate concerns, enforce dependency rules, and ensure the core domain remains independent of external frameworks.

    VendorHub/
    ├── VendorHub.Domain/         # Enterprise logic, Entities, Value Objects, Domain Events
    ├── VendorHub.Application/    # Business logic, CQRS Features (Vertical Slices), DTOs
    ├── VendorHub.Infrastructure/ # External concerns: EF Core, SQL Server, Redis, Identity
    └── VendorHub.Api/            # Entry point: Controllers, Middleware, Dependency Injection Setup

### Vertical Slices

Instead of organizing the `Application` layer strictly by technical concern, features are organized by **Vertical Slices** (e.g., `Features/Orders/CreateOrder`). This ensures that all components required for a single use case (Command, Handler, Validator) live together, making the system highly maintainable.

## 🚧 Current Status & Roadmap

- [x] Initialize Clean Architecture & Vertical Slices
- [x] Setup Domain Entities, Value Objects, and Enums
- [x] Configure Infrastructure (EF Core, SQL Server, Redis, Identity)
- [x] Implement initial API Controllers (Auth, Orders, Products, Users)
- [ ] Implement full CQRS handlers for all core features
- [ ] Complete payment gateway integration
- [ ] Set up SignalR notification hubs
- [ ] Write Unit and Integration tests using xUnit & Moq

## 📦 Core Modules

- **User Management:** Role-based access control supporting `Admin`, `Vendor`, and `Customer` profiles.
- **Catalog Management:** Vendors can propose products; Admins review and approve them.
- **Order Processing:** Secure cart management, checkout flows, and state-machine-driven order status transitions.
- **Notifications:** Real-time system alerts using SignalR and Domain Events.
- **Reviews & Ratings:** Customer feedback integration for approved product purchases.

## 🛠️ Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or latest)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for running Redis and SQL Server locally)

### Local Development Setup

1.  **Clone the repository:**

        git clone git@github.com:MazenMohamedali/Vendor-hub.git
        cd Vendor-hub

2.  **Spin up dependencies via Docker Compose:**

        docker-compose up -d

3.  **Apply Database Migrations:**

        cd VendorHub.Api
        dotnet ef database update --project ../VendorHub.Infrastructure

4.  **Run the API:**

        dotnet run

5.  **Explore the Endpoints:**
    Open your browser and navigate to `https://localhost:<port>/swagger`.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes using Conventional Commits
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 👤 Author

**Mazen Mohamed**

- GitHub: [@MazenMohamedali](https://github.com/MazenMohamedali)
