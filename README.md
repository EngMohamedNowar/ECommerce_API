# 🛒 ECommerce API

<p align="center">
  <strong>A scalable RESTful E-Commerce API built with ASP.NET Core and Clean Architecture.</strong>
</p>

<p align="center">
  <a href="https://github.com/EngMohamedNowar/ECommerce_API">
    <img src="https://img.shields.io/badge/GitHub-Repository-181717?style=for-the-badge&logo=github" alt="GitHub">
  </a>
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET">
  <img src="https://img.shields.io/badge/ASP.NET%20Core-REST%20API-512BD4?style=for-the-badge&logo=dotnet" alt="ASP.NET Core">
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-ORM-512BD4?style=for-the-badge&logo=dotnet" alt="EF Core">
  <img src="https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver" alt="SQL Server">
</p>

---

## 📌 Overview

**ECommerce API** is a backend RESTful API designed to provide the core services required by a modern e-commerce platform.

The project is built with **ASP.NET Core** and follows **Clean Architecture** principles to achieve a maintainable, testable, and scalable codebase.

The API is designed around clear separation of concerns between business logic, application services, infrastructure, and presentation layers.

### 🎯 Project Goals

* Build a production-oriented E-Commerce backend.
* Apply **Clean Architecture** principles.
* Keep business logic independent from infrastructure concerns.
* Provide secure authentication and authorization.
* Build a maintainable and scalable API.
* Follow modern **ASP.NET Core** development practices.
* Optimize database access using **Entity Framework Core**.

---

## ✨ Features

### 🔐 Authentication & Authorization

* JWT-based authentication.
* Secure user authentication.
* Role-based authorization.
* Protected API endpoints.
* ASP.NET Core Identity integration.

### 🛍️ Product Management

* Product catalog management.
* Product CRUD operations.
* Category management.
* Product filtering and querying.
* Inventory-related operations.

### 🛒 Shopping Cart

* Add products to cart.
* Update cart items.
* Remove products from cart.
* Retrieve the current user's cart.

### 📦 Orders

* Create orders from cart items.
* Manage order information.
* Track order-related data.
* Maintain relationships between users, products, carts, and orders.

### 👤 User Management

* User registration.
* User authentication.
* User roles.
* User profile management.
* Secure access to user-specific resources.

### 🗄️ Data Access

* Entity Framework Core.
* SQL Server.
* Repository-based data access.
* Unit of Work pattern.
* Entity relationships and Fluent API configuration.
* EF Core migrations.

### ⚡ Performance & Maintainability

* Clean separation of responsibilities.
* Efficient database querying.
* DTO-based API contracts.
* Centralized application logic.
* Dependency Injection.
* Global exception handling.

---

# 🏗️ Architecture

The project follows **Clean Architecture**, keeping the core business logic independent from external frameworks and infrastructure.

```text
                    ┌───────────────────────┐
                    │       API Layer       │
                    │   ASP.NET Core Web API│
                    └───────────┬───────────┘
                                │
                                ▼
                    ┌───────────────────────┐
                    │  Application Layer    │
                    │ Services / DTOs /     │
                    │ Interfaces / Business │
                    │ Logic                 │
                    └───────────┬───────────┘
                                │
                                ▼
                    ┌───────────────────────┐
                    │    Domain Layer       │
                    │ Entities / Contracts / │
                    │ Core Business Rules    │
                    └───────────┬───────────┘
                                ▲
                                │
                    ┌───────────┴───────────┐
                    │ Infrastructure Layer  │
                    │ EF Core / SQL Server / │
                    │ Repositories / Identity│
                    └───────────────────────┘
```

### Architectural Principles

* Separation of Concerns
* Dependency Inversion
* Dependency Injection
* Single Responsibility Principle
* Repository Pattern
* Unit of Work Pattern
* DTO Pattern
* Service Layer Pattern

---

# 📂 Project Structure

```text
ECommerce_API/
│
├── ECommerce/
│   │
│   ├── API/
│   │   ├── Controllers/
│   │   ├── Middlewares/
│   │   ├── Extensions/
│   │   └── Program.cs
│   │
│   ├── BLL/
│   │   ├── Services/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Mapping/
│   │
│   ├── DAL/
│   │   ├── Context/
│   │   ├── Repositories/
│   │   ├── UnitOfWork/
│   │   └── Configurations/
│   │
│   └── Domain/
│       ├── Entities/
│       ├── Enums/
│       └── Contracts/
│
├── ECommerce_API.slnx
├── LICENSE.txt
└── README.md
```

> The exact folders may evolve as the project grows, while the architecture remains centered around separation of concerns and dependency inversion.

---

# 🛠️ Tech Stack

