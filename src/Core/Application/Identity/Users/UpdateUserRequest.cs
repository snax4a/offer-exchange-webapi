namespace FSH.WebApi.Application.Identity.Users;

public class UpdateUserRequest
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = default!;
    public FileUploadRequest? Image { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
}