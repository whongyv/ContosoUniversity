using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Departments;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;
    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public Department Department { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? departmentid)
    {
        if (departmentid is null)
        {
            return NotFound();
        }

        // LINQ Version
        //var department = await _context.Departments
        //   .Include(d => d.Administrator)
        //   .AsNoTracking()
        //   .FirstOrDefaultAsync();

        // RawSQL Version
        string query = "SELECT * FROM Departments WHERE DepartmentID = {0}";
        var department = await _context.Departments
            .FromSqlRaw(query, departmentid)
            .Include(d => d.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (department is null)
        {
            return NotFound();
        }
        else
        {
            Department = department;
        }

        return Page();
    }
}
