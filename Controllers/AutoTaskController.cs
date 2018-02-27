using DynacomAutoTaskWebService.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace DynacomAutoTaskWebService.Controllers
{
    [Authorize]
    public class AutoTaskController : ApiController
    {
        [HttpGet]
        public IEnumerable<Models.AutoTask> List()
        {
            using (var context = new DynacomDataContext())
            { 
                foreach (var task in context.DynaAutoTasks.Where(t=>(t.IsSystem == false || t.IsSystem == null) && t.AvailableToAutoMan == true))
                {
                    yield return new Models.AutoTask() { TaskGuid = task.GUID, Name = task.Name };
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage Run(Guid id, [FromBody]IDictionary<string, string> properties)
        {
            using (var context = new DynacomDataContext())
            {
                var task = context.DynaAutoTasks.Where(t => t.GUID == id).SingleOrDefault();
                if (task == null)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.NotFound, $"Task {id} does not exist.");
                }
                else
                {
                    try
                    {
                        var errors = new List<Models.Error>();
                        var messages = new List<Models.Message>();

                        var principal = Request.GetRequestContext().Principal as DynacomPrincipal;
                        Dynacom.Helper.RunAutoTask(context.Database.Connection.DataSource, context.Database.Connection.Database, principal.Username, principal.Password, task.ID, properties, errors, messages);
                        return Request.CreateResponse<Models.RunTaskResponse>(errors.Count > 0 ? HttpStatusCode.InternalServerError : HttpStatusCode.OK, new Models.RunTaskResponse { Errors = errors, Messages = messages });
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }
        }
    }
}
