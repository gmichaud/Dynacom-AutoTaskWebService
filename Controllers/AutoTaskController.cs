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
    [RoutePrefix("api/{databaseName}/AutoTask")]
    [Authorize]
    public class AutoTaskController : ApiController
    {
        [Route("List")]
        [HttpGet]
        public IEnumerable<Models.AutoTask> List(string databaseName)
        {
            using (var context = new DynacomDataContext(DynacomDataContext.GetConnectionString(databaseName)))
            {
                foreach (var task in context.DynaAutoTasks.Where(t=>(t.IsSystem == false || t.IsSystem == null) && t.AvailableToAutoMan == true))
                {
                    yield return new Models.AutoTask() { TaskGuid = task.GUID, Name = task.Name };
                }
            }
        }

        [Route("Run/{taskGuid}")]
        [HttpPost]
        public HttpResponseMessage Run(string databaseName, Guid taskGuid, [FromBody]IDictionary<string, string> properties)
        {
            using (var context = new DynacomDataContext(DynacomDataContext.GetConnectionString(databaseName)))
            {
                var task = context.DynaAutoTasks.Where(t => t.GUID == taskGuid).SingleOrDefault();
                if (task == null)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.NotFound, $"Task {taskGuid} does not exist.");
                }
                else
                {
                    try
                    {
                        var errors = new List<Models.Error>();
                        var messages = new List<Models.Message>();

                        var principal = Request.GetRequestContext().Principal as DynacomPrincipal;
                        Dynacom.Helper.RunAutoTask(Properties.Settings.Default.DataSource, databaseName, principal.Username, principal.Password, task.ID, properties, errors, messages);
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
