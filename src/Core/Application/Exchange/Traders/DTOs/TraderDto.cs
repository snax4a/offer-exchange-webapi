namespace FSH.WebApi.Application.Exchange.Traders.DTOs;

public class TraderDto : IDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? CompanyName { get; set; }
}