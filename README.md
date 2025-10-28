# OsrsTool

OSRS (Old School RuneScape) Investment Tracking Platform

## Overview

OsrsTool is a platform for tracking your investments in Old School RuneScape. Built with modern technologies and following Domain-Driven Development (DDD) principles.

## Technology Stack

### Backend
- **Language**: C# (.NET 9)
- **Architecture**: Domain-Driven Development (DDD)
- **Testing Framework**: TUnit

### Frontend
- **Framework**: React with TypeScript
- **Build Tool**: Create React App

## Project Structure

```
OsrsTool/
├── src/
│   ├── OsrsTool.Api/              # Web API layer
│   ├── OsrsTool.Application/      # Application layer (use cases)
│   ├── OsrsTool.Domain/           # Domain layer (business logic)
│   └── OsrsTool.Infrastructure/   # Infrastructure layer (data access)
├── tests/
│   ├── OsrsTool.Api.Tests/
│   ├── OsrsTool.Application.Tests/
│   └── OsrsTool.Domain.Tests/
└── client/                         # React frontend
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js 20+ and npm

### Backend Setup

1. Build the solution:
```bash
dotnet build
```

2. Run tests:
```bash
dotnet test
```

3. Run the API:
```bash
cd src/OsrsTool.Api
dotnet run
```

### Frontend Setup

1. Install dependencies:
```bash
cd client
npm install
```

2. Start the development server:
```bash
npm start
```

3. Run tests:
```bash
npm test
```

## Development

This project follows Domain-Driven Development principles with clear separation of concerns:

- **Domain Layer**: Contains the core business logic and domain models
- **Application Layer**: Contains application services and use cases
- **Infrastructure Layer**: Contains implementations for data access, external services, etc.
- **API Layer**: Contains the Web API controllers and endpoints

## Testing

We use TUnit for backend testing, ensuring high-quality and maintainable test code.

## License

[Add License Information]