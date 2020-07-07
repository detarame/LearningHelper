using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
 using System.Web.Http.Filters;
//using System.Web.Mvc;

namespace LearningHelper.Filters
{
    public class ExtendedLoggingFilter : Attribute, IActionFilter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool AllowMultiple
        {
            get { return true; }
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            log.Info(actionContext.ActionDescriptor.ActionName);
            var result = continuation();
            return result;
        }

    }
}