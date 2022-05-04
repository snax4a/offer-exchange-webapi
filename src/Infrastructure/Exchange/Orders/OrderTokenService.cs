using System.Text;
using FSH.WebApi.Application.Exchange.Orders;
using Microsoft.AspNetCore.WebUtilities;

namespace FSH.WebApi.Infrastructure.Exchange.Orders;

public class OrderTokenService : IOrderTokenService
{
    // Encodes two Guids concatenated with dot to base64 string
    public string GenerateToken(Guid orderId, Guid traderId)
    {
        string plainString = $"{orderId}.{traderId}";
        byte[] plainStringBytes = Encoding.UTF8.GetBytes(plainString);
        return WebEncoders.Base64UrlEncode(plainStringBytes);
    }

    // Checks if Guids from decoded token are not empty
    public bool ValidateToken(string token)
    {
        (Guid orderId, Guid traderId) = DecodeToken(token);
        return orderId != Guid.Empty && traderId != Guid.Empty;
    }

    // Decodes base64 string and tries to parse two Guids from it
    public (Guid OrderId, Guid TraderId) DecodeToken(string token)
    {
        byte[] stringBytes = WebEncoders.Base64UrlDecode(token);
        string decodedString = Encoding.UTF8.GetString(stringBytes);
        string[] guidStrings = decodedString.Split(".");
        Guid.TryParse(guidStrings[0], out Guid orderId);
        Guid.TryParse(guidStrings[1], out Guid traderId);
        return (orderId, traderId);
    }
}