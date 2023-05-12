using Microsoft.OpenApi.Models;
using PloomesBackend.Security.Authentication;
using PloomesBackend.Security.Documentation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PloomesBackend.Security.Extensions
{
    public static class DocumentationExtensions
    {
        public static void AddBasicAuthenticationDocumentation(this SwaggerGenOptions genOptions)
        {
            genOptions.AddSecurityDefinition(BasicAuthenticationDefaults.SchemaName, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Basic",
                Type = SecuritySchemeType.Http,
            });

            genOptions.OperationFilter<BasicAuthenticationOperationFilter>();
        }
    }
}
