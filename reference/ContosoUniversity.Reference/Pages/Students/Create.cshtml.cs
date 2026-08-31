using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Reference.Pages.Students;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Student Student { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        var emptyStudent = new Student();

        if (await TryUpdateModelAsync<Student>(
            emptyStudent,
            "student",   // Prefix for form value.
            s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
        {
            _context.Students.Add(emptyStudent);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        return Page();
    }
}
