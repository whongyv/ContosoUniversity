using ContosoUniversity.Reference.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ContosoUniversity.Reference.Pages
{
    public class AboutModel : PageModel
    {
        private readonly SchoolContext _context;

        public AboutModel(SchoolContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> Students { get; set; }

        public async Task OnGetAsync()
        {
            // LINQ Version
            //IQueryable<EnrollmentDateGroup> data =
            //    from student in _context.Students
            //    group student by student.EnrollmentDate into dateGroup
            //    select new EnrollmentDateGroup()
            //    {
            //        EnrollmentDate = dateGroup.Key,
            //        StudentCount = dateGroup.Count()
            //    };

            //Students = await data.AsNoTracking().ToListAsync();

            // ADO.NET Core Version
            List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                        + "FROM Person "
                        + "WHERE Discriminator = 'Student' "
                        + "GROUP BY EnrollmentDate";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            Students = groups;
        }
    }
}
