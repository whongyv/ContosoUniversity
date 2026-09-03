using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class StudentCreateVM
    {
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
