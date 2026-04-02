# Course Management API - GitHub Copilot Instructions

## Project Overview
ASP.NET Core Web API for managing an educational system with JWT authentication, Entity Framework Core, and comprehensive API documentation.

## Technologies
- ASP.NET Core 9.0 Web API
- Entity Framework Core 9.0 with SQL Server
- JWT Authentication with role-based authorization
- Swagger/Swashbuckle documentation
- Data validation with annotations

## Project Structure
- `Models/` - Entity models with relationships
- `DTOs/` - Data transfer objects for CRUD operations
- `Services/` - Business logic with dependency injection
- `Auth/` - Authentication and JWT services
- `Controllers/` - RESTful API endpoints
- `Data/` - Entity Framework DB context
- `Migrations/` - Database schema versions

## Key Patterns
- Dependency injection throughout
- LINQ Select projections for optimization
- AsNoTracking() for read-only queries
- Async/await for all database operations
- DTO validation before database operations

## Running the Project
1. `dotnet restore` - Install packages
2. `dotnet ef database update` - Apply migrations
3. `dotnet run` - Start API server
4. Navigate to `https://localhost:7095/swagger/ui/` for documentation

## API Authentication
1. Register: `POST /api/auth/register`
2. Login: `POST /api/auth/login` - Returns JWT token
3. Include in requests: `Authorization: Bearer YOUR_TOKEN`

## Development Notes
- All endpoints return DTOs, never raw entities
- Validation happens in DTOs before service layer
- Read operations use AsNoTracking for performance
- Role-based access control on sensitive endpoints
- HTTP 400 for validation errors, 401 for auth, 403 for authorization, 404 for not found
