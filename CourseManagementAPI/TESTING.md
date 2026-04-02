# API Testing Guide

This document provides examples for testing all Course Management API endpoints using curl or Postman.

## Base URL
- Development: `https://localhost:7095` or `http://localhost:5095`

## Authentication Tests

### 1. Register a New User
```bash
curl -X POST "https://localhost:7095/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "testuser@example.com",
    "password": "Password123!"
  }'
```

### 2. Login and Get JWT Token
```bash
curl -X POST "https://localhost:7095/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Password123!"
  }'
```

**Response:**
```json
{
  "userId": 1,
  "username": "testuser",
  "email": "testuser@example.com",
  "role": "User",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

Copy the `token` value for use in authenticated requests.

## Authenticated Request Example
Include the JWT token in all subsequent requests:
```bash
curl -X GET "https://localhost:7095/api/instructors" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

## Instructor Tests

### Get All Instructors
```bash
curl -X GET "https://localhost:7095/api/instructors" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get Instructor by ID
```bash
curl -X GET "https://localhost:7095/api/instructors/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Create New Instructor (Admin Only)
```bash
curl -X POST "https://localhost:7095/api/instructors" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Dr. Michael",
    "lastName": "Johnson",
    "email": "michael.johnson@university.edu",
    "department": "Engineering"
  }'
```

### Update Instructor (Admin Only)
```bash
curl -X PUT "https://localhost:7095/api/instructors/1" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Dr. Michael",
    "lastName": "Anderson",
    "email": "michael.anderson@university.edu",
    "department": "Computer Science"
  }'
```

### Delete Instructor (Admin Only)
```bash
curl -X DELETE "https://localhost:7095/api/instructors/1" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN"
```

## Student Tests

### Get All Students
```bash
curl -X GET "https://localhost:7095/api/students" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Create New Student (Admin/Instructor)
```bash
curl -X POST "https://localhost:7095/api/students" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Emma",
    "lastName": "Davis",
    "email": "emma.davis@university.edu",
    "studentId": "STU12345",
    "major": "Computer Science"
  }'
```

### Update Student (Admin/Instructor)
```bash
curl -X PUT "https://localhost:7095/api/students/1" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Emma",
    "lastName": "Smith",
    "major": "Information Technology"
  }'
```

## Course Tests

### Get All Courses
```bash
curl -X GET "https://localhost:7095/api/courses" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get Courses by Instructor
```bash
curl -X GET "https://localhost:7095/api/courses/instructor/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Create New Course (Admin/Instructor)
```bash
curl -X POST "https://localhost:7095/api/courses" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "courseCode": "CS205",
    "title": "Web Development with ASP.NET Core",
    "description": "Build modern web applications using ASP.NET Core framework",
    "credits": 4,
    "maxCapacity": 25,
    "instructorId": 1
  }'
```

### Update Course (Admin/Instructor)
```bash
curl -X PUT "https://localhost:7095/api/courses/1" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Web Development with ASP.NET Core - Advanced",
    "credits": 3,
    "maxCapacity": 30
  }'
```

## Enrollment Tests

### Get Student's Enrollments
```bash
curl -X GET "https://localhost:7095/api/enrollments/student/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get Course Enrollments
```bash
curl -X GET "https://localhost:7095/api/enrollments/course/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Enroll Student in Course
```bash
curl -X POST "https://localhost:7095/api/enrollments" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "studentId": 1,
    "courseId": 2
  }'
```

### Update Enrollment Grade (Admin/Instructor)
```bash
curl -X PUT "https://localhost:7095/api/enrollments/1" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "grade": "A"
  }'
```

### Remove Enrollment (Admin/Instructor)
```bash
curl -X DELETE "https://localhost:7095/api/enrollments/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## Using Postman

### 1. Import API into Postman
- Open Postman
- Click "Import"
- URL: `https://localhost:7095/openapi/v1.json` (or `http://localhost:5095`)
- Click "Import"

### 2. Set Up Environment Variables
- Create new environment called "Dev"
- Add variable: `base_url` = `https://localhost:7095`
- Add variable: `token` = `(populated after login)`

### 3. Use Variables in Requests
Replace hardcoded URLs with `{{base_url}}`

### 4. Auto-populate Token
After login request:
- Select the login response
- Click "Tests" tab
- Add: `pm.environment.set("token", pm.response.json().token);`
- This automatically updates token for subsequent requests

## Common HTTP Status Codes

- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `204 No Content` - Update/Delete successful (no response body)
- `400 Bad Request` - Validation failed or malformed request
- `401 Unauthorized` - Missing or invalid JWT token
- `403 Forbidden` - Authenticated but lacks required role
- `404 Not Found` - Resource doesn't exist
- `409 Conflict` - Duplicate enrollment or unique constraint violation
- `500 Internal Server Error` - Server-side error

## Sample Test Scenario

1. **Register and Login**
   ```bash
   # Register
   curl -X POST "https://localhost:7095/api/auth/register" \
     -H "Content-Type: application/json" \
     -d '{
       "username": "student123",
       "email": "student123@example.com",
       "password": "SecurePassword123!"
     }'

   # Login
   curl -X POST "https://localhost:7095/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{
       "username": "student123",
       "password": "SecurePassword123!"
     }'
   ```

2. **View Available Courses**
   ```bash
   curl -X GET "https://localhost:7095/api/courses" \
     -H "Authorization: Bearer YOUR_TOKEN"
   ```

3. **Enroll in a Course**
   ```bash
   curl -X POST "https://localhost:7095/api/enrollments" \
     -H "Authorization: Bearer YOUR_TOKEN" \
     -H "Content-Type: application/json" \
     -d '{
       "studentId": 1,
       "courseId": 1
     }'
   ```

4. **View Your Enrollments**
   ```bash
   curl -X GET "https://localhost:7095/api/enrollments/student/1" \
     -H "Authorization: Bearer YOUR_TOKEN"
   ```

## Swagger UI Testing

1. Run the application: `dotnet run`
2. Navigate to `https://localhost:7095/swagger/ui/`
3. Click "Try it out" on any endpoint
4. Authorization automatically included after login
5. See request/response examples

## Validation Error Example

Request with missing required field:
```bash
curl -X POST "https://localhost:7095/api/students" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John"
  }'
```

Response (400 Bad Request):
```json
{
  "errors": {
    "lastName": ["The Last name field is required."],
    "email": ["The Email field is required."],
    "studentId": ["The Student ID field is required."],
    "major": ["The Major field is required."]
  },
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

## Troubleshooting

### SSL Certificate Error
Use `--insecure` flag with curl (development only):
```bash
curl --insecure -X GET "https://localhost:7095/api/instructors" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Invalid Token Error
- Ensure token is current (tokens expire after 24 hours by default)
- Include "Bearer " prefix when using token
- Check token hasn't been modified

### Roll-based Authorization Error (403)
- Verify user has required role for endpoint
- Test endpoints allow specific roles (e.g., Admin-only)
- Check role was properly assigned during registration/creation
