using FSH.WebApi.Application.Exchange.Traders.DTOs;

namespace FSH.WebApi.Application.Exchange.Groups.DTOs;

public class GroupDetailsDto : GroupDto
{
    public ICollection<TraderDto> Traders { get; set; } = default!;
}