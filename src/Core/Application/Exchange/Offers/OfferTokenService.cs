namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferTokenService : IOfferTokenService
{
    public string GenerateToken(Guid inquiryId, Guid traderId)
    {
        throw new NotImplementedException();
    }

    public bool ValidateToken(string token)
    {
        return token == "123";
    }

    public (Guid InquiryId, Guid TraderId) DecodeToken(string token)
    {
        Guid inquiryId = Guid.Parse("91ac0000-fe87-aae9-92ce-08d9f52ab8ed");
        Guid traderId = Guid.Parse("82125015-d1e5-46b1-a982-cb329d4666e0");
        return (inquiryId, traderId);
    }
}