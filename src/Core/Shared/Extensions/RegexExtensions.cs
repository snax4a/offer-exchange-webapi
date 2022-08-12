using System.Net;
using System.Text.RegularExpressions;

namespace FSH.WebApi.Core.Shared.Extensions;

public static class RegexExtensions
{
    private static readonly Regex Whitespace = new(@"\s+");
    private static readonly Regex HtmlTags = new("<[^>]*(>|$)");

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return Whitespace.Replace(input, replacement);
    }

    public static string StripHtml(this string input)
    {
        return HtmlTags.Replace(WebUtility.HtmlDecode(input), string.Empty);
    }
}