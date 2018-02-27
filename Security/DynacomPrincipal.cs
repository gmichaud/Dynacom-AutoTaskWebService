using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DynacomAutoTaskWebService.Security
{
    public class DynacomPrincipal : IPrincipal
    {
        public DynacomPrincipal(string username, string password)
        {
            Username = username;
            Password = password;
            Identity = new GenericIdentity(username);
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public IIdentity Identity { get; set; }
        public bool IsInRole(string role)
        {
            if (role.Equals("user"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}