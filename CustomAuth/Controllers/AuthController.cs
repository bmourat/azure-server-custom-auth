using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;

namespace CustomAuth.Controllers
{
    [MobileAppController]
    public class AuthController : ApiController
    {
        // GET api/Auth
        public HttpResponseMessage Post(string username, string password)
        {
            // return error if password is not correct
            if (!this.IsPasswordValid(username, password))
            {
                return this.Request.CreateUnauthorizedResponse();
            }

            JwtSecurityToken token = this.GetAuthenticationTokenForUser(username);

            return this.Request.CreateResponse(HttpStatusCode.OK, new
            {
                Token = token.RawData,
                Username = username
            });
        }

        private bool IsPasswordValid(string username, string password)
        {
            if (password == "123")
            {
                return true;
            }

            return false;
        }

        private JwtSecurityToken GetAuthenticationTokenForUser(string username)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var signingKey = "dmVyeSBsb25nIGxvbmcgbG9uZyBrZXkgdmVyeSBsb25nIGxvbmcgbG9uZyBrZXkgdmVyeSBsb25nIGxvbmcgbG9uZyBrZXkgdmVyeSBsb25nIGxvbmcgbG9uZyBrZXkgdmVyeSBsb25nIGxvbmcgbG9uZyBrZXk=";
            //Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            //
            //
            var audience = GetSiteUrl(); // audience must match the url of the site
            var issuer = GetSiteUrl(); // audience must match the url of the site

        JwtSecurityToken token = AppServiceLoginHandler.CreateToken(
            claims,
            signingKey,
            audience,
            issuer,
            TimeSpan.FromHours(24)
            );

            return token;
        }

        private string GetSiteUrl()
        {
            var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                return ConfigurationManager.AppSettings["ValidAudience"];
            }
            else
            {
                return "https://" + settings.HostName + "/";
            }
        }

        private string GetSigningKey()
        {
            var settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // this key is for debuggint and testing purposes only
                // this key should match the one supplied in Startup.MobileApp.cs
                return ConfigurationManager.AppSettings["SigningKey"];
            }
            else
            {
                return Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            }
        }
    }
}
