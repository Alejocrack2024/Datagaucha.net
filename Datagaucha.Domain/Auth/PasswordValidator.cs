using System.Text.RegularExpressions;

namespace Datagaucha.Domain.Auth;

public static class PasswordValidator
{
    private static readonly Regex PasswordRegex = new(
        @"\A(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s]).{12,128}\z",
        RegexOptions.None,
        TimeSpan.FromMilliseconds(250)
    );

    public static bool IsValid(string? password)
    {
        return password is not null &&
               PasswordRegex.IsMatch(password);
    }
}
