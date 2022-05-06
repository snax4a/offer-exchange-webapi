using FSH.WebApi.Application.Identity.Users;

namespace FSH.WebApi.Application.Exchange.Orders.DTOs;

public class OrderByTokenDto : IDto
{
    public OrderDetailsDto Order { get; set; } = default!;
    public UserDto Creator { get; set; } = default!;
}