using System.Text.RegularExpressions;

namespace Collections;

/// <summary>
/// Validation helpers for Egyptian phone numbers and national IDs, called as extension methods:
/// <c>"01012345678".IsValidEgyptianPhone()</c>.
/// Each method uses exactly one regular expression; null / empty / whitespace never throws — it returns false.
/// </summary>
public static class StringValidationExtensions
{
    // ^            start of the string
    // (?:0|\+20)   the number starts with 0, or +20 written instead of that 0
    // 1[0125]      mobile prefix 10, 11, 12 or 15   →  010 / 011 / 012 / 015
    // [0-9]{8}     exactly 8 more digits             →  11 digits in total with the leading 0
    // \z           real end of the string (unlike $, it does not accept a trailing "\n")
    // [0-9] is used instead of \d because \d also matches non-ASCII digits such as "٠١٢".
    private static readonly Regex EgyptianPhonePattern =
        new(@"^(?:0|\+20)1[0125][0-9]{8}\z", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    // ^ [23] [0-9]{13} \z  →  exactly 14 digits, the first one is 2 or 3 (birth century).
    private static readonly Regex EgyptianNationalIdPattern =
        new(@"^[23][0-9]{13}\z", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IsValidEgyptianPhone(this string? value) =>
        !string.IsNullOrWhiteSpace(value) && EgyptianPhonePattern.IsMatch(value);

    public static bool IsValidEgyptianNationalId(this string? value) =>
        !string.IsNullOrWhiteSpace(value) && EgyptianNationalIdPattern.IsMatch(value);
}
