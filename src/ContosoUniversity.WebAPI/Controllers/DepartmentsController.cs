using ContosoUniversity.WebAPI.Services;
using ContosoUniversity.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(DepartmentsService service, IConfiguration configuration)
        : BaseController(configuration)
    {
        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<PaginationResult<DepartmentListVM>>> Get(
            [FromQuery][Range(1, int.MaxValue)] int pageIndex = 1,
            [FromQuery][Range(1, 100)] int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? _defaultPageSize : pageSize;
            var result = await service.GetPagedAsync(pageIndex, pageSize);
            return Ok(result);
        }

        // GET: api/Departments/5
        [HttpGet("{departmentID:int}")]
        public async Task<ActionResult<DepartmentDetailVM>> Get([FromRoute] int departmentID)
        {
            var result = await service.GetByDepartmentIDAsync(departmentID);
            return Ok(result);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDetailVM>> Post([FromBody] CreateDepartmentVM departmentVM)
        {
            var result = await service.CreateAsync(departmentVM);
            return CreatedAtAction(nameof(Get), new { departmentID = result.DepartmentID }, result);
        }

        // PUT: api/Departments/5
        [HttpPut("{departmentID:int}")]
        public async Task<ActionResult> Put([FromRoute] int departmentID, [FromBody] UpdateDepartmentVM departmentVM)
        {
            await service.UpdateAsync(departmentID, departmentVM);
            return NoContent();
        }

        // DELETE: api/Departments/5
        [HttpDelete("{departmentID:int}")]
        public async Task<ActionResult> Delete([FromRoute] int departmentID)
        {
            await service.DeleteAsync(departmentID);
            return NoContent();
        }
    }
}
