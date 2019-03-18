using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using Brainiac.Exception;

namespace Brainiac.Attribute
{
    /// <summary>
    /// An attribute defining that there should be some kind of Authentication in the HTTP Request Headers.
    /// </summary>
    internal class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        private const string AUTH_KEY = "Authorization";

        /// <summary>
        /// Verify there is some level of authentication in the Request Headers.
        /// todo Add some proper authentication 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authKey = context.HttpContext.Request.Headers[AUTH_KEY].SingleOrDefault();

            if (string.IsNullOrWhiteSpace(authKey)) throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}