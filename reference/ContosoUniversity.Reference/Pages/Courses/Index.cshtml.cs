using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<Course> Courses { get; set; }

    public async Task OnGetAsync()
    {
        Courses = await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .ToListAsync();
    }
}
