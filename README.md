# TodoListApi

Welcome to the ToDoList API repository. This API provides endpoints to manage tasks, providing basic CRUD operations and additional utilities.

A live version of this API can be found at : 

[https://todolist-service-4bcheyuonq-rj.a.run.app/swagger/index.html]

## Features

- Add, Update, Delete tasks.
- Filter tasks by date, cost, and other parameters.
- Centralized error handling.
- Pagination support.
- Dockerized for easy deployment.
- Unit tests for core functionalities.

## Technologies Used

- .NET 7
- Entity Framework Core
- SQL Server
- Docker
- xUnit for testing

## Getting Started

### Prerequisites

- .NET SDK 7.0 or later
- Docker (optional)

### Installation

1. Clone the repository:

```bash
git clone https://github.com/Nooziergg/ToDoList-API.git
```

2. Navigate to the project directory and install dependencies:

```bash
cd ToDoList-API/ToDoList
dotnet restore
```

### Running the API

In the project directory:

```bash
dotnet run
```

The API will start and by default will be available at `http://localhost:5000`.

### Running with Docker

1. Build the Docker image:

```bash
docker build -t todolist-api .
```

2. Run the image:

```bash
docker run -p 5000:80 todolist-api
```

The API will be available at `http://localhost:5000`.

## API Endpoints

Please refer to our [Swagger documentation](http://localhost:5000/swagger) for a list of all available endpoints and their details.

## Running Tests

Navigate to the test directory:

```bash
cd ToDoList-API/ToDoList.Tests
```

Run the tests:

```bash
dotnet test
```

## Deployment

The repository includes a script for deploying the API to Google Cloud Run. Make sure you have the appropriate environment variables set before running the script.
