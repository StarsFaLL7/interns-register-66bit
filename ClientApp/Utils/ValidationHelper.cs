namespace ClientApp.Utils;

public static class ValidationHelper
{
    public static string ValidateStringNullOrWhitespace(string textToCheck, bool shouldCheck = false)
    {
        if (!shouldCheck)
        {
            return "";
        }
        return string.IsNullOrWhiteSpace(textToCheck) ? "is-invalid" : "is-valid";
    }
    
    public static string ValidatePhone(string phone, bool shouldCheck = false, bool validIfEmpty = false)
    {
        if (!shouldCheck)
        {
            return "";
        }
        if (validIfEmpty && string.IsNullOrWhiteSpace(phone))
        {
            return "is-valid";
        }
        if (!phone.StartsWith("+7") || phone.Length != 12)
        {
            return "is-invalid";
        }

        return !long.TryParse(phone[1..], out _) ? "is-invalid" : "is-valid";
    }
    
    public static string ValidateEmail(string email, bool shouldCheck = false)
    {
        if (!shouldCheck)
        {
            return "";
        }
        
        var trimmedEmail = email.Trim();
        if (trimmedEmail.EndsWith('.')) {
            return "is-invalid";
        }
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail ? "is-valid" : "is-invalid";
        }
        catch {
            return "is-invalid";
        }
    }
    
    public static string ValidateDateBiggerThan(DateTime dateToCheck, DateTime minDate)
    {
        return dateToCheck > minDate ? "is-valid" : "is-invalid";
    }
    
    public static string ValidateDateSmallerThan(DateTime dateToCheck, DateTime maxDate)
    {
        return dateToCheck < maxDate ? "is-valid" : "is-invalid";
    }
    
    public static string ValidateDateInBetween(DateTime dateToCheck, DateTime minDate, DateTime maxDate, bool shouldCheck = false)
    {
        if (!shouldCheck)
        {
            return "";
        }
        return dateToCheck > minDate && dateToCheck < maxDate ? "is-valid" : "is-invalid";
    }
}