<div align="center">

  <h1 align="center">CoreBooking API</h1>

  <p align="center">
    <strong>Advanced Hotel Booking Engine built with .NET 8, Clean Architecture, and DDD.</strong>
    <br />
    Features CQRS, Optimistic Concurrency Control, and Dynamic Inventory Management.
    <br />
    <br />
    <a href="#-about-the-project">About</a> ·
    <a href="#-architecture--design-patterns">Architecture</a> ·
    <a href="#-getting-started">Getting Started</a> ·
    <a href="#-key-features">Features</a>
  </p>

  <p align="center">
    <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 8">
    <img src="https://img.shields.io/badge/Architecture-Clean%20%26%20DDD-blue?style=for-the-badge" alt="Architecture">
    <img src="https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server" alt="SQL Server">
    <img src="https://img.shields.io/badge/Pattern-CQRS-green?style=for-the-badge" alt="CQRS">
  </p>
</div>

<br />

## 💻 About The Project

**CoreBooking** is a robust backend API designed to solve real-world challenges in the hospitality industry. Unlike simple CRUD applications, CoreBooking addresses complex scenarios such as **inventory management**, **dynamic pricing**, and the critical **"double booking" (overbooking)** problem in high-concurrency environments.

Built on top of the solid foundation of `nArchitecture`, this project demonstrates how to implement **Rich Domain Models** and **Business Invariants** within a scalable infrastructure.

## 🏗 Architecture & Design Patterns

This project adheres to **Clean Architecture** principles and **Domain-Driven Design (DDD)**.

### Core Concepts Implemented:
* **CQRS (Command Query Responsibility Segregation):** * Separated Read and Write operations using **MediatR**.
    * Write operations (`Commands`) handle complex domain logic and transaction consistency.
    * Read operations (`Queries`) are optimized for performance.
* **Domain-Driven Design (DDD):**
    * **Rich Domain Model:** Entities like `Booking` and `Inventory` encapsulate their own logic (e.g., validation, state changes) rather than being Anemic data bags.
    * **Invariant Protection:** Business rules (e.g., "Cannot cancel within 3 days") are enforced within the Domain layer.
* **Optimistic Concurrency Control:**
    * Solved the "Last Room" problem using `RowVersion` (Timestamp) at the database level.
    * Prevents race conditions where multiple users try to book the same last unit simultaneously.
* **Dynamic Inventory & Pricing:**
    * Prices are not static; they are calculated dynamically based on daily inventory rates stored in the database.

## 🌟 Key Features

- **🏨 Hotel & Room Management:** Flexible hierarchy for managing properties and room types.
- **📅 Granular Inventory System:** Daily stock and price management for precise availability control.
- **🛡️ Concurrency Safe Booking:** Thread-safe booking process using EF Core's Optimistic Concurrency.
- **💰 Dynamic Price Calculation:** Automatically aggregates daily prices for a given date range.
- **🔐 Robust Security:** JWT-based Authentication, Refresh Token rotation, and Role-Based Authorization.
- **⚡ Performance:** Middleware optimizations, Caching infrastructure (Redis ready), and efficient LINQ queries.

## 🛠️ Tech Stack

* **Framework:** .NET 8 Web API
* **Language:** C# 12
* **Data Access:** Entity Framework Core 8
* **Database:** Microsoft SQL Server
* **Mediator:** MediatR
* **Validation:** FluentValidation
* **Mapping:** AutoMapper
* **Documentation:** Swagger / OpenAPI
* **Boilerplate:** [nArchitecture](https://github.com/kodlamaio-projects/nArchitecture)

## ⚙️ Getting Started

Follow these steps to get a local copy up and running.

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB or Full Instance)

### Installation

1.  **Clone the repository**
    ```sh
    git clone [https://github.com/yourusername/CoreBooking.git](https://github.com/yourusername/CoreBooking.git)
    cd CoreBooking
    ```

2.  **Configure Database**
    Update the connection string in `src/coreBooking/WebAPI/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "BaseDb": "Server=(localdb)\\mssqllocaldb;Database=CoreBookingDb;Trusted_Connection=True;"
    }
    ```

3.  **Apply Migrations**
    Open your terminal in the root folder and run:
    ```sh
    dotnet ef database update --project src/coreBooking/Persistence --startup-project src/coreBooking/WebAPI
    ```
    *(Or use Package Manager Console: `Update-Database`)*

4.  **Run the API**
    ```sh
    dotnet run --project src/coreBooking/WebAPI
    ```

### Data Seeding (How to Test)

Since the logic relies on inventory, follow this flow in Swagger to test the booking engine:

1.  **Auth:** Register a new user via `/api/Auth/Register` and copy the `AccessToken`. Authorize via the padlock button.
2.  **Hotel:** Create a Hotel via `POST /api/Hotels`.
3.  **Room:** Create a RoomType via `POST /api/RoomTypes`.
4.  **Inventory:** Add stock for specific dates (e.g., tomorrow) via `POST /api/Inventories`.
5.  **Booking:** Use `POST /api/Bookings` to make a reservation. The system will check stock, calculate the price, and reduce the inventory quantity.

## 🚧 Roadmap

- [x] Core Domain Modeling & Relationships
- [x] Inventory Management & Pricing Logic
- [x] **Optimistic Concurrency Control** implementation
- [ ] Advanced Search Engine (Search by City, Date Range, Pax)
- [ ] Booking Cancellation & Refund Logic
- [ ] Payment Gateway Integration
- [ ] Unit & Integration Tests

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## ⚖️ License

Distributed under the MIT License.

---
<p align="center">
    Made with 💻 by <a href="https://github.com/mustafakurtt">Mustafa Kurt</a>
</p>
