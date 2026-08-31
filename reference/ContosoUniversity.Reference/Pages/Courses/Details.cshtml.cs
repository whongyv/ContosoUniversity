using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;
    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public Course Course { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? courseid)
    {
        if (courseid is null)
        {
            return NotFound();
        }

        var course = await _context.Courses
        .AsNoTracking()
        .Include(c => c.Department)
        .FirstOrDefaultAsync(m => m.CourseID == courseid);

        if (course is null)
        {
            return NotFound();
        }
        else
        {
            Course = course;
        }

        return Page();
    }
}
