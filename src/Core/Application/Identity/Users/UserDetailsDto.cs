namespace FSH.WebApi.Application.Identity.Users;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public string? ImageUrl { get; set; }
}