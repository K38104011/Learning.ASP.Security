using System.Linq;
using System.Security.Claims;

namespace Claims.Base
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var permissions = context.Resource.Where(x => x.Type == "http://localhost/claims/permission");
            if (permissions == null) return true;
            ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
            {
                return false;
            }
            var claims = identity.Claims.Select(claim => claim.Value);
            return permissions.All(permission => claims.Contains(permission.Value));
        }
    }
}