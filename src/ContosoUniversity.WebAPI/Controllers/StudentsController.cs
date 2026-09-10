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
        public async Task<ActionResult<PaginationResult<StudentListVM>>> Get(
            [FromQuery] string sortOrder,
            [FromQuery] string searchString,
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var result = await service.GetPagedAsync(sortOrder, searchString, pageIndex, pageSize);
            return Ok(result);
        }

        // GET: api/Students/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDetailVM>> Get([FromRoute] int id)
        {
            var result = await service.GetByIDAsync(id);
            return Ok(result);
        }

        // GET: api/Students/enrollment-stats
        [HttpGet("enrollment-stats")]
        public async Task<ActionResult<List<EnrollmentDateGroup>>> GetEnrollmentStats()
        {
            var result = await service.GetEnrollmentStatsAsync();
            return Ok(result);
        }

        // POST：api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDetailVM>> Post([FromBody] CreateStudentVM studentVM)
        {
            var result = await service.CreateAsync(studentVM);
            return CreatedAtAction(nameof(Get), new { id = result.ID }, result);
        }

        // PUT: api/Students/5
        [HttpPut("{id:int}")]
        public async Task<NoContentResult> Put([FromRoute] int id, [FromBody] UpdateStudentVM studentVM)
        {
            await service.UpdateAsync(id, studentVM);
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
