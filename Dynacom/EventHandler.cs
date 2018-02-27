using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DynaFoundation;
using VBA;

namespace DynacomAutoTaskWebService.Dynacom
{
    internal class EventHandler : DynaFoundation.IEventHandler
    {
        private List<Models.Error> _errors;
        private List<Models.Message> _messages;
        
        internal EventHandler(List<Models.Error> errors, List<Models.Message> messages)
        {
            _errors = errors;
            _messages = messages;
        }
        
        public ErrorActionEnum HandleError(int ErrorNumber, string ErrorDescription, string Project, string Module, string Procedure, int ErrorLine)
        {
            _errors.Add(new Models.Error()
            {
                ErrorNumber = ErrorNumber,
                ErrorDescription = ErrorDescription,
                Project = Project,
                Module = Module,
                Procedure = Procedure,
                ErrorLine = ErrorLine
            });
            return ErrorActionEnum.eaaAbort;
        }

        public VbMsgBoxResult HandleMessage(string Prompt, VbMsgBoxStyle Buttons = VbMsgBoxStyle.vbOKOnly, string Title = null, string MessageGUID = "")
        {
            _messages.Add(new Models.Message()
            {
                MessageGuid = MessageGUID,
                Prompt = Prompt
            });
            
            //Specific messages require specific responses; the list is taken from a decompiled copy of Dynacom Automation Managed which is used in a similar unattended fashion
            switch(MessageGUID)
            {
                case "{5924BB83-E31B-41DF-9072-A66642AB23FC}":
                case "{8F5453F1-B07E-4E65-8C14-F902FADB26A4}":
                case "{66685F35-E98D-45C9-A75D-41E896550869}":
                    return VbMsgBoxResult.vbYes;
                case "{DF01B3BD-0BDD-44AF-AC87-59A35F920F0F}": 
                case "{04F5CD4D-3FA1-4809-A0D4-C5104D55785B}":
                case "{EA231636-B0D5-4292-8EB1-387E2D1E0802}":
                    return VbMsgBoxResult.vbNo;
                case "{B1DE7901-0038-442F-8CEE-18853BC0AE40}":
                    return VbMsgBoxResult.vbRetry;
                case "{D2A2FC11-C46A-42CC-AEE5-676E204F0A26}":
                case "{77419F9D-F177-4746-A547-E4BB58B93EB7}":
                case "{E87C0584-304A-4E1A-825C-A7EA42722820}":
                case "{CF02F0C4-CE7D-45CE-94BD-5BC04AD12481}":
                case "{927B8A3B-CC14-441C-AC50-BA8669ACC686}":
                case "{61E8BD2C-1EA6-43B7-AE15-11BA79C7BC21}":
                case "{27730A94-C4AB-4812-B933-518B07F2998D}":
                case "{5024659B-0D51-467B-881F-D5CA2D7B6853}":
                case "{0DAAA17C-9E05-48A3-805B-C0E25D5FB0F9}":
                    return VbMsgBoxResult.vbCancel;
                default:
                    return VbMsgBoxResult.vbOK;
            }
        }
    }
}