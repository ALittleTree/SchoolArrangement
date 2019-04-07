using ContosoUniversity.Models;
using System;
using System.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new[]
            {
                new Student { Name = "Alexander",
                    EnrollmentDate = DateTime.Parse("2010-09-01"),EnrollmentCount=10 },
                new Student { Name = "Alonso",
                    EnrollmentDate = DateTime.Parse("2012-09-01") ,EnrollmentCount=10},
                new Student { Name = "Anand",
                    EnrollmentDate = DateTime.Parse("2013-09-01"),EnrollmentCount=10 },
                new Student { Name = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2012-09-01"),EnrollmentCount=10 },
                new Student { Name = "Li",
                    EnrollmentDate = DateTime.Parse("2012-09-01") ,EnrollmentCount=10},
                new Student { Name = "Justice",
                    EnrollmentDate = DateTime.Parse("2011-09-01") ,EnrollmentCount=10},
                new Student { Name = "Norman",
                    EnrollmentDate = DateTime.Parse("2013-09-01") ,EnrollmentCount=10},
                new Student { Name = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01") ,EnrollmentCount=10}
            };

            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var instructors = new[]
            {
                new Instructor { Name = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { Name = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { Name = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { Name = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { Name = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") }
            };

            foreach (Instructor i in instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();

            var courses = new[]
            {
                new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3
                },
                new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3
                },
                new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3
                },
                new Course {CourseID = 1045, Title = "Calculus",       Credits = 4
                },
                new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4
                },
                new Course {CourseID = 2021, Title = "Composition",    Credits = 3
                },
                new Course {CourseID = 2042, Title = "Literature",     Credits = 4
                },
            };

            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var officeAssignments = new[]
            {
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.Name == "Fakhouri").ID,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.Name == "Harui").ID,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.Name == "Kapoor").ID,
                    Location = "Thompson 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                context.OfficeAssignments.Add(o);
            }
            context.SaveChanges();

            var courseInstructors = new[]
            {
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Kapoor").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Harui").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Zheng").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Zheng").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Fakhouri").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Harui").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Abercrombie").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Literature" ).CourseID,
                    InstructorID = instructors.Single(i => i.Name == "Abercrombie").ID
                    },
            };

            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignments.Add(ci);
            }
            context.SaveChanges();

            var enrollments = new[]
            {
                new Enrollment {
                    StudentID = students.Single(s => s.Name == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.Name == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.Name == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID
                    },
                new Enrollment {
                    StudentID = students.Single(s => s.Name == "Barzdukas").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Li").ID,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.Name == "Justice").ID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID
                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.Student.ID == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();
            var courseSchedules = new[]
            {
                new CourseSchedule
                {
                    StudentID = students.Single(s => s.Name == "Justice").ID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    CourseGuid=Guid.NewGuid(),
                    InstructorID = instructors.Single(i => i.Name == "Abercrombie").ID,
                    IsAskForLeave=false,
                    ScheduleDate=DateTime.Now.Date
                },
                new CourseSchedule
                {
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    CourseGuid=Guid.NewGuid(),
                    InstructorID = instructors.Single(i => i.Name == "Harui").ID,
                    StudentID = students.Single(s => s.Name == "Alonso").ID,
                    IsAskForLeave=true,
                    ScheduleDate=DateTime.Now.Date
                }
            };

            foreach (var course in courseSchedules)
            {
                context.CourseSchedule.Add(course);
            }
            context.SaveChanges();
        }
    }
}