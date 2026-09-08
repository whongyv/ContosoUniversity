using ContosoUniversity.WebAPI.Services;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(StudentsService service, IConfiguration configuration)
        : BaseController(configuration)
    {
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
            var result = await service.GetAsync(sortOrder, searchString, pageIndex, pageSize);
            return result;
        }

        // GET: api/Students/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentVM>> GetByID([FromRoute] int id)
        {
            var student = await service.GetByIDAsync(id);
            return student;
        }

        // GET: api/Students/enrollment-date-groups
        [HttpGet("enrollment-date-groups")]
        public async Task<ActionResult<List<EnrollmentDateGroup>>> GetEnrollmentDateGroups()
        {
            var result = await service.GetEnrollmentDateGroupsAsync();
            return result;
        }

        // POST：api/Students
        [HttpPost]
        public async Task<ActionResult<StudentVM>> Post([FromBody] StudentVM studentVM)
        {
            var result = await service.CreateAsync(studentVM);
            return CreatedAtAction(nameof(GetByID), new { id = result.ID }, result);
        }

        // PUT: api/Students/5
        [HttpPut("{id:int}")]
        public async Task<NoContentResult> Put([FromRoute] int id, [FromBody] StudentVM studentVM)
        {
            await service.EditAsync(id, studentVM);
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id:int}")]
        public async Task<NoContentResult> Delete([FromRoute] int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
