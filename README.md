# StocksApp

A web application for tracking stock prices and managing a portfolio.

## Screenshot
![Screenshot1](https://github.com/atymri/StocksApp/blob/master/screenshots/Screenshot%201.png)
![Screenshot2](https://github.com/atymri/StocksApp/blob/master/screenshots/Screenshot%202.png)
![Screenshot3](https://github.com/atymri/StocksApp/blob/master/screenshots/Screenshot%203.png)
![Screenshot3](https://github.com/atymri/StocksApp/blob/master/screenshots/Screenshot%204.png)

## Architecture

The application follows a layered architecture, which separates concerns and improves maintainability.

- **StocksApp.Core**: Contains the core business logic, domain models, and service interfaces.
- **StocksApp.Infrastructure**: Implements the data access layer using Entity Framework Core and repositories.
- **StocksApp.Web**: The user interface of the application, built with ASP.NET Core MVC.
- **StocksApp.Tests**: Contains unit and integration tests for the application.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/StocksApp.git
   ```
2. Navigate to the project directory:
   ```bash
   cd StocksApp
   ```
3. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```
4. Update the database with the latest migrations:
   ```bash
   dotnet ef database update --project StocksApp.Infrastructure
   ```

## Running the Application

To run the application, execute the following command from the root directory:

```bash
dotnet run --project StocksApp.Web
```

The application will be available at `http://localhost:5000`.

## Technologies Used

- ASP.NET Core
- Entity Framework Core
- Serilog
- xUnit
- Moq
- Fluent Assertions
