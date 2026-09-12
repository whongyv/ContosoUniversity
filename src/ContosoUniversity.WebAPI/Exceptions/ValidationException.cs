namespace ContosoUniversity.WebAPI.Exceptions
{
    public class ValidationException(string message) : Exception(message)
    {
    }
}
