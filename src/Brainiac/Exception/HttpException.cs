using System;
using System.Net;

namespace Brainiac.Exception
{
    public class HttpException : SystemException
    {
        /// <summary>
        /// The HttpStatus Code.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// The base default constructor for HttpException errors.
        /// </summary>
        public HttpException() { }

        /// <summary>
        /// The base default constructor for HttpException errors.
        /// </summary>
        public HttpException(HttpStatusCode httpStatusCode) : this(httpStatusCode.ToString())
        {
            StatusCode = (int)httpStatusCode;
        }

        /// <summary>
        /// A constructor for HttpException errors that accepts a message.
        /// </summary>
        /// <param name="message">An additional exception message.</param>
        public HttpException(string message) : base(message) { }

        /// <summary>
        /// A constructor for Authourization errors that accepts a message and an inner message to wrap.
        /// </summary>
        /// <param name="message">An additional exception message.</param>
        /// <param name="innerException">The inner exception to wrap in an AuthorizationException.</param>
        public HttpException(string message, SystemException innerException) : base(message, innerException) { }
    }
}
