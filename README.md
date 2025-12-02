# File Sharing Platform API

A RESTful API for a file sharing platform built with ASP.NET Core, Entity Framework Core, and SQL Server. Users can upload files, organize them in folders, and share via secure public links.

## ✨ Features

### User Management
- ✅ User registration with email validation
- ✅ Secure password hashing (BCrypt)
- ✅ JWT-based authentication
- ✅ User profile management
- ✅ Storage usage tracking

### File & Folder Management
- ✅ Hierarchical folder structure (up to 10 levels deep)
- ✅ File upload with validation (max 10MB)
- ✅ Supported formats: PNG, JPG, JPEG, PDF, XLSX, ZIP, RAR
- ✅ Paginated file listing with sorting
- ✅ File download functionality

### Public Sharing
- ✅ Generate secure public share links
- ✅ Optional password protection
- ✅ Customizable expiration (1-30 days)
- ✅ Share link management (disable before expiration)
- ✅ Read-only access for public links

## 🛠 Technologies Used

- **Framework:** ASP.NET Core 8.0 / .NET 8.0
- **Database:** SQL Server
- **ORM:** Entity Framework Core 8.0
- **Authentication:** JWT Bearer Tokens
- **Password Hashing:** BCrypt.Net-Next
- **API Documentation:** Swagger/OpenAPI
- **Architecture:** Clean Architecture with Repository & Unit of Work patterns

## 💻 System Requirements

Before you begin, ensure you have the following installed:
- **.NET 8.0 SDK** or later
- **SQL Server** :
  - SQL Server Developer Edition

## 🚀 Installation & Setup

### Step 1: Clone or Download the Project
git clone https://github.com/somayayasser3/FileSharePlatform.git
cd FileSharePlatform

### Step 2: Open in Visual Studio

1. Open **Visual Studio 2022**
2. Click **File** → **Open** → **Project/Solution**
3. Navigate to the project folder
4. Select `FileSharePlatform.sln`
5. Click **Open**

### Step 3: Restore NuGet Packages

Visual Studio should automatically restore packages. If not:

1. Right-click on the **Solution** in Solution Explorer
2. Select **Restore NuGet Packages**

## 🗄 Database Configuration

### Step 1: Configure Connection String

1. Open `appsettings.json`
2. Update the connection string with your SQL Server details:

**For SQL Server Express:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=FileSharingPlatformDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```
### Step 2: Update JWT Secret Key (Important!)

In `appsettings.json`, change the JWT secret key:
```json
{
  "JwtSettings": {
    "SecretKey": "YOUR_SECURE_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "Issuer": "FileSharePlatform",
    "Audience": "FileSharePlatformUsers"
  }
}
```

### Step 3: Create Database Migration

Open **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console):
```powershell
# Create migration
Add-Migration InitialCreate

# Apply migration to create database
Update-Database
```

### Step 4: Run Application 
