using System.Net;

namespace FSH.WebApi.Application.Common.Exceptions;

public class PaymentRequiredException : CustomException
{
    public PaymentRequiredException(string message)
        : base(message, null, HttpStatusCode.PaymentRequired)
    {
    }

    public PaymentRequiredException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.PaymentRequired)
    {
    }
}