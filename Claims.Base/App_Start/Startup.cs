using System;
using Claims.Base;
using Claims.Base.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Claims.Base
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureApp(app);
        }

        private void ConfigureApp(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = App.Secrets.GoogleClientId,
                ClientSecret = App.Secrets.GoogleClientSecret
            });
        }
    }
}
