using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class DepartmentVMBase
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class DepartmentListVM : DepartmentVMBase
    {
        public int DepartmentID { get; set; }
        public string Administrator { get; set; }
    }

    public class DepartmentDetailVM : DepartmentVMBase
    {
        public int DepartmentID { get; set; }
        public int? InstructorID { get; set; }
        public string Administrator { get; set; }

        [JsonIgnore]
        public string Token { get; set; }
        public List<string> Courses { get; set; }
    }

    public class CreateDepartmentVM : DepartmentVMBase
    {
        public int? InstructorID { get; set; }
    }

    public class UpdateDepartmentVM : DepartmentVMBase
    {
        public int? InstructorID { get; set; }
    }
}
