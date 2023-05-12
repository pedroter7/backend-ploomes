using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using PloomesBackend.Security.Authentication;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PloomesBackend.Security.Documentation
{
    public class BasicAuthenticationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requiredAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>();

            if (requiredAttributes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                var basicScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = BasicAuthenticationDefaults.SchemaName }
                };

                operation.Security = new List<OpenApiSecurityRequirement>() {
                    new() {{
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = BasicAuthenticationDefaults.SchemaName }
                            },
                            new List<string>()
                        }
                    }
                };
            }
        }
    }
}
