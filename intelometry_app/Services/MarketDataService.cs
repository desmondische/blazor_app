using intelometry_app.Filters;
using intelometry_app.Interfaces;
using intelometry_app.Models;
using intelometry_app.Wrappers;
using System.Data.SqlClient;

namespace intelometry_app.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly QueryHelperService _queryHelperService;

        public MarketDataService(
            IConfiguration configuration,
            SqlConnection connection,
            QueryHelperService queryHelperService)
        {
            _configuration = configuration;
            _connection = connection;
            _queryHelperService = queryHelperService;
        }

        public async Task<PagedResponse<List<MarketDataModel>>> GetMarketDataAsync(
                        PaginationFilter paginationFilter,
                        DateRangeFilter dateFilter,
                        string? priceHubFilter)
        {
            _connection.ConnectionString =
                _configuration.GetConnectionString("MarketDataConnection");

            var query = _queryHelperService.DefaultMarketDataQuery(paginationFilter, dateFilter, priceHubFilter);
            var totalRecordsQuery = _queryHelperService.TotalRecordsQuery(dateFilter, priceHubFilter);

            var message = "OK";
            var totalRecords = 0;
            List<string> columns = new();
            List<MarketDataModel> marketData = new();

            using (_connection)
            {
                SqlCommand command = new(query, _connection);
                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    marketData.Add(new MarketDataModel
                    {
                        Id = reader.GetInt32(0),
                        PriceHub = reader.GetString(1),
                        TradeDate = reader.GetDateTime(2),
                        DeliveryStartDate = reader.GetDateTime(3),
                        DeliveryEndDate = reader.GetDateTime(4),
                        HighPrice = reader.GetDouble(5),
                        LowPrice = reader.GetDouble(6),
                        AvgPrice = reader.GetDouble(7),
                        Change = reader.GetDouble(8),
                        Volume = reader.GetInt32(9)
                    });           
                }

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                reader.Close();            

                command = new(totalRecordsQuery, _connection);
                reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    totalRecords = reader.GetInt32(0);
                }

                reader.Close();
                _connection.Close();
            }

            if (marketData.Count == 0)
            {
                message = "Requested data is not found!";
            }

            return await Task.FromResult(
                new PagedResponse<List<MarketDataModel>>(marketData, totalRecords, message, columns));
        }
    }
}
