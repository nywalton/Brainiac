using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;
using Brainiac.Exception;

namespace Brainiac.Services
{
    /// <summary>
    /// Handling of the custom HttpException.
    /// </summary>
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// A constructor that accepts a RequestDelegate.
        /// </summary>
        /// <param name="next">The next RequestDelegate.</param>
        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoked when the HttpContext is being processed.
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        /// <returns>An async Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            } 
            catch (HttpException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                IHttpResponseFeature feature = context.Features.Get<IHttpResponseFeature>();
                feature.ReasonPhrase = httpException.Message;
            }
        }
    }
}
