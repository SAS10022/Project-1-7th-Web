# Course Management System API

A comprehensive ASP.NET Core Web API for managing an educational institutional system with courses, instructors, students, and enrollments. This project serves as a foundation for a full-featured course management system and can be extended with additional features like scheduling, grading systems, notifications, and more.

## Project Overview

This API implements a complete course management system with support for:
- **User Authentication & Authorization** - JWT-based authentication with role-based access control
- **Entity Relationships** - One-to-One, One-to-Many, and Many-to-Many relationships
- **Data Transfer Objects (DTOs)** - Validated request/response models
- **Service Layer** - Clean architecture with dependency injection
- **Database Optimization** - LINQ projections with Select and AsNoTracking for performance
- **RESTful API** - Well-documented endpoints with Swagger/OpenAPI

## Technologies Used

### Core Framework
- **ASP.NET Core 9.0** - High-performance web framework for building APIs
- **Entity Framework Core 9.0** - ORM for database access and management

### Database
- **SQL Server LocalDB** - Local relational database for development

### Authentication & Authorization
- **JWT (JSON Web Tokens)** - Stateless authentication mechanism
- **System.IdentityModel.Tokens.Jwt** - JWT token generation and validation
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT middleware for ASP.NET Core

### API Documentation
- **Swagger/Swashbuckle** - OpenAPI/Swagger documentation and UI

### Data Validation
- **System.ComponentModel.DataAnnotations** - Built-in attribute-based validation

## Project Structure

```
CourseManagementAPI/
├── Controllers/           # API endpoint handlers
│   ├── AuthController.cs
│   ├── InstructorsController.cs
│   ├── StudentsController.cs
│   ├── CoursesController.cs
│   └── EnrollmentsController.cs
├── Models/                # Entity models
│   ├── User.cs
│   ├── Instructor.cs
│   ├── InstructorProfile.cs
│   ├── Student.cs
│   ├── Course.cs
│   └── Enrollment.cs
├── DTOs/                  # Data transfer objects
│   ├── AuthDto.cs
│   ├── InstructorDto.cs
│   ├── StudentDto.cs
│   ├── CourseDto.cs
│   └── EnrollmentDto.cs
├── Services/              # Business logic layer
│   ├── AuthService.cs
│   ├── InstructorService.cs
│   ├── StudentService.cs
│   ├── CourseService.cs
│   └── EnrollmentService.cs
├── Auth/                  # Authentication utilities
│   ├── JwtTokenService.cs
│   └── PasswordService.cs
├── Data/                  # Database context
│   └── ApplicationDbContext.cs
├── Migrations/            # EF Core database migrations
├── appsettings.json       # Configuration
└── Program.cs             # Application startup
```

## Entity Relationships

### One-to-One: Instructor ↔ InstructorProfile
- An Instructor can have one InstructorProfile
- An InstructorProfile belongs to exactly one Instructor
- Deletion of an Instructor cascades to InstructorProfile

### One-to-Many: Instructor → Course
- An Instructor can teach multiple Courses
- A Course is taught by exactly one Instructor
- Deletion of an Instructor is restricted if courses exist

### Many-to-Many: Student ↔ Course (via Enrollment)
- A Student can enroll in multiple Courses
- A Course can have multiple Students enrolled
- The Enrollment join table stores enrollment date and grade
- Unique constraint prevents duplicate enrollments

## Setup Instructions

### Prerequisites
- .NET 9.0 SDK or later
- SQL Server (LocalDB included with Visual Studio)
- Visual Studio Code or Visual Studio

### Installation

1. **Clone or Extract the Project**
   ```bash
   cd CourseManagementAPI
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Update Database**
   ```bash
   dotnet ef database update
   ```
   This will create the database `CourseManagementDb` in SQL Server LocalDB with all tables and relationships.

4. **Run the Application**
   ```bash
   dotnet run
   ```
   The API will start on `https://localhost:7095` (HTTPS) or `http://localhost:5095` (HTTP)

5. **Access Swagger Documentation**
   Navigate to `https://localhost:7095/swagger/ui/` to view and test all endpoints interactively

