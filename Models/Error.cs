using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynacomAutoTaskWebService.Models
{
    public class Error
    {
        public int ErrorNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string Project { get; set; }
        public string Module { get; set; }
        public string Procedure { get; set; }
        public int ErrorLine { get; set; }
    }
}