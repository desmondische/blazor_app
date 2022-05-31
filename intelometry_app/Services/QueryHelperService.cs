using intelometry_app.Filters;

namespace intelometry_app.Services
{
    public class QueryHelperService
    {
        public string DefaultMarketDataQuery(PaginationFilter paginationFilter, DateRangeFilter dateFilter, string? priceHubFilter)
        {
            string query = "SELECT * FROM IceElectric2021 ";

            query = ApplyFilters(query, dateFilter, priceHubFilter) + PaginationFilterQuery(paginationFilter);

            return query;
        }

        public string DefaultPriceHubsQuery()
        {
            string query = "SELECT * FROM PriceHubs";

            return query;
        }

        public string TotalRecordsQuery(DateRangeFilter dateFilter, string? priceHubFilter)
        {
            string query = "SELECT COUNT(*) FROM IceElectric2021 ";

            query = ApplyFilters(query, dateFilter, priceHubFilter);

            return query;
        }

        private static string ApplyFilters(string query, DateRangeFilter dateFilter, string? priceHubFilter)
        {
            if (priceHubFilter != null && !dateFilter.IsApplied)
            {
                query += "WHERE " + PriceHubFilterQuery(priceHubFilter);
            }

            if (priceHubFilter == null && dateFilter.IsApplied)
            {
                query += "WHERE " + DateFilterQuery(dateFilter);
            }

            if (priceHubFilter != null && dateFilter.IsApplied)
            {
                query += "WHERE " + PriceHubFilterQuery(priceHubFilter) + "AND " + DateFilterQuery(dateFilter);
            }

            return query;
        }

        private static string PaginationFilterQuery(PaginationFilter paginationFilter)
        {
            string query = $"ORDER BY [Id] " +
                           $"OFFSET {(paginationFilter.Page - 1) * paginationFilter.PageSize} ROWS " +
                           $"FETCH NEXT {paginationFilter.PageSize} ROWS ONLY";

            return query;
        }

        private static string PriceHubFilterQuery(string? priceHubFilter)
        {
            string query = $"[Price hub]=\'{priceHubFilter}\' ";
            return query;
        }

        private static string DateFilterQuery(DateRangeFilter dateFilter)
        {
            string query = "";
            if (dateFilter.Type == "Trade Date")
            {
                query = $"NOT ([Trade date] < '{dateFilter.Start}' " +
                        $"OR [Trade date] > '{dateFilter.End}') ";
            }
            else if (dateFilter.Type == "Delivery Date")
            {
                query = $"NOT ([Delivery start date] < '{dateFilter.Start}' " +
                        $"OR [Delivery end date] > '{dateFilter.End}') ";
            }

            return query;
        }
    }
}