## Authentication & Authorization

### Authentication flow

1. **Register a User**
   ```
   POST /api/auth/register
   ```
   Create a new user account

2. **Login**
   ```
   POST /api/auth/login
   ```
   Receive a JWT token for authenticated requests

3. **Use JWT Token**
   Include the token in the `Authorization` header:
   ```
   Authorization: Bearer YOUR_JWT_TOKEN_HERE
   ```

### Roles
- **Admin** - Full access to all endpoints including create, update, delete operations
- **Instructor** - Can manage students and courses; cannot delete system data
- **User** - Can only read data and enroll in courses

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token

### Instructors
- `GET /api/instructors` - Get all instructors
- `GET /api/instructors/{id}` - Get instructor by ID
- `POST /api/instructors` - Create instructor (Admin only)
- `PUT /api/instructors/{id}` - Update instructor (Admin only)
- `DELETE /api/instructors/{id}` - Delete instructor (Admin only)

### Students
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create student (Admin/Instructor)
- `PUT /api/students/{id}` - Update student (Admin/Instructor)
- `DELETE /api/students/{id}` - Delete student (Admin only)

### Courses
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `GET /api/courses/instructor/{instructorId}` - Get courses by instructor
- `POST /api/courses` - Create course (Admin/Instructor)
- `PUT /api/courses/{id}` - Update course (Admin/Instructor)
- `DELETE /api/courses/{id}` - Delete course (Admin only)

### Enrollments
- `GET /api/enrollments/{id}` - Get enrollment by ID
- `GET /api/enrollments/student/{studentId}` - Get student's enrollments
- `GET /api/enrollments/course/{courseId}` - Get course enrollments
- `POST /api/enrollments` - Create enrollment
- `PUT /api/enrollments/{id}` - Update enrollment grade (Admin/Instructor)
- `DELETE /api/enrollments/{id}` - Delete enrollment (Admin/Instructor)

## Database Configuration

### Connection String
Edit `appsettings.json` to change the database connection:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CourseManagementDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### JWT Settings
Modify JWT token configuration in `appsettings.json`:
```json
"JwtSettings": {
  "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long-change-this-in-production",
  "Issuer": "CourseManagementAPI",
  "Audience": "CourseManagementClient",
  "ExpirationHours": "24"
}
```

**IMPORTANT**: Change the `SecretKey` in production to a secure, randomly generated value.

## Key Features

### 1. DTO Validation
All DTOs use Data Annotations for validation:
- `[Required]` - Ensures field is provided
- `[MaxLength]` - Limits string length
- `[MinLength]` - Minimum string length
- `[EmailAddress]` - Validates email format
- `[Range]` - Limits numeric values

Validation errors return HTTP 400 responses before database operations.

### 2. LINQ Optimization
- **Select() Projections** - Only retrieves required fields instead of entire entities
- **AsNoTracking()** - Read-only queries don't track entities in memory
- **Async Operations** - ToListAsync, FirstOrDefaultAsync, and SaveChangesAsync for non-blocking database calls

### 3. Database Migrations
- Uses Entity Framework Core Code-First migrations
- Apply migrations with: `dotnet ef database update`
- Revert migrations with: `dotnet ef database update <TargetMigration>`

## Why HTTP-Only Cookies Are the Industry Standard for Authentication Security

While this API uses JWT tokens (suitable for mobile and SPA applications), HTTP-only cookies are preferred in traditional web applications for several reasons:

### Cookie Security Advantages

1. **Protection Against XSS (Cross-Site Scripting)**
   - HTTP-only cookies cannot be accessed by JavaScript code
   - Even if attackers inject malicious JavaScript, they cannot steal the authentication token
   - JWTs stored in localStorage or sessionStorage are vulnerable to XSS attacks

2. **Automatic Request Inclusion**
   - Browsers automatically include HTTP-only cookies in requests
   - No manual header configuration needed (reduces client-side code complexity)
   - Reduces the chance of developer error in token handling

