using FSH.WebApi.Application.Exchange.Groups;

namespace FSH.WebApi.Application.Exchange.Traders;

public class TraderDto : IDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public ICollection<GroupDto> Groups { get; set; } = default!;
}