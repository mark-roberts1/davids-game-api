using Davids.Game.Api.DiscordAuth;
using Davids.Game.Api.HostedServices;
using Davids.Game.Api.OperationFilters;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Davids.Game.Models.Leagues;
using Davids.Game.SportsApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/fpl/secrets.json", optional: true);

// Add services to the container.

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    var types = assembly.GetTypes();

    foreach (var type in types)
    {
        var attributes = type.GetCustomAttributes().Where(att => att.GetType().IsAssignableTo(typeof(ServiceAttribute))).Cast<ServiceAttribute>();

        foreach (var attribute in attributes)
        {
            builder.Services.Add(new ServiceDescriptor(attribute.Type, type, attribute.Lifetime));
        }
    }
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContextFactory<DavidsGameContext>(options => options.UseNpgsql(builder.Configuration.GetValue<string>("DavidsGameDbConnectionString")));

builder.Services.AddTransient(provider => new SportsApiConfig { Token = provider.GetRequiredService<IConfiguration>().GetValue<string>("SportsApiToken")! });
builder.Services.AddHttpClient<DiscordHttpClient>();
builder.Services.AddHttpClient<SportsApiHttpClient>(client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SportsApi:BaseAddress")!));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new()
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://discordapp.com/api/oauth2/authorize"),
                Scopes = new Dictionary<string, string>
                {
                    { "identify", "identify" },
                }
            }
        }
    });

    c.OperationFilter<AuthorizeOperationFilter>();
});

var seasonChannel = Channel.CreateUnbounded<LeagueSeasonStatsRequest>();
var leagueChannel = Channel.CreateUnbounded<LeagueSyncRequest>();

builder.Services.AddSingleton(seasonChannel);
builder.Services.AddSingleton(seasonChannel.Writer);
builder.Services.AddSingleton(seasonChannel.Reader);

builder.Services.AddSingleton(leagueChannel);
builder.Services.AddSingleton(leagueChannel.Writer);
builder.Services.AddSingleton(leagueChannel.Reader);

builder.Services.AddHostedService<LeagueDataLoader>();
builder.Services.AddHostedService<TeamStatisticsDataLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "Fantasy Prediction League API";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fantasy Prediction League API");
    c.OAuthClientId(builder.Configuration.GetValue<string>("DISCORD_CLIENT_ID"));
    c.OAuthClientSecret(builder.Configuration.GetValue<string>("DISCORD_CLIENT_SECRET"));
    c.OAuthScopes("identify");
});

app.UseHttpsRedirection();

app.UseMiddleware<DiscordAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
