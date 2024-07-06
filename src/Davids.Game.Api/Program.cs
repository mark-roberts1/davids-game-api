using Davids.Game.Api.DiscordAuth;
using Davids.Game.Api.OperationFilters;
using Davids.Game.Data;
using Davids.Game.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddHttpClient<DiscordHttpClient>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
        c.OAuthClientId(builder.Configuration.GetValue<string>("DISCORD_CLIENT_ID"));
        c.OAuthClientSecret(builder.Configuration.GetValue<string>("DISCORD_CLIENT_SECRET"));
        c.OAuthScopes("identify");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<DiscordAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
