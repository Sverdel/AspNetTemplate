namespace AspNetTemplate.IntegrationTests.Setup
{
    public static class StringExtensions
    {
        public static string WithMaxLength(this string str, int length)
        {
            return str.Length > length
                ? str.Substring(0, length)
                : str;
        }
    }
}