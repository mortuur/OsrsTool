# OsrsTool Architecture

## Domain-Driven Development (DDD) Structure

This project follows Domain-Driven Development principles with a clear separation of concerns across four main layers:

### 1. Domain Layer (`OsrsTool.Domain`)
The core of the application containing business logic and domain models.

**Responsibilities:**
- Define domain entities and value objects
- Implement business rules and validation
- Maintain domain invariants
- No dependencies on other layers

**Current Entities:**
- `Investment`: Represents an OSRS item investment with properties like ItemName, Quantity, PurchasePrice, CurrentPrice, and methods for calculating profit/loss.

### 2. Application Layer (`OsrsTool.Application`)
Contains application-specific business logic and orchestrates domain operations.

**Responsibilities:**
- Define use cases and application services
- Coordinate domain objects
- Handle transactions
- Define interfaces for infrastructure services
- Depends only on Domain layer

### 3. Infrastructure Layer (`OsrsTool.Infrastructure`)
Implements technical capabilities that support higher layers.

**Responsibilities:**
- Implement data access (repositories)
- Implement external service integrations
- Handle persistence concerns
- Implement interfaces defined in Application layer
- Depends on Domain and Application layers

### 4. API Layer (`OsrsTool.Api`)
The presentation layer exposing Web API endpoints.

**Responsibilities:**
- Define HTTP endpoints (controllers)
- Handle HTTP requests/responses
- Perform input validation
- Configure dependency injection
- Depends on Application and Infrastructure layers

## Testing Strategy

We use **TUnit** as our testing framework across all layers:

### Domain Tests (`OsrsTool.Domain.Tests`)
- Test domain entities and their business logic
- Verify business rule enforcement
- Example: Investment entity tests (11 tests covering all functionality)

### Application Tests (`OsrsTool.Application.Tests`)
- Test application services and use cases
- Verify orchestration logic
- Test integration between application and domain layers

### API Tests (`OsrsTool.Api.Tests`)
- Test HTTP endpoints
- Verify request/response handling
- Test API-specific validation and error handling

## Frontend Architecture

The frontend is built with **React** and **TypeScript**, located in the `client/` directory.

**Key Technologies:**
- React 18
- TypeScript
- Create React App

The frontend communicates with the backend API to provide a user interface for tracking OSRS investments.

## Benefits of this Architecture

1. **Separation of Concerns**: Each layer has a clear responsibility
2. **Testability**: Easy to test each layer independently
3. **Maintainability**: Changes in one layer have minimal impact on others
4. **Flexibility**: Easy to swap implementations (e.g., change database or UI framework)
5. **Domain Focus**: Business logic is isolated and protected from technical concerns

## Getting Started

See the main [README.md](../README.md) for setup instructions.
