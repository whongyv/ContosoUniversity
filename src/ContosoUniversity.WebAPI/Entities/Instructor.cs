namespace ContosoUniversity.WebAPI.Entities
{
    public class Instructor : Person
    {
        public DateTime HireDate { get; set; }

        public ICollection<Course> Courses { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
