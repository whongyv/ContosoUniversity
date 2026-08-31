using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Students;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;
    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public Student Student { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
        if (student is null)
        {
            return NotFound();
        }
        else
        {
            Student = student;
        }

        return Page();
    }
}
