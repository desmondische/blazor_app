using intelometry_app.Interfaces;
using intelometry_app.Models;
using System.Data.SqlClient;

namespace intelometry_app.Services
{
    public class PriceHubService : IPriceHubService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public PriceHubService(
            IConfiguration configuration,
            SqlConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
        }

        public Task<List<PriceHubModel>> GetPriceHubDataAsync()
        {
            _connection.ConnectionString =
                _configuration.GetConnectionString("MarketDataConnection");

            var query = "SELECT * FROM PriceHubs";

            List<PriceHubModel> priceHubs = new();

            using (_connection)
            {
                SqlCommand command = new(query, _connection);
                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    priceHubs.Add(new PriceHubModel
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                    });
                }

                reader.Close();
                _connection.Close();
            }

            return Task.FromResult(priceHubs);
        }
    }
}
