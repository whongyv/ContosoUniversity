using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class CoursesService(SchoolContext context)
    {
        public async Task<PaginationResult<CourseVM>> GetAsync(int pageIndex, int pageSize)
        {
            return await PaginationResult<CourseVM>.Create(pageIndex, pageSize, context.Courses
                .OrderBy(c => c.CourseID)
                .Select(c => new CourseVM
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentID = c.DepartmentID,
                    Department = c.Department.Name
                }));
        }

        public async Task<CourseDetailVM> GetByCourseIDAsync(int courseID)
        {
            var course = await context.Courses
                .Select(c => new CourseDetailVM
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentID = c.DepartmentID,
                    Department = c.Department.Name
                })
                .FirstOrDefaultAsync(c => c.CourseID == courseID);

            course.Enrollments = await context.Enrollments
                .Where(e => e.CourseID == courseID)
                .Select(e => new CourseEnrollment
                {
                    Student = e.Student.FullName,
                    Grade = e.Grade.ToString()
                })
                .ToListAsync();

            course.Instructors = await context.Courses
                .Where(c => c.CourseID == courseID)
                .Select(c => c.Instructors.Select(i => i.FullName).ToList())
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NotFoundException($"Course with ID {courseID} not found.");
            }

            return course;
        }

        public async Task<CourseVM> CreateAsync(CourseVM courseVM)
        {
            if (await context.Courses.AnyAsync(c => c.CourseID == courseVM.CourseID))
            {
                throw new NotFoundException($"Course with ID {courseVM.CourseID} already exists.");
            }

            if (!await context.Departments.AnyAsync(d => d.DepartmentID == courseVM.DepartmentID))
            {
                throw new NotFoundException($"Department with ID {courseVM.DepartmentID} does not exist.");
            }

            var course = new Course
            {
                CourseID = courseVM.CourseID,
                Title = courseVM.Title,
                Credits = courseVM.Credits,
                DepartmentID = courseVM.DepartmentID
            };

            context.Add(course);
            await context.SaveChangesAsync();

            return new CourseVM
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentID = course.DepartmentID
            };
        }
    }
}
