using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;

namespace Claims.Base
{
    public class AuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal == null || string.IsNullOrWhiteSpace(incomingPrincipal.Identity.Name))
            {
                throw new SecurityException("Name claim missing");
            }

            var roles = GetRoles(incomingPrincipal.Identity.Name).ToArray();

            if (!roles.Any())
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            var permissions = new List<string>();

            foreach (var role in roles)
            {
                var pers = GetPermission(role).ToArray();
                if (pers.Any())
                {
                    permissions.AddRange(pers);
                }
            }

            permissions = permissions.Distinct().ToList();
            var identity = (ClaimsIdentity)incomingPrincipal.Identity;
            foreach (var permission in permissions)
            {
                var claim = new Claim("http://localhost/claims/permission", permission);
                identity.AddClaim(claim);
            }

            return base.Authenticate(resourceName, incomingPrincipal);
        }

        private IEnumerable<string> GetRoles(string name)
        {
            if (name == "admin")
            {
                return new[]
                {
                    "AdminSys"
                };
            }
            return null;
        }

        private IEnumerable<string> GetPermission(string role)
        {
            if (role == "AdminSys")
            {
                return new[]
                {
                    "MyApplication.ViewHomePage",
                    "MyApplication.ViewIndex"
                };
            }
            return null;
        }
    }

}