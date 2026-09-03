namespace ContosoUniversity.WebAPI.ViewModels
{
    public class StudentDetailVM : StudentVM
    {
        public List<EnrollmentVM> Enrollments { get; set; }
    }
}
