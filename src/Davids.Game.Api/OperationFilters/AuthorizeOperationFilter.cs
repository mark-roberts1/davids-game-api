using Davids.Game.Api.DiscordAuth;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Davids.Game.Api.OperationFilters;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes().OfType<DiscordAuthorizeAttribute>().Any()
            || (context.MethodInfo.DeclaringType != null && context.MethodInfo.DeclaringType.GetCustomAttributes().OfType<DiscordAuthorizeAttribute>().Any()))
        {
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new() { Description = "Unauthorized" });
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new() { Description = "Forbidden" });

            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        ["identify"]
                    }
                }
            ];
        }
    }
}
