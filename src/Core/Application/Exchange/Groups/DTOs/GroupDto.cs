namespace FSH.WebApi.Application.Exchange.Groups.DTOs;

public class GroupDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
}