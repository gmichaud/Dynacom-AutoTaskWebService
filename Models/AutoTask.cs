using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynacomAutoTaskWebService.Models
{
    public class AutoTask
    {
        public Guid TaskGuid { get; set; }
        public string Name { get; set; }
    }
}