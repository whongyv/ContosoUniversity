using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(SchoolContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly SchoolContext _context = context;
        private readonly int _defaultPageSize = configuration.GetValue("PageSize", 3);

        [HttpGet]
        public async Task<ActionResult<PaginationResult<StudentListVM>>> GetStudents(
            [FromQuery] string sortOrder,
            [FromQuery] string searchString,
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.LastName.Contains(searchString)
                    || s.FirstMidName.Contains(searchString));
            }

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

            return await PaginationResult<StudentListVM>.Create(pageIndex, pageSize,
                query.Select(s => new StudentListVM
                {
                    ID = s.ID,
                    LastName = s.LastName,
                    FirstMidName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate
                }));
        }
    }
}
