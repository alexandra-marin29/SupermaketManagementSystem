# Supermarket Management System

## Overview

Supermarket Management System is a desktop application developed in C# using WPF and SQL Server.

The project manages core supermarket operations such as products, categories, manufacturers, users, stock, sales, receipts, and reports.

---

## Features

- User authentication
- Role-based access
- Product management
- Category and manufacturer management
- Stock management
- Sales and receipt management
- Reports section
- SQL Server database integration
- CRUD operations through a dedicated data access layer

---

## Technologies Used

- C#
- WPF
- .NET Framework
- SQL Server
- ADO.NET / SqlClient
- MVVM Pattern

---

## Database

The application uses a SQL Server database to store and manage supermarket-related data.

Main entities include:

- Users
- Roles
- Products
- Categories
- Manufacturers
- Stock records
- Receipts
- Receipt details

Database communication is handled through a dedicated Data Access Layer, keeping SQL operations separated from the user interface and business logic.

---

## Architecture

The project follows a layered structure:

- **Views** — WPF interface screens
- **ViewModels** — connects the interface with application logic
- **Business Logic Layer** — handles application-specific operations
- **Data Access Layer** — manages SQL Server communication
- **Entity Layer** — defines the main data models

This structure helps separate responsibilities and keeps the project easier to understand and maintain.

---

## Main Modules

### Product and Stock Management

Manages products, categories, manufacturers, and stock information.

### User Management

Handles users, authentication, and role-based access.

### Sales and Receipts

Supports sales operations, receipt creation, and receipt details.

### Reports

Provides reporting functionality based on stored supermarket data.
