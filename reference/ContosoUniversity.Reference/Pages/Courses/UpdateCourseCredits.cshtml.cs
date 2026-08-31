using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Courses
{
    public class UpdateCourseCreditsModel : PageModel
    {
        private readonly SchoolContext _context;

        public UpdateCourseCreditsModel(SchoolContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(int? multiplier)
        {
            if (multiplier is null)
            {
                return Page();
            }
            var rowsAffected = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Course SET Credits = Credits * {multiplier}");
            ViewData["RowsAffected"] = rowsAffected;
            return Page();
        }
    }
}
