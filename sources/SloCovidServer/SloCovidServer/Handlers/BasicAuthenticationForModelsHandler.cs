using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SloCovidServer.DB.Models;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SloCovidServer.Handlers
{
    public class BasicAuthenticationForModelsHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly DataContext dataContext;
        internal readonly ref struct Credentials
        {
            public ReadOnlySpan<char> Username { get; }
            public ReadOnlySpan<char> Password { get; }
            public Credentials(ReadOnlySpan<char> username, ReadOnlySpan<char> password)
            {
                Username = username;
                Password = password;
            }
        }
        /// <summary>
        /// Initializes a new instance of <see cref="BasicAuthenticationForModelsHandler`1"/>.
        /// </summary>
        /// <param name="options">The monitor for the options instance.</param>
        /// <param name="logger">The <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/>.</param>
        /// <param name="encoder">The <see cref="System.Text.Encodings.Web.UrlEncoder"/>.</param>
        /// <param name="clock">The <see cref="Microsoft.AspNetCore.Authentication.ISystemClock"/>.</param>
        public BasicAuthenticationForModelsHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, DataContext dataContext)
            : base(options, logger, encoder, clock)
        {
            this.dataContext = dataContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            try
            {
                AuthenticationHeaderValue authorizationHeader;
                try
                {
                    authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.LogWarning(ex, "Failed verifying Authorization header");
                    return AuthenticateResult.Fail("Bad header");
                }
                catch (FormatException ex)
                {
                    Logger.LogWarning(ex, "Failed verifying Authorization header");
                    return AuthenticateResult.Fail("Bad header");
                }
                byte[] credentialBytes;
                try
                {
                    credentialBytes = Convert.FromBase64String(authorizationHeader.Parameter);
                }
                catch (FormatException ex)
                {
                    Logger.LogWarning(ex, "Failed verifying Authorization header");
                    return AuthenticateResult.Fail("Bad header");
                }
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                if (credentials.Length < 2)
                {
                    return AuthenticateResult.Fail("Wrong credentials format");
                }
                var username = credentials[0];
                if (!Guid.TryParse(username, out var id))
                {
                    return AuthenticateResult.Fail("Wrong credentials");
                }
                var password = credentials[1];
                var model = await dataContext.ModelsModels.Where(m => m.Id == id && m.Password == password && m.Active)
                                .SingleOrDefaultAsync();
                if (model is null)
                {
                    return AuthenticateResult.Fail("Wrong credentials");
                }
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, model.Name),
                };

                var identity = new ModelClaimsIdentity(claims, Scheme.Name, id);
                var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Something went wrong with basic authentication");
                return AuthenticateResult.Fail("Something went wrong with basic authentication");
            }
        }
    }
    /// <summary>
    /// Transports parsed Model.Id as Guid along claims
    /// </summary>
    public class ModelClaimsIdentity : ClaimsIdentity
    {
        public Guid ModelId { get; }
        public ModelClaimsIdentity(Claim[] claims, string schemaName, Guid modelId) : base(claims, schemaName)
        {
            ModelId = modelId;
        }
    }
}
