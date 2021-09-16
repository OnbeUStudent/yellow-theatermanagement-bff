namespace DiiCommon
{
    public static class StringExtensions
    {
        public static string TruncateWithEllipses(this string value, int maxChars)
        {
            const string ellipses = "...";
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - ellipses.Length) + ellipses;
        }
    }
}
