using intelometry_app.Filters;
using intelometry_app.Models;
using intelometry_app.Wrappers;

namespace intelometry_app.Interfaces
{
    public interface IMarketDataService
    {
        Task<PagedResponse<List<MarketDataModel>>> GetMarketDataAsync(
            PaginationFilter paginationFilter, DateRangeFilter dateFilter, int priceHubFilter);
    }
}
