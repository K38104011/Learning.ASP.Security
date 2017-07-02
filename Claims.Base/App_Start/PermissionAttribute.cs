using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Claims.Base
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string[] Permissions { get; }

        public PermissionAttribute()
        {
        }

        public PermissionAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;
            var context = new AuthorizationContext(principal as ClaimsPrincipal, "resource", "action");
            var claims = new List<Claim>();
            if (Permissions == null || !Permissions.Any())
            {
                base.OnAuthorization(actionContext);
                return;
            }
            foreach (var permission in Permissions)
            {
                var claim = new Claim("http://localhost/claims/permission", permission);
                claims.Add(claim);
            }
            claims.ToList().ForEach(claim => context.Resource.Add(claim));
            var config = new IdentityConfiguration();
            var result = config.ClaimsAuthorizationManager.CheckAccess(context);
            if (!result)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }
            base.OnAuthorization(actionContext);
        }
    }
}