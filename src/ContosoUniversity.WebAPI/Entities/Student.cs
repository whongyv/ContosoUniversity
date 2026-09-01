namespace ContosoUniversity.WebAPI.Entities
{
    public class Student : Person
    {
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
