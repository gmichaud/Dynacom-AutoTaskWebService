using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace DynacomAutoTaskWebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new Security.DynacomAuthHandler());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
