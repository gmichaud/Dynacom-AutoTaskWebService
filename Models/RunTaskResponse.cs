using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynacomAutoTaskWebService.Models
{
    public class RunTaskResponse
    {
        public List<Message> Messages;
        public List<Error> Errors;
    }
}