# Jewellery Management System

A modern jewellery management platform built with **ASP.NET Core**,
**Entity Framework Core**, and **SQL Server**. The system brings product
and inventory management, document processing, and online jewellery
sales into one application.

> **Project status:** Under active development. Some features and
> deployment details may change as the project evolves.

## Overview

Jewellery Management System is designed to help jewellery businesses
manage their products, stock, supporting documents, and customer orders
through a unified web application.

The solution follows a layered **Clean Architecture** approach,
separating the domain model, application contracts and use cases,
infrastructure implementations, and web presentation layer.

## Key Features

### Product and Inventory Management

-   Create, view, update, and delete jewellery products.
-   Manage product details such as name, barcode, karat, weight, prices,
    and quantity.
-   Upload and manage multiple product images.
-   Select a primary product image.
-   Track stock availability.

### Online Shop and Orders

-   Browse available products through the shop.
-   Add products to a session-based shopping cart.
-   Complete checkout with customer and delivery information.
-   Create orders and order items.
-   Reduce stock when an order is placed.
-   Track order statuses.

### Document Processing with OCR

-   Upload document images for text extraction.
-   Process images before OCR to improve recognition.
-   Extract structured data from supported document types:
    -   Certificates
    -   Invoices
    -   Appraisal reports
-   Save extracted document information and associate it with a product
    where applicable.

**Important:** OCR results should be reviewed before they are used to
change product information. In particular, a certificate's stone weight
(for example, carats) must not be confused with the jewellery item's
weight in grams. Product updates based on certificate data require
explicit review and approval.

## Technology Stack

  Area               Technology
  ------------------ --------------------------------
  Backend            C# / ASP.NET Core
  Framework          .NET 10
  Architecture       Clean Architecture
  ORM                Entity Framework Core
  Database           Microsoft SQL Server
  OCR                Tesseract OCR
  Image processing   SixLabors.ImageSharp
  Frontend           ASP.NET Core MVC / Razor Views

## Solution Structure

The solution is organized into the following projects:

``` text
JewelleryManagementSystem/
├── Jewellery.Domain/
│   └── Entities, enums, and domain rules
├── Jewellery.Application/
│   └── DTOs, interfaces, and application contracts
├── Jewellery.Infrastructure/
│   └── Database access and service implementations
└── Jewellery.Web/
    └── MVC controllers, views, and web application configuration
```

The exact folder contents may grow as additional features are
implemented.

## Prerequisites

Before running the application, install and configure:

-   .NET 10 SDK
-   Microsoft SQL Server
-   Visual Studio 2022 or another .NET-compatible IDE
-   The Tesseract OCR native runtime and English trained-data file
    required by the application

The OCR service currently expects its Tesseract files (including
`tessdata/eng.traineddata`, `tesseract55.dll`, and
`leptonica-1.85.0.dll`) to be available under the application's output
directory. Ensure the required files are copied there when building or
publishing the web project.

## Getting Started

### 1. Clone the repository

``` bash
git clone <YOUR_GITHUB_REPOSITORY_URL>
cd JewelleryManagementSystem
```

### 2. Configure the database connection

Open the web project's configuration file (for example,
`appsettings.json`) and set the SQL Server connection string for your
local environment.

Do not commit real passwords, production credentials, or other secrets.
Use .NET user secrets or environment variables for sensitive
configuration.

Example connection-string shape:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=JewelleryManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> The connection-string key and database name shown above are examples.
> Use the key and database configuration defined by the application.

### 3. Restore and build

From the solution directory, run:

``` bash
dotnet restore
dotnet build
```

### 4. Apply Entity Framework Core migrations

Run the database migration command using the actual `DbContext`, startup
project, and migrations project configured in the solution.

A typical command has this form:

``` bash
dotnet ef database update --project Jewellery.Infrastructure --startup-project Jewellery.Web
```

If the solution uses a different migrations project or requires a
specific `DbContext`, add the corresponding `--context` option and
adjust the project arguments.

### 5. Run the web application

``` bash
dotnet run --project Jewellery.Web
```

Use the local URL printed by ASP.NET Core in the terminal or IDE.

## OCR Notes

The OCR workflow uses Tesseract to extract text from uploaded document
images. Image preprocessing is performed with ImageSharp before
recognition.

For reliable results:

-   Use clear, well-lit, high-resolution images.
-   Keep the document flat and avoid shadows or reflections.
-   Review extracted values before saving or applying them.
-   Verify units and meanings, especially weight and purity values.

OCR output can contain recognition errors and should not be treated as
verified data without review.

## Security and Configuration

Before deploying or publishing the repository:

-   Keep database credentials and API keys out of source control.
-   Review `appsettings.*.json` files for secrets.
-   Do not commit local database files, build output, temporary OCR
    files, or personal data.
-   Configure production connection strings and file-storage paths
    through secure environment-specific settings.
-   Validate uploaded files and restrict accepted file types and sizes.

## Screenshots

Screenshots of the application will be added here as the user interface
design is finalized.

Suggested screenshots: - Home page - Products and product details - Shop
and shopping cart - Checkout and order details - Document OCR results -
Certificates, invoices, and appraisal reports

## Roadmap

Potential next steps as development continues:

-   Complete the certificate-to-product review and approval workflow.
-   Finalize and standardize the Modern Minimal user interface.
-   Expand validation and end-to-end testing.
-   Prepare and verify production deployment.
-   Add application screenshots and finalized setup instructions.

## Contributing

This project is currently under active development. If you would like to
suggest an improvement, report an issue, or contribute, please open an
issue or pull request in the repository.

## License

No license has been selected yet. Unless a license is added to this
repository, all rights remain with the project owner and reuse is not
automatically permitted.
