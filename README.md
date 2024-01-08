# Task Management Project

This repository contains the source code for a Task Management application implemented in .NET Core 7.0 using Visual Studio 2022. The project follows Clean Architecture principles to ensure modularity, scalability, and maintainability. Additionally, Test-Driven Development (TDD) practices have been incorporated to achieve a high level of code quality.

## Features

- **CRUD Operations:** Manage tasks through Create, Read, Update, and Delete operations.
- **Clean Architecture:** The project structure adheres to Clean Architecture principles, ensuring a clear separation of concerns.
- **Test-Driven Development (TDD):** The application has been developed using TDD, with a comprehensive suite of unit tests, integration tests, and end-to-end tests to ensure robustness and reliability.
- **ADO .NET and Repository Pattern:** The data layer is implemented using ADO .NET, with the Repository pattern facilitating database interactions.
- **Docker Compose:** Docker Compose is configured to simplify the deployment and management of the project, including the database.

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/task-management.git
```

### 2. Build and Run with Docker Compose

```bash
cd task-management
docker-compose up --build
```
### 3. Access the Application
Open your web browser and navigate to http://localhost:5000/swagger/index.html to access the Task Management application.
Use the login:
  jolivares
  Pass@w0rd1

## License
This project is licensed under the MIT License, allowing for free use, modification, and distribution.
