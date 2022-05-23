using PhoneNumbers;

namespace FSH.WebApi.Application.Common.Validation;

public static class PhoneNumberValidator
{
    public static bool IsValid(string phoneNumber)
    {
        try
        {
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, null);
            return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }
        catch (Exception)
        {
            return false;
        }
    }
}