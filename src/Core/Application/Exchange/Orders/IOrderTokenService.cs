namespace FSH.WebApi.Application.Exchange.Orders;

public interface IOrderTokenService : ITransientService
{
    string GenerateToken(Guid orderId, Guid traderId);
    bool ValidateToken(string token);
    (Guid OrderId, Guid TraderId) DecodeToken(string token);
}