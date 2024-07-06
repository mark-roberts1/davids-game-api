using Davids.Game.Models.Users;
using System.Text.Json;

namespace Davids.Game.Api.DiscordAuth;

public class DiscordHttpClient(HttpClient client, IConfiguration configuration)
{
    private string ClientId => configuration.GetValue<string>("DISCORD_CLIENT_ID")!;
    private string ClientSecret => configuration.GetValue<string>("DISCORD_CLIENT_SECRET")!;

    public async Task<DiscordUser?> GetUserAsync(string token, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, configuration.GetValue<string>("DiscordAuth:UserInformationEndpoint"));

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request, cancellationToken);

        var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<DiscordUser>(json);
    }

    public async Task<DiscordTokenResponse> GetAccessTokenAsync(string redirectCode, string redirectUrl, CancellationToken cancellationToken)
    {
        var formContent = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", redirectCode),
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
            new KeyValuePair<string, string>("redirect_uri", redirectUrl),
        ]);

        var response = await client.PostAsync(configuration.GetValue<string>("DiscordAuth:TokenEndpoint"), formContent, cancellationToken);

        var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<DiscordTokenResponse>(json)!;
    }

    public async Task<DiscordTokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var formContent = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
        ]);

        var response = await client.PostAsync(configuration.GetValue<string>("DiscordAuth:TokenEndpoint"), formContent, cancellationToken);

        var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<DiscordTokenResponse>(json)!;
    }

    public async Task RevokeTokenAsync(string token, CancellationToken cancellationToken)
    {
        var formContent = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("token_type_hint", "access_token"),
            new KeyValuePair<string, string>("token", token),
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret),
        ]);

        var response = await client.PostAsync(configuration.GetValue<string>("DiscordAuth:RevokeEndpoint"), formContent, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
