namespace ContosoUniversity.WebAPI.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }
}
