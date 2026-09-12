using ContosoUniversity.WebAPI.Services;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController(InstructorsService service, IConfiguration configuration)
        : BaseController(configuration)
    {
        // GET: api/Instructors
        [HttpGet]
        public async Task<ActionResult<PaginationResult<InstructorListVM>>> Get(
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var result = await service.GetPagedAsync(pageIndex, pageSize);
            return Ok(result);
        }

        // GET: api/Instructors/5/courses
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<List<CourseListVM>>> GetCourses([FromRoute] int id)
        {
            var result = await service.GetCoursesByInstructorIDAsync(id);
            return Ok(result);
        }

        // GET: api/Instructors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorDetailVM>> Get([FromRoute] int id)
        {
            var result = await service.GetByIDAsync(id);
            return Ok(result);
        }

        // POST: api/Instructors
        [HttpPost]
        public async Task<ActionResult<InstructorDetailVM>> Post([FromBody] CreateInstructorVM instructorVM)
        {
            var result = await service.CreateAsync(instructorVM);
            return CreatedAtAction(nameof(Get), new { id = result.ID }, result);
        }

        // PUT: api/Instructors/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateInstructorVM instructorVM)
        {
            await service.UpdateAsync(id, instructorVM);
            return NoContent();
        }

        // DELETE: api/Instructors/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
