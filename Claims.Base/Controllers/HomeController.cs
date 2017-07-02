using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace Claims.Base.Controllers
{
    public class HomeController : ApiController
    {
        private readonly List<Claim> _testClaims = new List<Claim>()
        {
            new Claim("http://localhost/claims/permission", "MyApplication.ViewHomePage")
        };

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            if (!User.CheckAccess("http://localhost/claims/permission", _testClaims))
            {
                return null;
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public static class PrincipalHelper
    {
        public static bool CheckAccess(this IPrincipal principal, string permission, IList<Claim> claims)
        {
            var context = new AuthorizationContext(principal as ClaimsPrincipal, "resource", permission);
            claims.ToList().ForEach(claim => context.Resource.Add(claim));
            var config = new IdentityConfiguration();
            return config.ClaimsAuthorizationManager.CheckAccess(context);
        }
    }

}