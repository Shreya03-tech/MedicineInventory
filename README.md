# 💊 Medicine Expiry & Inventory Alert System

A web-based medicine inventory management application developed using **ASP.NET Core MVC, C#, Entity Framework Core, and MySQL**.

The system helps users manage medicine records, monitor stock quantities, maintain batch information, and identify expired or soon-to-expire medicines.

## ✨ Features

- **Dashboard:** View inventory summaries and stock alerts.
- **Medicine Management:** Add, view, edit, search, and delete medicine records.
- **Batch Management:** Maintain batch numbers, quantities, manufacturing dates, expiry dates, prices, and supplier information.
- **Expiry Monitoring:** Identify expired medicines and batches expiring soon.
- **Low-Stock Alerts:** Compare available stock against minimum stock thresholds.
- **Database Integration:** Store inventory records in MySQL.
- **Form Validation:** Validate medicine and batch information before saving.

## 🛠️ Technology Stack

| Technology | Purpose |
|---|---|
| C# | Application programming language |
| ASP.NET Core MVC | Web application framework |
| Entity Framework Core | Database access and ORM |
| MySQL | Relational database |
| Razor | Dynamic web pages |
| HTML, CSS, JavaScript | Front-end development |
| Bootstrap | Responsive user interface |
| Git and GitHub | Version control and source code hosting |

## 📁 Project Structure

```text
MedicineInventory/
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── Properties/
├── Views/
├── wwwroot/
├── .gitignore
├── MedicineInventory.csproj
├── Program.cs
└── appsettings.json
```

## ⚙️ Prerequisites

Install the following before running the application:

- .NET 10 SDK
- MySQL Server 8.x
- Visual Studio Code or Visual Studio
- Git

## 🚀 Setup and Installation

### 1. Clone the repository

```bash
git clone https://github.com/Shreya03-tech/MedicineInventory.git
cd MedicineInventory
```

### 2. Configure MySQL

Start your MySQL server and create the application database:

```sql
CREATE DATABASE medicine_inventory
CHARACTER SET utf8mb4
COLLATE utf8mb4_0900_ai_ci;
```

Configure a MySQL database user with appropriate permissions for this database.

### 3. Configure the database connection

Configure the `ConnectionStrings:DefaultConnection` setting using .NET User Secrets or another secure configuration provider.

For a local development environment, you can initialize User Secrets if they are not already initialized:

```bash
dotnet user-secrets init
```

Set your connection string securely:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_MYSQL_CONNECTION_STRING"
```

Replace the placeholder with your actual connection string. **Never commit passwords or credentials to GitHub.**

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Apply database migrations

```bash
dotnet ef database update
```

If the `dotnet ef` command is unavailable, install the compatible EF Core command-line tool before running the migration.

### 6. Run the application

```bash
dotnet run
```

Open the local URL shown in the terminal.

## 🧪 Testing

Test the application by:

1. Adding a medicine with its first batch.
2. Editing medicine information.
3. Adding or updating batch details.
4. Checking expiry status and low-stock alerts.
5. Searching and filtering inventory records.
6. Verifying that saved information persists in MySQL.

## 🎯 Project Objective

The objective of this project is to demonstrate how a web application can simplify medicine inventory management through database integration, batch tracking, expiry monitoring, and stock-level alerts.

## 👩‍💻 Author

**Shreya Patel**

GitHub: [Shreya03-tech](https://github.com/Shreya03-tech)

## 📌 Disclaimer

This is an educational software project. Sample inventory data is for testing and demonstration purposes only.