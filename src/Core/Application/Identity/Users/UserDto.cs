namespace FSH.WebApi.Application.Identity.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string Email { get; set; } = default!;
}