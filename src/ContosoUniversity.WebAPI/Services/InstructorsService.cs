using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class InstructorsService(SchoolContext context)
    {
        public async Task<PaginationResult<InstructorVM>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await PaginationResult<InstructorVM>.Create(pageIndex, pageSize, context.Instructors
                .OrderBy(i => i.ID)
                .Select(i => new InstructorVM
                {
                    ID = i.ID,
                    LastName = i.LastName,
                    FirstName = i.FirstMidName,
                    HireDate = i.HireDate,
                    Office = i.OfficeAssignment != null ? i.OfficeAssignment.Location : null,
                    Courses = i.Courses
                    .Select(c => new InstructorCourse
                    {
                        CourseID = c.CourseID,
                        Title = c.Title,
                    })
                    .ToList()
                }));
        }

        public async Task<List<CourseVM>> GetCoursesByInstructorIDAsync(int insructorID)
        {
            return await context.Instructors
                 .Where(i => i.ID == insructorID)
                 .SelectMany(i => i.Courses)
                 .Select(c => new CourseVM
                 {
                     CourseID = c.CourseID,
                     Title = c.Title,
                     Credits = c.Credits,
                     DepartmentID = c.DepartmentID,
                     Department = c.Department.Name
                 })
                 .ToListAsync();
        }

        public async Task<InstructorVM> GetByIDAsync(int id)
        {
            var instructor = await context.Instructors
                 .Select(i => new InstructorVM
                 {
                     ID = i.ID,
                     LastName = i.LastName,
                     FirstName = i.FirstMidName,
                     HireDate = i.HireDate,
                     Office = i.OfficeAssignment != null ? i.OfficeAssignment.Location : null,
                     Courses = i.Courses
                     .Select(c => new InstructorCourse
                     {
                         CourseID = c.CourseID,
                         Title = c.Title,
                     })
                     .ToList()
                 })
                 .FirstOrDefaultAsync(i => i.ID == id)
                 ?? throw new NotFoundException($"Instructor with ID {id} not found.");

            return instructor;
        }
    }
}
