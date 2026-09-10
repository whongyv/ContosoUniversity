using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class StudentVMBase
    {
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class StudentListVM : StudentVMBase
    {
        public int ID { get; set; }
    }

    public class StudentDetailVM : StudentVMBase
    {
        public int ID { get; set; }
        public List<CourseGrade> CourseGrades { get; set; }
    }

    public class CreateStudentVM : StudentVMBase
    {
    }

    public class UpdateStudentVM : StudentVMBase
    {
    }
}
