using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class CoursesService(SchoolContext context)
    {
        public async Task<PaginationResult<CourseVM>> GetPagedAsync(int pageIndex, int pageSize)
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
                .FirstOrDefaultAsync(c => c.CourseID == courseID)
                ?? throw new NotFoundException($"Course with ID {courseID} not found.");

            course.StudentGrades = await context.Enrollments
                .Where(e => e.CourseID == courseID)
                .Select(e => new StudentGrade
                {
                    Student = e.Student.FullName,
                    Grade = e.Grade.ToString()
                })
                .ToListAsync();

            course.Instructors = await context.Courses
                .Where(c => c.CourseID == courseID)
                .Select(c => c.Instructors.Select(i => i.FullName).ToList())
                .FirstOrDefaultAsync();

            return course;
        }

        public async Task<List<StudentGrade>> GetStudentGradesByCourseIdAsync(int courseID)
        {
            return await context.Enrollments
                .Where(e => e.CourseID == courseID)
                .Select(e => new StudentGrade
                {
                    Student = e.Student.FullName,
                    Grade = e.Grade.HasValue ? e.Grade.ToString() : "No grade"
                })
                .ToListAsync();
        }

        public async Task<CourseVM> CreateAsync(CourseVM courseVM)
        {
            if (await context.Courses.AnyAsync(c => c.CourseID == courseVM.CourseID))
            {
                throw new NotFoundException($"Course with ID {courseVM.CourseID} already exists.");
            }

            await EnsureDepartmentExistsAsync(courseVM.DepartmentID);

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

        public async Task UpdateAsync(int courseID, CourseVM courseVM)
        {
            var course = await GetCourseOrThrowAsync(courseID);

            await EnsureDepartmentExistsAsync(courseVM.DepartmentID);

            course.Title = courseVM.Title;
            course.Credits = courseVM.Credits;
            course.DepartmentID = courseVM.DepartmentID;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int courseID)
        {
            var course = await GetCourseOrThrowAsync(courseID);
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
        }

        public Task<int> ScaleCreditsAsync(int multiplier)
        {
            var sql = $"UPDATE Courses SET Credits = Credits * {multiplier}";
            return context.Database.ExecuteSqlRawAsync(sql);
        }

        private async Task<Course> GetCourseOrThrowAsync(int courseID)
        {
            var course = await context.Courses.FindAsync(courseID)
                ?? throw new NotFoundException($"Course with ID {courseID} not found.");
            return course;
        }

        private async Task EnsureDepartmentExistsAsync(int departmentID)
        {
            if (!await context.Departments.AnyAsync(d => d.DepartmentID == departmentID))
            {
                throw new NotFoundException($"Department with ID {departmentID} does not exist.");
            }
        }
    }
}
