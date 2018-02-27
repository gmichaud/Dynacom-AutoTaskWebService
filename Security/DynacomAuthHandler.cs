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

namespace DynacomAutoTaskWebService.Security
{
    public class DynacomAuthHandler : DelegatingHandler
    {
        string _username = "";
        string _password = "";

        private bool ValidateCredentials(AuthenticationHeaderValue authenticationHeaderVal)
        {
            if (authenticationHeaderVal != null && !String.IsNullOrEmpty(authenticationHeaderVal.Parameter))
            {
                string[] decodedCredentials = Encoding.ASCII.GetString(Convert.FromBase64String(authenticationHeaderVal.Parameter)).Split(new[] { ':' });
                string decodedUsername = decodedCredentials[0];
                string decodedPassword = decodedCredentials[1];

                using (var context = new DynacomDataContext())
                {
                    var user = context.DynaUsers.Where(u => u.UserName == decodedUsername).SingleOrDefault();
                    if (user != null && user.Password.SequenceEqual(HashUserPassword(decodedUsername, decodedPassword)))
                    { 
                        _username = decodedUsername;
                        _password = decodedPassword;
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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (ValidateCredentials(request.Headers.Authorization))
            {
                Thread.CurrentPrincipal = new DynacomPrincipal(_username, _password);
                HttpContext.Current.User = new DynacomPrincipal(_username, _password);
            }
            
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized
                && !response.Headers.Contains("WwwAuthenticate"))
            {
                response.Headers.Add("WwwAuthenticate", "Basic");
            }

            return response;
        }
    }
}