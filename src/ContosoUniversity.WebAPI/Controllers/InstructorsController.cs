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
        public async Task<ActionResult<PaginationResult<InstructorVM>>> Get(
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var result = await service.GetPagedAsync(pageIndex, pageSize);
            return result;
        }

        // GET: api/Instructors/5/courses
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<List<CourseVM>>> GetCourses([FromRoute] int id)
        {
            var result = await service.GetCoursesByInstructorIDAsync(id);
            return result;
        }

        // GET: api/Instructors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorVM>> Get([FromRoute] int id)
        {
            var result = await service.GetByIDAsync(id);
            return result;
        }
    }
}
