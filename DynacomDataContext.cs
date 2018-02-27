using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DynacomAutoTaskWebService
{
    public partial class DynacomDataContext : DbContext
    {
        public DynacomDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public static string GetConnectionString(string database)
        {
            return "metadata=res://*/DynacomDataModel.csdl|res://*/DynacomDataModel.ssdl|res://*/DynacomDataModel.msl;" +
                "provider=System.Data.SqlClient;provider connection string=" +
                    "\"data source=" + Properties.Settings.Default.DataSource + 
                    ";initial catalog=" + database + 
                    ";integrated security=True;MultipleActiveResultSets=True;App=DynacomAutoTaskWebService\"";
        }
    }
}