using intelometry_app.Filters;

namespace intelometry_app.Services
{
    public class QueryHelperService
    {
        public string DefaultMarketDataQuery(PaginationFilter paginationFilter, DateRangeFilter dateFilter, int priceHubFilter)
        {
            string query = "select IceElectric2021_Modified.Id, PriceHubs.Title as [Price hub], " +
                           "[Trade date],[Delivery start date],[Delivery end date], " +
                           "[High price $/MWh], [Low price $/MWh], [Wtd avg price $/MWh], " +
                           "[Change], [Daily volume MWh] " +
                           "from IceElectric2021_Modified " +
                           "inner join PriceHubs " +
                           "on IceElectric2021_Modified.PriceHubId = PriceHubs.Id ";

            query = ApplyFilters(query, dateFilter, priceHubFilter) + PaginationFilterQuery(paginationFilter);

            return query;
        }

        public string DefaultPriceHubsQuery()
        {
            string query = "SELECT * FROM PriceHubs";

            return query;
        }

        public string TotalRecordsQuery(DateRangeFilter dateFilter, int priceHubFilter)
        {
            string query = "SELECT COUNT(*) FROM IceElectric2021_Modified ";

            query = ApplyFilters(query, dateFilter, priceHubFilter);

            return query;
        }

        private static string ApplyFilters(string query, DateRangeFilter dateFilter, int priceHubFilter)
        {
            if (priceHubFilter != 0 && !dateFilter.IsApplied)
            {
                query += "WHERE " + PriceHubFilterQuery(priceHubFilter);
            }

            if (priceHubFilter == 0 && dateFilter.IsApplied)
            {
                query += "WHERE " + DateFilterQuery(dateFilter);
            }

            if (priceHubFilter != 0 && dateFilter.IsApplied)
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

        private static string PriceHubFilterQuery(int priceHubFilter)
        {
            string query = $"[PriceHubId]=\'{priceHubFilter}\' ";
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
