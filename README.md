# Library API

A RESTful Web API for managing books, members, and loans.

## Technologies

- C#
- ASP.NET Core
- Entity Framework Core
- xUnit
- Moq
- Swagger
- In-Memory Database

## Features

- Create and manage books
- Create and manage members
- Create and manage loans
- Return books
- Book availability management
- Input validation
- Global exception handling
- Unit tests
- Integration tests
- Controller unit tests with Moq

## API Endpoints

### Books

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/book` | Get all books |
| GET | `/api/book/{id}` | Get book by ID |
| POST | `/api/book` | Create a book |
| DELETE | `/api/book/{id}` | Delete a book |

### Members

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/member` | Get all members |
| GET | `/api/member/{id}` | Get member by ID |
| POST | `/api/member` | Create a member |
| DELETE | `/api/member/{id}` | Delete a member |

### Loans

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/loan` | Get all loans |
| GET | `/api/loan/{id}` | Get loan by ID |
| POST | `/api/loan` | Create a loan |
| DELETE | `/api/loan/{id}` | Return a book |

## Testing

The project contains:

- Service unit tests
- Controller unit tests using Moq
- API integration tests

Run all tests with:

```bash
dotnet test
