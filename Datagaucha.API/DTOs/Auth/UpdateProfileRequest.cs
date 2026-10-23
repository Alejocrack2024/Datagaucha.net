using Microsoft.AspNetCore.Http;

namespace Datagaucha.Domain.Auth;

public class UpdateProfileRequest
{
    public string name { get; set; } = string.Empty;

    public string userName { get; set; } = string.Empty;

    public DateTime? dateBirth { get; set; }

    public IFormFile? file { get; set; }
    
    public string? password { get; set; }
}