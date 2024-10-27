using System.Text.RegularExpressions;

namespace AppointmentScheduler.Domain.Business;

public static class EntitiesExtensions
{
    private static readonly Regex UserNameRegex = new(@"^[A-Za-z0-9][A-Za-z0-9_-]{4,98}[A-Za-z0-9]$");
    private static readonly Regex PasswordRegex = new(@"^(([A-Z])|([a-z])|([0-9])|([`~!@#$%^&*()_+\-=[\]{}\\|;:'""<>,./?])){8,100}$");
    private static readonly Regex EmailRegex = new(@"^((?:[A-Za-z0-9!#$%&'*+\-\/=?^_`{|}~]|(?<=^|\.)""|""(?=$|\.|@)|(?<="".*)[ .](?=.*"")|(?<!\.)\.){1,64})(@)((?:[A-Za-z0-9.\-])*(?:[A-Za-z0-9])\.(?:[A-Za-z0-9]){2,})$");
    private static readonly Regex PhoneRegex = new(@"^0\d{9,10}");
    public static bool IsBlankOrExceed(this string value, int limit = 100) => string.IsNullOrWhiteSpace(value) || value.Length > limit;
    public static bool IsValidName(this string value) => !value.IsBlankOrExceed();
    public static bool IsValidUserName(this string value) => !value.IsBlankOrExceed() && UserNameRegex.Match(value).Success;
    public static bool IsValidPassword(this string value)
    {
        if (value == null) return false;
        var match = PasswordRegex.Match(value);
        return match.Success && match.Groups[2].Success
            && match.Groups[3].Success && match.Groups[4].Success;
    }
    private static bool IsValidByRegex(this string value, Regex regex, bool emptyAllowed)
        => !string.IsNullOrWhiteSpace(value) && ((emptyAllowed && value.Length == 0) || regex.Match(value).Success);
    public static bool IsValidEmail(this string value, bool emptyAllowed = false)
        => value.IsValidByRegex(EmailRegex, emptyAllowed);
    public static bool IsValidPhone(this string value, bool emptyAllowed = false)
        => value.IsValidByRegex(PhoneRegex, emptyAllowed);
    public static bool IsValidDescription(this string value)
        => !value.IsBlankOrExceed(250);
}