using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using PloomesBackend.Security.Authentication;
using System.Security.Claims;
using static PloomesBackend.Security.Authentication.BasicAuthenticationHandler;

namespace PloomesBackend.Security.Extensions
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder,
            Action<BasicAuthenticationOptions>? configureOptions = null)
        {
            builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.SchemaName, configureOptions);
            return builder;
        }

        public static long? GetUsuarioId(this ClaimsPrincipal principal)
        {
            var dataClaim = principal.FindFirst(ClaimTypes.UserData);
            if (dataClaim is null)
                return null;

            var decoded = JsonConvert.DeserializeObject<UserDataModel>(dataClaim.Value);
            if (decoded is null)
                return null;

            return decoded.Id;
        }
    }
}
