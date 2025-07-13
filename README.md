# Mini IAM - Simple Access Management Platform

## Overview

Mini IAM is a simple user and permission management system built with C# and ASP.NET Core (.NET 6+). This application allows user registration, simulated authentication, role assignment, and querying users with their assigned roles.

## Features

- User registration with email, name, and password validations  
- Simulated login endpoint  
- Assign roles to users  
- Retrieve user information along with their roles  
- RESTful API using ASP.NET Core  
- Unit tests with xUnit  
- CI pipeline using GitHub Actions for automated build and test  

## Technologies Used

- C# 
- ASP.NET Core Web API  
- Entity Framework Core (in-memory database)  
- xUnit for unit testing  
- GitHub Actions for CI  

## API Endpoints

| Method | Endpoint           | Description                       |
|--------|--------------------|---------------------------------|
| POST   | `/users`           | Create a new user                |
| POST   | `/login`           | Simulate login (no real auth)   |
| POST   | `/users/{id}/roles`| Assign a role to a user          |
| GET    | `/users/{id}`      | Get user info and their roles    |

## Validations

- Valid email format required  
- Name and password are mandatory with a minimum length of 6 characters  

## Architecture

- Layered structure: Controllers, Services, Repositories  
- Application of SOLID principles for maintainability and scalability  

## Running the Application Locally

1. Clone the repository:  
   git clone https://github.com/leo3852/MiniIAM.git
   cd MiniIAM

2. Restore dependencies and build the project:
   dotnet restore 
   dotnet build
3. Run the application
   dotnet run --project MiniIAM
4. The API will be available at https://localhost:5001 (or http://localhost:5000).

## Running Tests

dotnet test

## CI with GitHub Actions

The repository includes a GitHub Actions workflow (.github/workflows/ci.yml) that automatically builds and runs tests on each push and pull request.



## notes

This project uses an in-memory database for simplicity; no external database is required.

Authentication is simulated and does not implement real security mechanisms.