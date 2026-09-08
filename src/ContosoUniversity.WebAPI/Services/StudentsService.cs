using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class StudentsService(SchoolContext context)
    {
        public async Task<PaginationResult<StudentVM>> GetPagedAsync(
            string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            var query = context.Students.AsQueryable();

            // Build the query based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.LastName.Contains(searchString)
                    || s.FirstMidName.Contains(searchString));
            }

            // Determine the sort order
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "name";
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(s => s.LastName),
                "date" => query.OrderBy(s => s.EnrollmentDate),
                "date_desc" => query.OrderByDescending(s => s.EnrollmentDate),
                _ => query.OrderBy(s => s.LastName),
            };

            return await PaginationResult<StudentVM>.Create(pageIndex, pageSize,
                query.Select(s => new StudentVM
                {
                    ID = s.ID,
                    LastName = s.LastName,
                    FirstName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate
                }));
        }

        public async Task<StudentDetailVM> GetByIDAsync(int id)
        {
            var student = await context.Students
                 .Select(s => new StudentDetailVM
                 {
                     ID = s.ID,
                     LastName = s.LastName,
                     FirstName = s.FirstMidName,
                     EnrollmentDate = s.EnrollmentDate,
                     Enrollments = s.Enrollments.Select(e => new StudentEnrollment
                     {
                         Course = e.Course.Title,
                         Grade = e.Grade.ToString()
                     }).ToList()
                 })
                 .FirstOrDefaultAsync(s => s.ID == id)
                 ?? throw new NotFoundException($"Student with ID {id} not found.");

            return student;
        }

        public async Task<List<EnrollmentDateGroup>> GetEnrollmentStatsAsync()
        {
            return await context.Students
                .OrderBy(s => s.EnrollmentDate)
                .GroupBy(s => s.EnrollmentDate)
                .Select(g => new EnrollmentDateGroup
                {
                    EnrollmentDate = g.Key,
                    StudentCount = g.Count()
                })
                .ToListAsync();
        }

        public async Task<StudentVM> CreateAsync(StudentVM studentVM)
        {
            var student = new Student
            {
                LastName = studentVM.LastName,
                FirstMidName = studentVM.FirstName,
                EnrollmentDate = studentVM.EnrollmentDate
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();

            return new StudentVM
            {
                ID = student.ID,
                LastName = student.LastName,
                FirstName = student.FirstMidName,
                EnrollmentDate = student.EnrollmentDate
            };
        }

        public async Task UpdateAsync(int id, StudentVM studentVM)
        {
            var student = await GetStudentOrThrowAsync(id);
            student.LastName = studentVM.LastName;
            student.FirstMidName = studentVM.FirstName;
            student.EnrollmentDate = studentVM.EnrollmentDate;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await GetStudentOrThrowAsync(id);
            context.Students.Remove(student);
            await context.SaveChangesAsync();
        }

        private async Task<Student> GetStudentOrThrowAsync(int id)
        {
            return await context.Students.FindAsync(id)
                ?? throw new NotFoundException($"Student with ID {id} not found.");
        }
    }
}
