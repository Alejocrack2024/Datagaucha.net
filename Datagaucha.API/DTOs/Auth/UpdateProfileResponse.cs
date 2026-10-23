namespace Datagaucha.Domain.Auth;

public class UpdateProfileResponse
{
    public long id { get; set; }
    public string name { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public DateTime? dateBirth { get; set; }
    public string? avatarUrl { get; set; }
}