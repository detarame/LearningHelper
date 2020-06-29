//using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace LearningHelper.Filters
{
    public class ControllerExceptionFilter : Attribute, IExceptionFilter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool AllowMultiple
        {
            get { return true; }
        }
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {

            if (actionExecutedContext.Exception != null )
            {
                log.Error(actionExecutedContext.Exception.Message);
            }
            return Task.FromResult<object>(null);
        }
    }
}