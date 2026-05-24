using System.Net;


namespace GlobalExceptions.Exceptions;
public sealed class BadRequestException(string message) : AppException(message, HttpStatusCode.BadRequest);