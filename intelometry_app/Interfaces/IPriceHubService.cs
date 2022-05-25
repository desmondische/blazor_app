using intelometry_app.Models;

namespace intelometry_app.Interfaces
{
    public interface IPriceHubService
    {
        Task<List<PriceHubModel>> GetPriceHubDataAsync();
    }
}
