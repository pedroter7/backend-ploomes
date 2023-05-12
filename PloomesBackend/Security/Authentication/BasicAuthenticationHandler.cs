using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PloomesBackend.Data.Models;
using PloomesBackend.Data.Repository;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace PloomesBackend.Security.Authentication
{
    public sealed class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public const string AuthorizationSchema = "Basic";

        private readonly IUsuarioRepository _usuarioRepository;

        public class UserDataModel
        {
            public long Id { get; set; }
        }

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IUsuarioRepository usuarioRepository)
                : base(options, logger, encoder, clock)
        {
            _usuarioRepository = usuarioRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var authHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }

            if (!AuthorizationSchema.Equals(authHeaderValue.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            if (authHeaderValue.Parameter is null)
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                var (login, password) = ExtractLoginAndPassword(authHeaderValue.Parameter);
                if (!await IsAuthenticationValid(login, password))
                {
                    return AuthenticateResult.Fail("Invalid login or password");
                }

                var userData = await _usuarioRepository.BuscarUsuario(login);
                if (userData is null)
                {
                    return AuthenticateResult.Fail("Could not retrieve user data");
                }

                var principal = CreatePrincipal(userData);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    return AuthenticateResult.NoResult();
                }

                throw;
            }
        }

        private static (string login, string password) ExtractLoginAndPassword(string base64BasicAuthString)
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64BasicAuthString));
            if (string.IsNullOrEmpty(decoded))
                throw new ArgumentException("Decoded string is empty or null");

            var spplited = decoded.Split(":");
            if (spplited.Length != 2)
                throw new FormatException(decoded);

            return (spplited[0], spplited[1]);
        }

        private Task<bool> IsAuthenticationValid(string login, string password)
        {
            return _usuarioRepository.ChecarDadosAutenticacaoUsuario(login, password);
        }

        private ClaimsPrincipal CreatePrincipal(UsuarioModel usuarioModel)
        {
            var claims = new Claim[]
                {
                    new(ClaimTypes.Email, usuarioModel.Email),
                    new(ClaimTypes.Name, usuarioModel.Nome),
                    new(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserDataModel
                    {
                        Id = usuarioModel.Id,
                    })),
                };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            return new ClaimsPrincipal(identity);
        }

    }
}
