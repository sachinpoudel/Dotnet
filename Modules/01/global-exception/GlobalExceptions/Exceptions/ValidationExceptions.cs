using System.Net;

namespace GlobalExceptions.Exceptions;

public sealed class ValidationException : AppException
{
    public IDictionary<string, string[]> Errors {get;}

    public ValidationException(IDictionary<string, string[]> errors): base("One or more validation errors occured", HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }

    public ValidationException(string field, string error): base("One or more validation error occured", HttpStatusCode.BadRequest)
    {
        Errors = new Dictionary<string, string[]>
        {
            {field, [error]}
        };
    }
}