3. **CSRF (Cross-Site Request Forgery) Protection**
   - Combined with CSRF tokens, provides robust protection
   - Browsers block requests from different origins when using cookies
   - Must be explicitly handled with JWT headers (defense in depth)

4. **Storage Security**
   - Never stored in client-side JavaScript accessible storage
   - Cannot be exfiltrated through XSS vulnerabilities
   - More resistant to data theft from browser storage

### JWT vs HTTP-Only Cookies

**JWT Tokens (Used in This API)**
- ✅ Great for stateless, distributed APIs
- ✅ Works well with mobile apps and single-page applications (SPAs)
- ✅ Suitable for microservices and cross-domain requests
- ❌ Vulnerable to XSS if stored in localStorage/sessionStorage
- ❌ Requires manual header management

**HTTP-Only Cookies**
- ✅ Better protection against XSS attacks
- ✅ Automatic browser handling
- ✅ Reduced client-side security complexity
- ❌ Subject to CSRF (requires additional CSRF tokens)
- ❌ Less suitable for cross-domain scenarios
- ❌ Stateful (server must track sessions)

### Best Practice Recommendation
For production applications, consider:
- **HTTP-only cookies + CSRF tokens** for traditional server-rendered web applications
- **Secure HTTP-only cookies** for SPAs with same-origin API calls
- **JWT tokens** when building APIs consumed by multiple clients (mobile, web, third-party)
- **Hybrid approach** - Use HTTP-only cookies for web UI, JWT for API access

## Performance Optimization

This API follows best practices for performance:

1. **Async/Await** - Non-blocking database operations prevent thread pool starvation
2. **LINQ Select Projections** - Query only needed fields, reducing data transfer
3. **AsNoTracking()** - Read-only queries avoid entity state tracking overhead
4. **Proper Indexing** - Unique constraints and foreign keys indexed for query performance
5. **Lazy Loading Prevention** - Manual eager loading prevents N+1 query problems
6. **Connection Pooling** - SQL Server connection pooling enabled by default

## Extending the Project

### Adding New Features

1. **Background Jobs**
   - Add Hangfire for scheduled tasks (send notifications, cleanup, reports)

2. **Email Notifications**
   - Implement SendGrid or SMTP for enrollment confirmations
   - Send course updates to students

3. **Advanced Grading**
   - Add grade calculation and GPA tracking
   - Implement grading rubrics

4. **Scheduling System**
   - Add course schedules and room assignments
   - Implement scheduling conflicts detection

5. **Administrative Reports**
   - Course enrollment reports
   - Student progress reports
   - Instructor performance metrics

6. **API Pagination**
   - Add Skip/Take parameters for large result sets
   - Implement cursor-based pagination

7. **Caching**
   - Add Redis for frequently accessed data
   - Cache instructor and course listings

## Troubleshooting

### Database Connection Issues
- Verify SQL Server (LocalDB) is running: `sqllocaldb info`
- Check connection string in `appsettings.json`
- Create instance: `sqllocaldb create mssqllocaldb`

### Migration Issues
- Delete `Migrations` folder and `appsettings.json` changes, then recreate
- Reset database: `dotnet ef database drop --force` then `dotnet ef database update`

### JWT Token Issues
- Ensure token includes proper expiration time
- Check that secret key is configured in `appsettings.json`
- Verify roles match authorization requirements on endpoints

## Development Commands

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run

# Create migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Drop database
dotnet ef database drop --force

# View all migrations
dotnet ef migrations list
```

## Testing with Swagger

1. Navigate to `https://localhost:7095/swagger/ui/`
2. Register a new user using the `/api/auth/register` endpoint
3. Login using the `/api/auth/login` endpoint
4. Copy the returned token (without "Bearer" prefix)
5. Click the "Authorize" button at the top of Swagger
6. Paste the token in the "Value" field with "Bearer" prefix: `Bearer YOUR_TOKEN`
7. Execute any protected endpoint

## License

This project is provided as-is for educational purposes.

## Author

Created for Web Engineering (ASP.NET Core) Course

---

**Last Updated**: April 2, 2026
