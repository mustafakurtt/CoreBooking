<div align="center">

  <h1 align="center">CoreBooking API</h1>

  <p align="center">
    <strong>Enterprise-grade Hotel Booking Engine built with .NET 8, Clean Architecture, and DDD.</strong>
    <br />
    Features CQRS, Optimistic Concurrency, Hybrid Search Engine, and Dynamic Inventory Management.
    <br />
    <br />
    <a href="#-about-the-project">About</a> ·
    <a href="#-architecture--patterns">Architecture</a> ·
    <a href="#-search-engine-logic">Search Engine</a> ·
    <a href="#-booking-lifecycle">Lifecycle</a>
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

**CoreBooking** is a robust backend API designed to simulate the core engine of OTAs (Online Travel Agencies) like Booking.com or Expedia. It solves complex distributed system problems such as **inventory management**, **dynamic pricing aggregation**, and the critical **"double booking" (overbooking)** problem in high-concurrency environments.

Unlike simple CRUD applications, this project enforces **Rich Domain Models** and strict **Business Invariants** within a scalable infrastructure.

---

## 🏗 Architecture & Patterns

### 1. Domain-Driven Design (DDD) & V2 Architecture
The project moved from Anemic Models (V1) to Rich Domain Models (V2).
* **Value Objects:** Replaced primitive types with `Money`, `Address`, and `DateRange` to ensure type safety and logic encapsulation.
* **Shared Kernel:** Centralized Constants and Enums for maintainability.
* **Invariants:** Business rules (e.g., "Cannot cancel within 3 days") are enforced inside Entities.

### 2. Optimistic Concurrency Control
Solved the "Last Room" problem using `RowVersion` (Timestamp) at the database level.
* **Scenario:** Two users try to book the last room at the exact same millisecond.
* **Solution:** The database rejects the second transaction with a `DbUpdateConcurrencyException`, ensuring data consistency.

### 3. CQRS with Mediator
* **Commands (Write):** Handle complex transaction scopes (Booking + Inventory + Payment).
* **Queries (Read):** Optimized with EF Core `Include` and AutoMapper `Flattening` for rich DTOs.

---

## 🔍 Advanced Search Engine Logic

The `GetAvailableRooms` query is not a simple filter; it's a hybrid algorithm.

**The Challenge:** "Find a room for 5 nights."
**The Solution:**
1.  **Gap Analysis:** The query scans the `Inventory` table. It groups records by RoomType and checks if *every single day* in the requested range has `Quantity > 0`. If even one day is sold out, the room is excluded.
2.  **Dynamic Pricing:** It aggregates (SUM) daily prices to calculate the total cost for the specific date range.
3.  **Hybrid Filtering:** Combines custom LINQ logic (Availability/Price) with Dynamic Querying (City, Hotel Name filtering).

---

## 🔄 Booking Lifecycle & Logic

The system manages the full lifecycle of a reservation:

1.  **Create (Transaction):**
    * Validates availability gap.
    * Calculates total price dynamically.
    * **Decrements** inventory stock.
    * Uses **RowVersion** to prevent race conditions.
2.  **Cancel (Compensation):**
    * Checks Domain Rule: "Is check-in within 3 days?"
    * **Increments** inventory stock (Restocking).
    * Updates Payment status to `Refunded`.
    * Invalidates Search Cache.

---

## 🛠️ Tech Stack

* **Framework:** .NET 8 Web API
* **Data Access:** Entity Framework Core 8
* **Database:** Microsoft SQL Server
* **Mediator:** MediatR
* **Validation:** FluentValidation
* **Mapping:** AutoMapper
* **Caching:** Redis (Ready) / InMemory
* **Boilerplate:** [nArchitecture](https://github.com/kodlamaio-projects/nArchitecture)

---

## 🚀 Roadmap & Status

| Module | Status | Features |
| :--- | :--- | :--- |
| **Hotel** | ✅ Done | Address Value Object, Name Uniqueness Rule |
| **RoomType** | ✅ Done | Enriched Queries, Hotel Relation |
| **Inventory** | ✅ Done | Daily Stock, Money Value Object, Concurrency |
| **Booking** | ✅ Done | **Core Engine**, DateRange Logic, Lifecycle Management |
| **Payment** | ✅ Done | Financial Safety Rules, TransactionId Check |
| **Guest** | ✅ Done | Capacity Control, EntityLengths Validation |
| **Search** | ✅ Done | **Hybrid Search Algorithm** |

---

## ⚙️ Getting Started

1.  **Clone the repository**
    ```sh
    git clone [https://github.com/mustafakurtt/CoreBooking.git](https://github.com/mustafakurtt/CoreBooking.git)
    ```
2.  **Update Connection String** in `appsettings.json`.
3.  **Run Migrations:**
    ```sh
    dotnet ef database update --project src/coreBooking/Persistence --startup-project src/coreBooking/WebAPI
    ```
4.  **Run API:** `dotnet run --project src/coreBooking/WebAPI`

---

<p align="center">
    Made with 💻 by <a href="https://github.com/mustafakurtt">Mustafa Kurt</a>
</p>
