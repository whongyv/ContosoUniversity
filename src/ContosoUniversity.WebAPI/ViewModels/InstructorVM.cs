using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class InstructorVMBase
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public string Office { get; set; }
    }

    public class InstructorListVM : InstructorVMBase
    {
        public List<CourseBrief> CourseBriefs { get; set; }
    }

    public class InstructorDetailVM : InstructorVMBase
    {
        public List<CourseBrief> CourseBriefs { get; set; }
    }

    public class CreateInstructorVM : InstructorVMBase
    {
        public List<int> CourseIDs { get; set; }
    }

    public class UpdateInstructorVM : InstructorVMBase
    {
        public List<int> CourseIDs { get; set; }
    }
}
