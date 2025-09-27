# StocksApp

This repository contains the source code for **StocksApp**, a web application designed to display real-time stock price information. The application focuses on providing a clear and functional interface for tracking live market data.

-----

## Screenshots

Here are some screenshots of the application interface:

![Screenshot 1](screenshots/Screenshot%201.png)
![Screenshot 2](screenshots/Screenshot%202.png)
![Screenshot 3](screenshots/Screenshot%203.png)

-----

## Core Features

The primary objective of the StocksApp application is to provide users with essential stock market information.

  * **Live Stock Price Display:** Shows current market prices for selected stocks.
  * **Data Updates:** Fetches and updates stock data in real-time or near real-time.
  * **Modular Architecture:** Designed with clear separation of concerns (Entities, Services, Repositories).

-----

## Technologies Used

The application is primarily built as a web solution using Microsoft technologies and standard web languages.

  * **Backend:** C\# (.NET Framework or .NET Core)
  * **Frontend:** HTML, CSS, and JavaScript
  * **Project Structure:** Solution file (`.sln`) indicates a standard Visual Studio/C\# project setup.

-----

## Setup and Running

To clone this project and run it locally, you will need the appropriate .NET SDK installed on your machine.

### Prerequisites

  * **.NET SDK:** Ensure you have the necessary .NET SDK (likely .NET Core or a recent .NET version) for building and running C\# projects.
  * **Git:** Required for cloning the repository.

### Installation

1.  **Clone the Repository**

    ```bash
    git clone https://github.com/atymri/StocksApp.git
    cd StocksApp
    ```

2.  **Restore Dependencies**
    Open the solution file (`StocksApp.sln`) in a compatible IDE like Visual Studio, or use the command line:

    ```bash
    dotnet restore
    ```

3.  **Run the Application**
    Navigate to the main web project directory (e.g., `StocksApp.Web`) and execute the run command:

    ```bash
    dotnet run --project StocksApp.Web
    ```

    The application will typically launch on a local port (e.g., `http://localhost:5000`) displayed in the console output.

-----

## License

This project is released under the **Apache-2.0 License**. See the `LICENSE.txt` file for full details.
