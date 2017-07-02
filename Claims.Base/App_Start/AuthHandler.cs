using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Claims.Base
{
    public class AuthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "admin")
            };
            var id = new ClaimsIdentity(claims, "myAuthentication");
            var principal = new ClaimsPrincipal(new []
            {
                id
            });
            var config = new IdentityConfiguration();
            var newPrincipal =
                config.ClaimsAuthenticationManager.Authenticate(request.RequestUri.ToString(), principal);
            Thread.CurrentPrincipal = newPrincipal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = newPrincipal;
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}