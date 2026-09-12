namespace ContosoUniversity.WebAPI.Exceptions
{
    public class DuplicateKeyException(string message) : Exception(message)
    {
    }
}
