using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string ToUnsignString(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        var result = Regex.Replace(sb.ToString(), @"\s+", "").ToLower();

        return result.Normalize(NormalizationForm.FormC);
    }
}
