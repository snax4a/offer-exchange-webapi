using FSH.WebApi.Application.Exchange.Groups.DTOs;

namespace FSH.WebApi.Application.Exchange.Traders;

public class TraderDetailsDto : TraderDto
{
    public ICollection<GroupDto> Groups { get; set; } = default!;
}