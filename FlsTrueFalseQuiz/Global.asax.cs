using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using log4net.Config;

namespace FlsTrueFalseQuiz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            Logger.Error(nameof(Application_Error), exception);
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            Server.ClearError();
        }
    }
}
