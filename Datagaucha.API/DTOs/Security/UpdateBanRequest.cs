public class UpdateBanRequest
{
    public string reason { get; set; } = "";
    public DateTime bannedAt { get; set; }
    public DateTime? expiresAt { get; set; }
}