using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectManager.Common.Exceptions
{
    /// <summary>
    /// Class representing custom exception that is thrown when a bad request is made.
    /// </summary>
    /// <remarks>
    /// This exception is interpreted by the exception handling middleware to return HTTP 400 status codes.
    /// </remarks>
    public class ProjectManagerBadRequestException : ProjectManagerException
    {
        /// <summary>
        /// Initializes a new instance of the ProjectManagerBadRequestException with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ProjectManagerBadRequestException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectManagerBadRequestException class.
        /// </summary>
        public ProjectManagerBadRequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectManagerBadRequestException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProjectManagerBadRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectManagerBadRequestException class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public ProjectManagerBadRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Checks the validity of the model state and throws a ProjectManagerBadRequestException if invalid.
        /// </summary>
        /// <param name="modelState">The model state to check.</param>
        /// <exception cref="ProjectManagerBadRequestException">Thrown when the model state is invalid.</exception>
        public static void ThrowIfInvalid(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return;
            }

            var exceptionMessageSb = new StringBuilder("Invalid Request. ");
            exceptionMessageSb.Append(string.Join(". ",
                modelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)));
            var exceptions = modelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.Exception)
                .Where(x => x != null)
                .ToList();
            throw exceptions.Count switch
            {
                0 => new ProjectManagerBadRequestException(exceptionMessageSb.ToString()),
                1 => new ProjectManagerBadRequestException(exceptionMessageSb.ToString(), exceptions[0]),
                _ => new ProjectManagerBadRequestException(exceptionMessageSb.ToString(),
                    new AggregateException(exceptions))
            };
        }
    }
}