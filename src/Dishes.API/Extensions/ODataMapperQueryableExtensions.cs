using AutoMapper;
using AutoMapper.AspNet.OData;
using Dishes.Common.Models;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Web;

namespace Dishes.API.Extensions;

public static class ODataMapperQueryableExtensions
{
    /// <summary>Default page size.</summary>
    public const int DEFAULT_PAGE_SIZE = 20;
    private static readonly PropertyInfo _orderByPropertyInfo = typeof(ODataQueryOptions).GetProperty("OrderBy");

    public static async Task<PagedResponse<TOut>> GetPagedResponseAsync<TIn, TOut>(
      this IQueryable<TIn> queryable,
      IMapper mapper,
      HttpRequest request,
      ODataQueryOptions<TOut> queryOptions,
      ODataValidationSettings oDataValidationSettings = null,
      int pageSize = DEFAULT_PAGE_SIZE,
      CancellationToken cancellationToken = default
    ) where TOut : class
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        ValidateOptions(queryOptions, oDataValidationSettings);
        EnsureStableOrdering(queryOptions);

        var querySettings = new QuerySettings
        {
            ODataSettings = new ODataSettings()
            {
                HandleNullPropagation = HandleNullPropagationOption.False,
                PageSize = pageSize + 1
            },
            AsyncSettings = new AsyncSettings()
            {
                CancellationToken = cancellationToken
            }
        };

        IEnumerable<TOut> source = await queryable.GetAsync(mapper, queryOptions, querySettings).ConfigureAwait(false);

        IODataFeature odataFeature = request.HttpContext.ODataFeature();

        return new PagedResponse<TOut>(
            source.Take(pageSize),
            BuildNextPageLink(odataFeature.NextLink, source.Count(), pageSize),
            odataFeature.TotalCount
        );
    }

    private static void ValidateOptions<TIn>(
      ODataQueryOptions<TIn> queryOptions,
      ODataValidationSettings oDataValidationSettings)
    {

        _ = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));

        queryOptions.Validate(oDataValidationSettings ?? new ODataValidationSettings());
    }

    private static void EnsureStableOrdering(ODataQueryOptions queryOptions)
    {
        if (queryOptions.OrderBy == null)
        {
            _orderByPropertyInfo.SetValue(queryOptions, queryOptions.GenerateStableOrder());
        }
    }

    private static Uri BuildNextPageLink(Uri nextLink, int collectionCount, int pageSize)
    {
        if (collectionCount <= pageSize || (object)nextLink == null)
        {
            return null;
        }

        NameValueCollection queryString = HttpUtility.ParseQueryString(nextLink.Query);
        int result;
        if (int.TryParse(queryString["$skip"], out result))
        {
            queryString["$skip"] = (result - 1).ToString(CultureInfo.InvariantCulture);
        }

        return new UriBuilder(nextLink)
        {
            Query = queryString.ToString(),
            Scheme = Uri.UriSchemeHttps,
            Port = nextLink.IsDefaultPort ? -1 : nextLink.Port
        }.Uri;
    }
}