| Technology                | Purpose                               |
| ------------------------- | ------------------------------------- |
| **C#**                    | Primary programming language          |
| **.NET 10**               | Application framework                 |
| **ASP.NET Core Web API**  | RESTful API development               |
| **Entity Framework Core** | ORM / Data Access                     |
| **SQL Server**            | Relational database                   |
| **ASP.NET Core Identity** | User & role management                |
| **JWT**                   | Authentication                        |
| **Clean Architecture**    | Application architecture              |
| **Repository Pattern**    | Data access abstraction               |
| **Unit of Work**          | Transaction & repository coordination |
| **Dependency Injection**  | Loose coupling                        |
| **Git & GitHub**          | Version control                       |

---

# 🔑 Authentication Flow

The API uses **JWT Bearer Authentication**.

```text
Client
   │
   │ Login
   ▼
Authentication Endpoint
   │
   │ Validate Credentials
   ▼
ASP.NET Core Identity
   │
   │ Generate JWT
   ▼
Access Token
   │
   ▼
Client
   │
   │ Authorization: Bearer <token>
   ▼
Protected API Endpoint
   │
   ▼
JWT Validation
   │
   ▼
Authorized Request
```

---

# 🗃️ Database

The project uses **Microsoft SQL Server** as the primary relational database with **Entity Framework Core** as the ORM.

Database responsibilities include:

* Entity persistence.
* Relationships.
* Constraints.
* Migrations.
* Querying.
* Transaction management.

### Entity Relationship Concept

```text
User
 │
 ├──────────► Cart
 │              │
 │              └────► Cart Items
 │                         │
 │                         ▼
 │                       Product
 │
 └──────────► Orders
                │
                └────► Order Items
                           │
                           ▼
                         Product

Category
   │
   └──────────► Products
```

---

# 🚀 Getting Started

## Prerequisites

Make sure you have the following installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* Microsoft SQL Server
* Visual Studio 2022/2026 or VS Code
* Git

---

## 1️⃣ Clone the Repository

```bash
git clone https://github.com/EngMohamedNowar/ECommerce_API.git
```

```bash
cd ECommerce_API
```

---

## 2️⃣ Configure the Database

Update your connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For production environments, use environment variables or a secure secrets provider instead of committing sensitive credentials.

---

## 3️⃣ Apply EF Core Migrations

From the project directory:

```bash
dotnet ef database update
```

If EF CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

---

## 4️⃣ Run the Application

```bash
dotnet run
```

Or run the project directly through Visual Studio.

---

# 📡 API Documentation

The API is designed to be consumed by any frontend or client application capable of making HTTP requests.

Typical resources include:

```text
/api/auth
/api/users
/api/products
/api/categories
/api/cart
/api/orders
```

For development, the API can be tested using:

* Postman
* Swagger
* .NET HttpClient
* Any frontend application

---

# 🧪 Testing the API

You can use **Postman** to test authentication and protected endpoints.

Example:

```http
POST /api/auth/login
Content-Type: application/json
```

Request:

```json
{
  "email": "user@example.com",
  "password": "your-password"
}
```

After successful authentication, use the returned JWT:

```http
Authorization: Bearer <your-token>
```

---

# 🔒 Security

Security is an important part of the API design.

The project uses:

* JWT authentication.
* Role-based authorization.
* ASP.NET Core Identity.
* Password hashing.
* Authorization policies.
* Dependency Injection.
* Input validation.
* Secure configuration practices.

### ⚠️ Important

Never commit secrets such as:

```text
Database passwords
JWT signing keys
API keys
Connection strings containing credentials
```

Use:

```text
User Secrets
Environment Variables
Azure Key Vault
```

for sensitive configuration.

---

# 📈 Future Improvements

The project is continuously evolving. Planned improvements include:

* [ ] Comprehensive Unit Tests
* [ ] Integration Tests
* [ ] Redis Caching
* [ ] Advanced Product Filtering
* [ ] Pagination & Sorting
* [ ] Payment Gateway Integration
* [ ] Email Notifications
* [ ] Background Jobs
* [ ] Docker Containerization
* [ ] CI/CD with GitHub Actions
* [ ] Cloud Deployment
* [ ] Advanced Logging & Monitoring
* [ ] API Versioning
* [ ] Rate Limiting

---

# 📚 What I Learned

Building this project helped reinforce several important backend engineering concepts:

* Designing RESTful APIs.
* Applying Clean Architecture.
* Implementing JWT authentication.
* Working with ASP.NET Core Identity.
* Building scalable service layers.
* Using Entity Framework Core efficiently.
* Applying Repository & Unit of Work patterns.
* Designing relational database relationships.
* Managing EF Core migrations.
* Applying SOLID principles.
* Writing maintainable and reusable backend code.

---

# 👨‍💻 Author

**Mohamed Nowar**

.NET Backend Developer focused on building clean, scalable, and maintainable backend systems.

### Connect With Me

* GitHub: [@EngMohamedNowar](https://github.com/EngMohamedNowar)
* Repository: [ECommerce_API](https://github.com/EngMohamedNowar/ECommerce_API)

---

# ⭐ Support

If you find this project useful or interesting, consider giving it a ⭐ on GitHub.

Your feedback and suggestions are always welcome.

---

## 📄 License

This project is licensed under the **MIT License**.

See the `LICENSE.txt` file for more information.
