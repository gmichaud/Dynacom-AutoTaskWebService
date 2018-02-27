using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;

namespace DynacomAutoTaskWebService.Security
{
    public class DynacomAuthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var routeData = (IHttpRouteData[]) request.GetRouteData().Values["MS_SubRoutes"];
            var databaseName = routeData[0].Values["DatabaseName"] as string;

            ValidateCredentials(databaseName, request.Headers.Authorization);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !response.Headers.Contains("WwwAuthenticate"))
            {
                response.Headers.Add("WwwAuthenticate", "Basic");
            }

            return response;
        }
    
        private bool ValidateCredentials(string databaseName, AuthenticationHeaderValue authenticationHeaderVal)
        {
            if (authenticationHeaderVal != null && !String.IsNullOrEmpty(authenticationHeaderVal.Parameter))
            {
                string[] decodedCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(authenticationHeaderVal.Parameter)).Split(new[] { ':' });
                string decodedUsername = decodedCredentials[0];
                string decodedPassword = decodedCredentials[1];
                
                using (var context = new DynacomDataContext(DynacomDataContext.GetConnectionString(databaseName)))
                {
                    var user = context.DynaUsers.Where(u => u.UserName == decodedUsername).SingleOrDefault();
                    if (user != null && user.Password.SequenceEqual(HashUserPassword(decodedUsername, decodedPassword)))
                    {
                        var principal = new DynacomPrincipal(decodedUsername, decodedPassword);
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;
                        return true;
                    }
                }
            }
            
            return false;
        }

        private byte[] HashUserPassword(string username, string password)
        {
            var encrypt = new DynaCrypt.CEncryptClass();
            var hash = new DynaCrypt.CHashClass();

            return hash.HashText(encrypt.EncryptData(password.ToUpper(), username.ToUpper()));
        }

    }
}