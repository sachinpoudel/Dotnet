using System.Net;

namespace GlobalExceptions.Exceptions;
public sealed class NotFoundException(string resourceName, object key) : AppException($"{resourceName} with identifier {key} not found", HttpStatusCode.NotFound);