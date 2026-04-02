using CourseManagementAPI.Models;

namespace CourseManagementAPI.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Users.Any() || context.Instructors.Any() || context.Students.Any() || context.Courses.Any())
        {
            return; // Database has been seeded
        }

        // Add sample users
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@university.edu",
            PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", // "admin" hashed
            Role = "Admin"
        };

        var instructorUser = new User
        {
            Username = "instructor1",
            Email = "instructor1@university.edu",
            PasswordHash = "6b98c78c94d3e8c18708c13d77c4ea3e", // user password
            Role = "Instructor"
        };

        var studentUser = new User
        {
            Username = "student1",
            Email = "student1@university.edu",
            PasswordHash = "482c811da5d5b4bc6d497ffa98491e38", // password
            Role = "User"
        };

        context.Users.AddRange(adminUser, instructorUser, studentUser);
        context.SaveChanges();

        // Add sample instructors
        var instructor1 = new Instructor
        {
            FirstName = "Dr. John",
            LastName = "Smith",
            Email = "john.smith@university.edu",
            Department = "Computer Science"
        };

        var instructor2 = new Instructor
        {
            FirstName = "Prof. Jane",
            LastName = "Doe",
            Email = "jane.doe@university.edu",
            Department = "Mathematics"
        };

        context.Instructors.AddRange(instructor1, instructor2);
        context.SaveChanges();

        // Add instructor profiles
        var profile1 = new InstructorProfile
        {
            InstructorId = instructor1.Id,
            PhoneNumber = "555-0101",
            OfficeLocation = "Building A, Room 201",
            Biography = "PhD in Computer Science from MIT. 10 years of experience in software development and education.",
            YearsOfExperience = 10
        };

        var profile2 = new InstructorProfile
        {
            InstructorId = instructor2.Id,
            PhoneNumber = "555-0102",
            OfficeLocation = "Building B, Room 305",
            Biography = "PhD in Pure Mathematics. 8 years of teaching experience.",
            YearsOfExperience = 8
        };

        context.InstructorProfiles.AddRange(profile1, profile2);
        context.SaveChanges();

        // Add sample students
        var student1 = new Student
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@university.edu",
            StudentId = "STU001",
            Major = "Computer Science"
        };

        var student2 = new Student
        {
            FirstName = "Bob",
            LastName = "Williams",
            Email = "bob.williams@university.edu",
            StudentId = "STU002",
            Major = "Mathematics"
        };

        var student3 = new Student
        {
            FirstName = "Carol",
            LastName = "Brown",
            Email = "carol.brown@university.edu",
            StudentId = "STU003",
            Major = "Computer Science"
        };

        context.Students.AddRange(student1, student2, student3);
        context.SaveChanges();

        // Add sample courses
        var course1 = new Course
        {
            CourseCode = "CS101",
            Title = "Introduction to Programming",
            Description = "Learn the fundamentals of programming using C#",
            Credits = 3,
            MaxCapacity = 30,
            InstructorId = instructor1.Id
        };

        var course2 = new Course
        {
            CourseCode = "CS201",
            Title = "Data Structures",
            Description = "Advanced data structures and algorithms",
            Credits = 4,
            MaxCapacity = 25,
            InstructorId = instructor1.Id
        };

        var course3 = new Course
        {
            CourseCode = "MATH101",
            Title = "Calculus I",
            Description = "Introduction to differential and integral calculus",
            Credits = 4,
            MaxCapacity = 35,
            InstructorId = instructor2.Id
        };

        context.Courses.AddRange(course1, course2, course3);
        context.SaveChanges();

        // Add sample enrollments
        var enrollments = new[]
        {
            new Enrollment { StudentId = student1.Id, CourseId = course1.Id, Grade = null },
            new Enrollment { StudentId = student1.Id, CourseId = course2.Id, Grade = "A" },
            new Enrollment { StudentId = student2.Id, CourseId = course3.Id, Grade = null },
            new Enrollment { StudentId = student3.Id, CourseId = course1.Id, Grade = "B+" },
            new Enrollment { StudentId = student3.Id, CourseId = course2.Id, Grade = null }
        };

        context.Enrollments.AddRange(enrollments);
        context.SaveChanges();
    }
}
