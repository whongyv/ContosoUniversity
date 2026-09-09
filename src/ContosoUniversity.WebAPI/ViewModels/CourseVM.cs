using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class CourseVM
    {
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        [Required]
        public int DepartmentID { get; set; }
        public string Department { get; set; }
    }

    public class CourseDetailVM : CourseVM
    {
        public List<StudentGrade> StudentGrades { get; set; }
        public List<string> Instructors { get; set; }
    }
}
