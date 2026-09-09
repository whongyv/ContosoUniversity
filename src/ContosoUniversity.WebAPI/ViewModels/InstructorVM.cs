using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class InstructorVM
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
        public List<InstructorCourse> Courses { get; set; }
    }

    public class InstructorCourse
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
    }
}
