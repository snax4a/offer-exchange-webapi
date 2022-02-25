using System.Text;

namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferTokenService : IOfferTokenService
{
    // Converts two Guids concatenated with dot to base64 string
    public string GenerateToken(Guid inquiryId, Guid traderId)
    {
        string plainString = $"{inquiryId}.{traderId}";
        byte[] plainStringBytes = Encoding.UTF8.GetBytes(plainString);
        return Convert.ToBase64String(plainStringBytes);
    }

    // Checks if Guids from decoded token are not empty
    public bool ValidateToken(string token)
    {
        (Guid inquiryId, Guid traderId) = DecodeToken(token);
        return inquiryId != Guid.Empty && traderId != Guid.Empty;
    }

    // Decodes base64 string and tries to parse two Guids from it
    public (Guid InquiryId, Guid TraderId) DecodeToken(string token)
    {
        byte[] stringBytes = Convert.FromBase64String(token);
        string decodedString = Encoding.UTF8.GetString(stringBytes);
        string[] guidStrings = decodedString.Split(".");
        Guid.TryParse(guidStrings[0], out Guid inquiryId);
        Guid.TryParse(guidStrings[1], out Guid traderId);
        return (inquiryId, traderId);
    }
}