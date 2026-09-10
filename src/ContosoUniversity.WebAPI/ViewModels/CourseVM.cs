using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.WebAPI.ViewModels
{
    public class CourseVMBase
    {
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }
    }

    public class CourseListVM : CourseVMBase
    {
        public string Department { get; set; }
    }

    public class CourseDetailVM : CourseVMBase
    {
        public string Department { get; set; }
        public List<StudentGrade> StudentGrades { get; set; }
        public List<string> Instructors { get; set; }
    }

    public class CreateCourseVM : CourseVMBase
    {
        [Required]
        public int DepartmentID { get; set; }
    }

    public class UpdateCourseVM : CourseVMBase
    {
    }
}
