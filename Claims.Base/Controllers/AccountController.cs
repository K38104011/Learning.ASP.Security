using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Claims.Base.Controllers
{
    public class AccountController : Controller
    {
        private static List<string> dumpData = new List<string>()
        {
            "phamdanghoanggiang@gmail.com"
        };

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleLoginCallback", "Account")
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            var loginInfo = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            if (dumpData.Contains(loginInfo.Email))
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, loginInfo.DefaultUserName),
                    new Claim(ClaimTypes.Email, loginInfo.Email),
                }, DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                    new AuthenticationProperties()
                    {
                        IsPersistent = false
                    }, identity);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View("Register", ViewBag.Email = loginInfo.Email);
        }

    }
}