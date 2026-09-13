namespace ContosoUniversity.WebAPI
{
    public static class ETagHelper
    {
        public static string Format(string value)
        {
            return $"\"{value}\"";
        }

        public static string UnFormat(string eTag)
        {
            return eTag.Trim('"');
        }
    }
}
