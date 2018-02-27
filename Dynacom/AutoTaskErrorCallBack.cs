using DynaAutoTaskLib;
using System.Collections.Generic;

namespace DynacomAutoTaskWebService.Dynacom
{
    internal class AutoTaskErrorCallBack : IAutoTaskErrorCallBack
    {
        private List<Models.Error> _errors;

        internal AutoTaskErrorCallBack(List<Models.Error> errors)
        {
            _errors = errors;
        }

        public void LogError(string Message)
        {
            _errors.Add(new Models.Error() { ErrorDescription = Message });
        }
    }
}