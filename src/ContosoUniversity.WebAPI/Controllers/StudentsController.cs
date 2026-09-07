using ContosoUniversity.WebAPI.Data;
using ContosoUniversity.WebAPI.Entities;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(SchoolContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly SchoolContext _context = context;
        private readonly int _defaultPageSize = configuration.GetValue("PageSize", 3);

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<PaginationResult<StudentVM>>> Get(
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

            return await PaginationResult<StudentVM>.Create(pageIndex, pageSize,
                query.Select(s => new StudentVM
                {
                    ID = s.ID,
                    LastName = s.LastName,
                    FirstName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate
                }));
        }

        // GET: api/Students/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDetailVM>> GetByID([FromRoute] int id)
        {
            var student = await _context.Students
                .Where(s => s.ID == id)
                .Select(s => new StudentDetailVM
                {
                    ID = s.ID,
                    LastName = s.LastName,
                    FirstName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate,
                    Enrollments = s.Enrollments.Select(e => new EnrollmentVM
                    {
                        Course = e.Course.Title,
                        Grade = e.Grade.ToString()
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // GET: api/Students/enrollment-date-groups
        [HttpGet("enrollment-date-groups")]
        public async Task<ActionResult<List<EnrollmentDateGroup>>> GetEnrollmentDateGroups()
        {
            return await _context.Students
                .OrderBy(s => s.EnrollmentDate)
                .GroupBy(s => s.EnrollmentDate)
                .Select(g => new EnrollmentDateGroup
                {
                    EnrollmentDate = g.Key,
                    StudentCount = g.Count()
                })
                .ToListAsync();
        }

        // POST：api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDetailVM>> Post([FromBody] StudentVM studentVM)
        {
            var student = new Student
            {
                LastName = studentVM.LastName,
                FirstMidName = studentVM.FirstName,
                EnrollmentDate = studentVM.EnrollmentDate
            };

            _context.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByID), new { id = student.ID }, new StudentDetailVM
            {
                ID = student.ID,
                LastName = student.LastName,
                FirstName = student.FirstMidName,
                EnrollmentDate = student.EnrollmentDate,
                Enrollments = []
            });
        }

        // PUT: api/Students/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] StudentVM studentVM)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.LastName = studentVM.LastName;
            student.FirstMidName = studentVM.FirstName;
            student.EnrollmentDate = studentVM.EnrollmentDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
