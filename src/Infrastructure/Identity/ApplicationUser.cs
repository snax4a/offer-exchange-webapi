using Microsoft.AspNetCore.Identity;

namespace FSH.WebApi.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    // TODO: dodać imię i nazwisko i numer telefonu
    public string? CompanyName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }
}