namespace Dishes.Common.Models;

public class PagedResponse<TModel>
{
    public IEnumerable<TModel> Items { get; set; }

    public string NextPageLink { get; set; }

    public long? Count { get; set; }

    public PagedResponse(IEnumerable<TModel> items, Uri nextPageLink, long? itemCount)
    {
        Items = items;
        NextPageLink = nextPageLink?.AbsoluteUri;
        Count = itemCount;
    }
}
