namespace ContosoUniversity.WebAPI.ViewModels
{
    public class StudentDetailVM
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<EnrollmentVM> Enrollments { get; set; }
    }
}
