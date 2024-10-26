using System.Text.RegularExpressions;

namespace AppointmentScheduler.Domain.Business;

public static class EntitiesExtensions
{
    private static readonly Regex UserNameRegex = new(@"^[A-Za-z0-9][A-Za-z0-9_-]{4,98}[A-Za-z0-9]$");
    private static readonly Regex PasswordRegex = new(@"^(([A-Z])|([a-z])|([0-9])|([`~!@#$%^&*()_+\-=[\]{}\\|;:'""<>,./?])){8,100}$");
    private static readonly Regex EmailRegex = new(@"^((?:[A-Za-z0-9!#$%&'*+\-\/=?^_`{|}~]|(?<=^|\.)""|""(?=$|\.|@)|(?<="".*)[ .](?=.*"")|(?<!\.)\.){1,64})(@)((?:[A-Za-z0-9.\-])*(?:[A-Za-z0-9])\.(?:[A-Za-z0-9]){2,})$");
    public static bool IsBlankOrExceed(this string value, int limit = 100) => string.IsNullOrWhiteSpace(value) || value.Length > limit;
    public static bool IsValidName(this string value) => !value.IsBlankOrExceed();
    public static bool IsValidUserName(this string value) => !value.IsBlankOrExceed() && UserNameRegex.Match(value).Success;
    public static bool IsValidPassword(this string value)
    {
        if (value == null) return false;
        var match = PasswordRegex.Match(value);
        if (!match.Success) return false;
        if (!match.Groups[2].Success) return false;
        if (!match.Groups[3].Success) return false;
        if (!match.Groups[4].Success) return false;
        return true;
    }

    public static bool IsValidEmail(this string value) => !value.IsBlankOrExceed() && EmailRegex.Match(value).Success;
    public static bool IsValidDescription(this string value) => !value.IsBlankOrExceed(250);
}