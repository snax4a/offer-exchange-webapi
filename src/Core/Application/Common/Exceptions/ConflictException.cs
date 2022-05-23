using System.Net;

namespace FSH.WebApi.Application.Common.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message)
        : base(message, null, HttpStatusCode.Conflict)
    {
    }

    public ConflictException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.Conflict)
    {
    }
}