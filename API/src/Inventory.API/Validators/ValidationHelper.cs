using System.Globalization;
using System.Text.RegularExpressions;

namespace Inventory.API.Validators;

public static class ValidationHelper
{
    private const int RegexTimeoutMs = 250;
    private static readonly string[] DefaultReservedWords = { "admin", "root", "system", "null", "undefined" };

    public static bool BeAValidPrice(decimal? price, int decimalPlaces = 2, decimal maxValue = 10_000_000)
    {
        if (!price.HasValue) return false;
        
        decimal value = price.Value;
        return value > 0 && 
               value <= maxValue &&
               decimal.Round(value, decimalPlaces) == value;
    }

    public static bool BeAValidQuantity(int? quantity, int min = 0, int max = 100_000)
    {
        return quantity.HasValue && 
               quantity >= min && 
               quantity <= max;
    }

    public static bool BeAValidName(string? name, int minLength = 2, int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        
        try
        {
            return Regex.IsMatch(name, 
                $@"^[\p{{L}}0-9\s'\-\.\,&:!()%@\#\$\*\+]{{{minLength},{maxLength}}}$", 
                RegexOptions.Compiled, 
                TimeSpan.FromMilliseconds(RegexTimeoutMs));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool ContainsHtmlTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        try
        {
            return Regex.IsMatch(input, 
                @"<[^>]*(>|$)", 
                RegexOptions.IgnoreCase, 
                TimeSpan.FromMilliseconds(RegexTimeoutMs));
        }
        catch (RegexMatchTimeoutException)
        {
            return true;
        }
    }

    public static bool ContainsSqlInjectionPatterns(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var patterns = new[] 
        {
            ";", "--", "/*", "*/", "xp_", 
            "char(", "nchar(", "exec ", "execute ", 
            "select ", "insert ", "update ", "delete ", 
            "drop ", "create ", "alter "
        };

        return patterns.Any(pattern => 
            input.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    public static bool ContainsReservedWords(string? input, string[]? reservedWords = null)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        
        var wordsToCheck = reservedWords ?? DefaultReservedWords;
        return wordsToCheck.Any(word => 
            input.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    public static bool ContainsEmoji(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        
        return input.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == 
                              UnicodeCategory.OtherSymbol);
    }

    public static bool MatchPattern(string input, string pattern)
    {
        if (String.IsNullOrWhiteSpace(input) || String.IsNullOrWhiteSpace(pattern)) 
            return false;
            
        try
        {
            return Regex.IsMatch(
                input,
                pattern,
                RegexOptions.None,
                TimeSpan.FromMilliseconds(RegexTimeoutMs));
        }
        catch (RegexMatchTimeoutException)
        {
            return false; // Timeout means we consider it invalid
        }
        catch (ArgumentException)
        {
            return false; // Invalid pattern means we consider it invalid
        }
    }
}