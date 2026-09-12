using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.ViewModels;

namespace ContosoUniversity.WebAPI.Services
{
    public class DepartmentsService(SchoolContext context)
    {
        public async Task<PaginationResult<DepartmentListVM>> GetPagedAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<DepartmentDetailVM> GetByDepartmentIDAsync(int departmentID)
        {
            throw new NotImplementedException();
        }

        public async Task<DepartmentDetailVM> CreateAsync(CreateDepartmentVM departmentVM)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, UpdateDepartmentVM departmentVM)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int departmentID)
        {
            throw new NotImplementedException();
        }
    }
}
