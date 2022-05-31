using intelometry_app.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace intelometry_app.Shared
{
    public class MarketDataTableBase : ComponentBase
    {
        protected Dictionary<int, string>? mapping;


        [Parameter]
        public List<MarketDataModel> MarketData { get; set; } = new ();

        [Parameter]
        public List<string> Columns { get; set; } = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            mapping = new Dictionary<int, string>()
            {
                {1, "asd1" },
                {2, "asd2" },
                {3, "asd3" },
                {4, "asd4" },
                {5, "asd5" },
                {6, "asd6" },
                {7, "asd7" },
            };
        }

        protected string PriceHubTitle(IDictionary<int, string> mapping, int externalState)
        {
            return mapping[externalState];
        }
    }
}
