namespace InvestigationCaseManagement.Data.Utilities
{
    public class TruncateText
    {
        public static string truncateText(string text, int maxLength = 64)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }
    }
}
