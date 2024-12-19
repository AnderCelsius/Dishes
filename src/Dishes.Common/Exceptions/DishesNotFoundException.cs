using Dishes.Common.Exceptions;
using System.Net;

namespace Dishes.API.Exceptions;

[Serializable]
public sealed class DishesNotFoundException : BaseException
{
    private const string TYPE = "ResourceNotFound";
    private const string TITLE = "Resource not found";

    private const int STATUS = (int)HttpStatusCode.NotFound;

    public DishesNotFoundException(string? detail)
        : base(TYPE, TITLE, STATUS, detail)
    {
    }

    public DishesNotFoundException(string detail, Exception innerException)
        : base(TYPE, TITLE, STATUS, detail, innerException)
    {
    }
}
