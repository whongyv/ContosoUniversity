using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.Exceptions;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Services
{
    public class DepartmentsService(SchoolContext context)
    {
        public async Task<PaginationResult<DepartmentListVM>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await PaginationResult<DepartmentListVM>.Create(pageIndex, pageSize, context.Departments
                .OrderBy(d => d.DepartmentID)
                .Select(d => new DepartmentListVM
                {
                    DepartmentID = d.DepartmentID,
                    Name = d.Name,
                    Budget = d.Budget,
                    StartDate = d.StartDate,
                    Administrator = d.Administrator != null ? d.Administrator.FullName : "No administrator"
                }));
        }

        public async Task<DepartmentDetailVM> GetByDepartmentIDAsync(int departmentID)
        {
            var department = await context.Departments
                .Where(d => d.DepartmentID == departmentID)
                .Select(d => new DepartmentDetailVM
                {
                    DepartmentID = d.DepartmentID,
                    Name = d.Name,
                    Budget = d.Budget,
                    StartDate = d.StartDate,
                    InstructorID = d.InstructorID,
                    Administrator = d.Administrator != null ? d.Administrator.FullName : "No administrator",
                    Token = Convert.ToBase64String(d.ConcurrencyToken),
                    Courses = d.Courses.Select(c => c.Title).ToList()
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Department with ID {departmentID} not found.");

            return department;
        }

        public async Task<DepartmentDetailVM> CreateAsync(CreateDepartmentVM departmentVM)
        {
            var department = new Department
            {
                Name = departmentVM.Name,
                Budget = departmentVM.Budget,
                StartDate = departmentVM.StartDate,
                InstructorID = departmentVM.InstructorID
            };

            context.Add(department);
            await context.SaveChangesAsync();

            return new DepartmentDetailVM
            {
                DepartmentID = department.DepartmentID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                InstructorID = department.InstructorID,
                Administrator = department.InstructorID != null ?
                    (await context.Instructors.FirstOrDefaultAsync(i => i.ID == department.InstructorID))?.FullName
                    : "No administrator",
                Token = Convert.ToBase64String(department.ConcurrencyToken),
                Courses = []
            };
        }

        public async Task<string> UpdateAsync(int departmentID, UpdateDepartmentVM departmentVM, string token)
        {
            var department = await GetDepartmentOrThrowAsync(departmentID);
            department.Name = departmentVM.Name;
            department.Budget = departmentVM.Budget;
            department.StartDate = departmentVM.StartDate;
            department.InstructorID = departmentVM.InstructorID;
            context.Entry(department)
                   .Property(d => d.ConcurrencyToken).OriginalValue = Convert.FromBase64String(token);
            await context.SaveChangesAsync();
            return Convert.ToBase64String(department.ConcurrencyToken);
        }

        public async Task DeleteAsync(int departmentID, string token)
        {
            var department = await GetDepartmentOrThrowAsync(departmentID);
            context.Entry(department)
                   .Property(d => d.ConcurrencyToken).OriginalValue = Convert.FromBase64String(token);
            context.Remove(department);
            await context.SaveChangesAsync();
        }

        private async Task<Department> GetDepartmentOrThrowAsync(int departmentID)
        {
            return await context.Departments.FindAsync(departmentID)
                ?? throw new NotFoundException($"Department with ID {departmentID} not found.");
        }
    }
}
