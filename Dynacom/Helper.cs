using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace DynacomAutoTaskWebService.Dynacom
{
    internal static class Helper
    {
        internal static void RunAutoTask(string dataSource, string databaseName, string userName, string password, int taskID, IDictionary<string, string> properties, List<Models.Error> errors, List<Models.Message> messages)
        {
            DynaFoundation.CApplication app;

            var appObject = new DynaFoundation.CAppObject();
            app = appObject.Application;
            app.IsUnattended = true;
            app.set_EventHandler(new EventHandler(errors, messages));

            if (!app.InitFramework()) throw new ApplicationException("Error initializing framework.");
            if (!app.LoadConfig(Properties.Settings.Default.DynacomPath)) throw new ApplicationException("Error loading Dynacom configuration (please check DynacomPath parameter in app.config).");
            if (!app.LicensingHelper.LoadLicenses(false)) throw new ApplicationException("Error loading licenses.");

            app.Database.DBMS = 0; //deSQLServer

            if (!app.Database.OpenDatabase(dataSource, databaseName, LoginInfo: GetLoginInfo(userName, password)))
            {
                throw new ApplicationException("Error connecting to database.");
            }

            var ats = new DynaAutoTaskLib.CAutoTasks();
            if(!ats.Load(taskID))
            {
                throw new ApplicationException($"Load task failed");
            }

            var task = ats.Item[1];
            if(task is DynaILib.IContext)
            {
                ((DynaILib.IContext)task).Context = app.LogManager.CreateContext();
            }

            if(properties != null)
            {
                foreach(var p in properties)
                {
                    app.Properties.SetProperty(p.Key, p.Value);
                }
            }

            task.Execute(null, new AutoTaskErrorCallBack(errors), false);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app = null;
        }

        private static DynaFoundation.CLoginInfo GetLoginInfo(string userName, string password)
        {
            var loginInfo = new DynaFoundation.CLoginInfo();
            loginInfo.UserName = userName;
            loginInfo.Password = password;
            loginInfo.PasswordIsHashed = false;
            loginInfo.UserType = DynaILib.UserTypeEnum.utDynacom;
            return loginInfo;
        }
    }
}