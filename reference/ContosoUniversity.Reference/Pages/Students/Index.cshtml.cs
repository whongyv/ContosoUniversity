using ContosoUniversity.Reference.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Reference.Pages.Students;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;
    private readonly IConfiguration Configuration;

    public IndexModel(SchoolContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }

    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string CurrentFilter { get; set; }
    public string CurrentSort { get; set; }

    public PaginatedList<Student> Students { get; set; }

    public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
    {
        CurrentSort = sortOrder;

        // Switch Version
        //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //DateSort = sortOrder == "Date" ? "date_desc" : "Date";

        // Dynamic LINQ Version
        NameSort = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
        DateSort = sortOrder == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";

        if (searchString != null)
        {
            pageIndex = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        CurrentFilter = searchString;

        IQueryable<Student> studentsIQ = from s in _context.Students
                                         select s;
        if (!String.IsNullOrEmpty(searchString))
        {
            studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                   || s.FirstMidName.Contains(searchString));
        }

        // Dynamic LINQ Version
        NameSort = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
        DateSort = sortOrder == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";

        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "LastName";
        }

        bool descending = false;
        if (sortOrder.EndsWith("_desc"))
        {
            sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
            descending = true;
        }

        if (descending)
        {
            studentsIQ = studentsIQ.OrderByDescending(e => EF.Property<object>(e, sortOrder));
        }
        else
        {
            studentsIQ = studentsIQ.OrderBy(e => EF.Property<object>(e, sortOrder));
        }

        // Switch Version
        //switch (sortOrder)
        //{
        //    case "name_desc":
        //        studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
        //        break;
        //    case "Date":
        //        studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
        //        break;
        //    case "date_desc":
        //        studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
        //        break;
        //    default:
        //        studentsIQ = studentsIQ.OrderBy(s => s.LastName);
        //        break;
        //}

        var pageSize = Configuration.GetValue("PageSize", 4);
        Students = await PaginatedList<Student>.CreateAsync(
            studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
    }
}
