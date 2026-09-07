using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class StudentVM
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class StudentDetailVM : StudentVM
    {
        public List<StudentEnrollment> Enrollments { get; set; }
    }

    public class StudentEnrollment
    {
        public string Course { get; set; }
        public string Grade { get; set; }
    }
}
