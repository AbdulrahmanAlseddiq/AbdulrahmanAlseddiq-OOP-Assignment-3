using Collections;

// Task 2.3 — run every input from the assignment table (+ a few edge cases) and compare with the expected result.
// Format: input, method, expected.
var cases = new List<(string? Input, string Method, bool Expected)>
{
    // ---- table from the assignment ----
    ("01012345678",     "IsValidEgyptianPhone",      true),
    ("+201512345678",   "IsValidEgyptianPhone",      true),
    ("01312345678",     "IsValidEgyptianPhone",      false), // 013 is not a valid prefix
    ("0101234567",      "IsValidEgyptianPhone",      false), // only 10 digits
    ("0101234567a",     "IsValidEgyptianPhone",      false), // contains a letter
    ("29901011234567",  "IsValidEgyptianNationalId", true),
    ("19901011234567",  "IsValidEgyptianNationalId", false), // must start with 2 or 3
    ("2990101123456",   "IsValidEgyptianNationalId", false), // only 13 digits

    // ---- extra edge cases (R1, R4) ----
    ("01112345678",     "IsValidEgyptianPhone",      true),  // 011
    ("01212345678",     "IsValidEgyptianPhone",      true),  // 012
    ("+201012345678",   "IsValidEgyptianPhone",      true),  // +20 with 10
    ("+2001012345678",  "IsValidEgyptianPhone",      false), // +20 AND the leading 0
    ("010123456789",    "IsValidEgyptianPhone",      false), // 12 digits
    ("01012345678\n",   "IsValidEgyptianPhone",      false), // trailing new line
    ("30001011234567",  "IsValidEgyptianNationalId", true),  // starts with 3
    ("299010112345678", "IsValidEgyptianNationalId", false), // 15 digits
    (null,              "IsValidEgyptianPhone",      false),
    ("",                "IsValidEgyptianPhone",      false),
    ("   ",             "IsValidEgyptianPhone",      false),
    (null,              "IsValidEgyptianNationalId", false),
    ("",                "IsValidEgyptianNationalId", false),
    ("   ",             "IsValidEgyptianNationalId", false),
};

var passed = 0;
foreach (var (input, method, expected) in cases)
{
    // Called as extension methods (R5).
    var actual = method == "IsValidEgyptianPhone"
        ? input.IsValidEgyptianPhone()
        : input.IsValidEgyptianNationalId();

    var ok = actual == expected;
    if (ok)
        passed++;

    Console.WriteLine($"{(ok ? "PASS" : "FAIL")}  {Show(input),-20} {method,-26} expected={expected,-5} actual={actual}");
}

Console.WriteLine();
Console.WriteLine($"{passed}/{cases.Count} passed");
return passed == cases.Count ? 0 : 1;

static string Show(string? s) => s is null ? "null" : "\"" + s.Replace("\n", "\\n") + "\"";
