using intelometry_app.Models;
using Microsoft.AspNetCore.Components;

namespace intelometry_app.Shared
{
    public class MarketDataTableBase : ComponentBase
    {
        [Parameter]
        public List<MarketDataModel> MarketData { get; set; } = new ();

        [Parameter]
        public List<string> Columns { get; set; } = new();
    }
}
