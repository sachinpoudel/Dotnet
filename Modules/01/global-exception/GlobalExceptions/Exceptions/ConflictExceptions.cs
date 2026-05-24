using System.Net;

namespace GlobalExceptions.Exceptions;

public sealed class ConflictExceptions(string message): AppException(message, HttpStatusCode.Conflict);