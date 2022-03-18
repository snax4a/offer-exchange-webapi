using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Groups;

public class GroupDetailsDto : GroupDto
{
    public ICollection<TraderDto> Traders { get; set; } = default!;
}