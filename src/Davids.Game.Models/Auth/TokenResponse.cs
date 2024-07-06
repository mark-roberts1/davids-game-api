namespace Davids.Game.Models.Auth;

public class TokenResponse
{
    public string TokenType { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public long ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = null!;
    public string Scope { get; set; } = null!;
}
