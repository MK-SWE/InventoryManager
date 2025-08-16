using System.Globalization;
using System.Text.RegularExpressions;

namespace Inventory.API.Validators;

public static class ValidationHelper
{
    private const int RegexTimeoutMs = 250;
    private static readonly string[] ReservedWords = { "admin", "root", "system", "null", "undefined" };

    public static bool BeAValidPrice(decimal? price, int decimalPlaces = 2)
    {
        if (!price.HasValue) return false;
        
        decimal value = price.Value;
        return value > 0 && 
               value <= 10_000_000 &&
               decimal.Round(value, decimalPlaces) == value;
    }

    public static bool BeAValidQuantity(int? quantity, int min = 0, int max = 100_000)
    {
        return quantity.HasValue && 
               quantity >= min && 
               quantity <= max;
    }

    public static bool BeAValidProductName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        
        try
        {
            return Regex.IsMatch(name, 
                @"^[\p{L}0-9\s'\-\.\,&:!()%@\#\$\*\+]{2,100}$", 
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

    public static bool ContainsReservedWords(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        
        return ReservedWords.Any(word => 
            input.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    public static bool ContainsEmoji(string input)
    {
        return input.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == 
                              UnicodeCategory.OtherSymbol);
    }
}