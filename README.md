*** DWQueueAPI - Employee Leave Management System
A robust, enterprise-ready ASP.NET Core Web API designed to manage employee leaves and queueing systems. This project follows a clean architecture, utilizing Entity Framework Core (Code-First), AutoMapper, and a custom Global Exception Middleware.

*** Tech Stack
Framework: .NET 8 / .NET 9 (ASP.NET Core)

Database: SQL Server (SSMS)

ORM: Entity Framework Core (Code-First approach)

Mapping: AutoMapper

Documentation: Swagger UI (OpenAPI)

Architecture: Repository/Service Pattern with DTOs

✨ Key Features
Structured DTOs: Separate models for Creating, Updating, and Responding to keep data secure and prevent "Overposting."

Baseline Migration Logic: Managed via the "Baseline Strategy" to sync C# models with an existing SQL database seamlessly.

Global Exception Handling: A custom Middleware intercepts all errors to return standardized JSON responses.

Automated Mapping: Complex object transformations (including relational data like EmployeeName) are handled by AutoMapper Profiles.

Relational Database: Full support for one-to-many relationships between Employees and Leaves.

📂 Project Structure
Plaintext
DWQueueAPI
│
├── Controllers           # API Endpoints (EmployeeLeavesController)
├── Data                  # DB Context and Entity Models
├── DTOs                  # Data Transfer Objects (Create, Update, Response)
├── Services              # Business Logic Layer (LeaveService)
├── Mapping               # AutoMapper Profiles
├── Middlewares           # Global Exception Middleware
└── Migrations            # Database Version Control (Baseline Strategy)
⚙️ Getting Started
Prerequisites
Visual Studio 2022 or VS Code

SQL Server (Express or LocalDB)

.NET SDK (8.0+)
