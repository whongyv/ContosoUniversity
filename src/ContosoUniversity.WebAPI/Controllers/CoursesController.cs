using ContosoUniversity.WebAPI.Services;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(CoursesService service, IConfiguration configuration)
        : BaseController(configuration)
    {
        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<PaginationResult<CourseVM>>> Get(
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var result = await service.GetPagedAsync(pageIndex, pageSize);
            return result;
        }

        // GET: api/Courses/5
        [HttpGet("{courseID:int}")]
        public async Task<ActionResult<CourseDetailVM>> Get(int courseID)
        {
            var result = await service.GetByCourseIDAsync(courseID);
            return result;
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseVM>> Post(CourseVM courseVM)
        {
            var result = await service.CreateAsync(courseVM);
            return CreatedAtAction(nameof(Get), new { courseID = result.CourseID }, result);
        }

        // POST: /api/Courses/actions/scale-credits
        [HttpPost("actions/scale-credits")]
        public async Task<ActionResult<int>> ScaleCredits([FromQuery][Required][Range(1, 5)] int multiplier)
        {
            var updatedCount = await service.ScaleCreditsAsync(multiplier);
            return updatedCount;
        }

        // PUT: api/Courses/5
        [HttpPut("{courseID:int}")]
        public async Task<ActionResult> Put(int courseID, CourseVM courseVM)
        {
            await service.UpdateAsync(courseID, courseVM);
            return NoContent();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{courseID:int}")]
        public async Task<ActionResult> Delete(int courseID)
        {
            await service.DeleteAsync(courseID);
            return NoContent();
        }
    }
}
