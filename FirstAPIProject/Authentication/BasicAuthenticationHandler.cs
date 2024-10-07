using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace FirstAPIProject.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());
            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic", StringComparison.Ordinal))
                return Task.FromResult(AuthenticateResult.Fail("Unkown scheme"));
            var encodedCredentials = authHeader["Basic ".Length..];

            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var userNameAndPassword = decodedCredentials.Split(':');
            if (userNameAndPassword[0]!="admin" || userNameAndPassword[1]!="password")
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim (ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Name,userNameAndPassword[0])
            }, "Basic");
            var Principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(Principal,"Basic");


            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
