using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Reference.Pages.Courses;

public class CreateModel : DepartmentNamePageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        PopulateDepartmentsDropDownList(_context);
        return Page();
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        var emptyCourse = new Course();

        if (await TryUpdateModelAsync<Course>(
             emptyCourse,
             "course",   // Prefix for form value.
             s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits))
        {
            _context.Courses.Add(emptyCourse);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        // Repopulate departments dropdown. emptyCourse.DepartmentID determines the selected item.
        PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);
        return Page();
    }
}
