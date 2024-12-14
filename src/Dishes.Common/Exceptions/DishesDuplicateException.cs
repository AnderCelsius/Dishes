using Dishes.Common.Exceptions;
using System.Net;

namespace Dishes.API.Exceptions;

[Serializable]
public sealed class DishesDuplicateException : BaseException
{
    private const string TYPE = "BadRequest";
    private const string TITLE = "Duplicate Resource";

    private const int STATUS = (int)HttpStatusCode.BadRequest;

    public DishesDuplicateException(string? detail)
        : base(TYPE, TITLE, STATUS, detail)
    {
    }

    public DishesDuplicateException(string detail, Exception innerException)
        : base(TYPE, TITLE, STATUS, detail, innerException)
    {
    }
}
