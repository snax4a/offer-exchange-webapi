namespace FSH.WebApi.Application.Exchange.Offers;

public interface IOfferTokenService : ITransientService
{
    string GenerateToken(Guid inquiryId, Guid traderId);
    bool ValidateToken(string token);
    (Guid InquiryId, Guid TraderId) DecodeToken(string token);
}