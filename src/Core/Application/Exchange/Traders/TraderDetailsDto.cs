using FSH.WebApi.Application.Exchange.Groups;

namespace FSH.WebApi.Application.Exchange.Traders;

public class TraderDetailsDto : TraderDto
{
    public ICollection<GroupDto> Groups { get; set; } = default!;
}