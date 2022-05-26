using intelometry_app.Interfaces;
using intelometry_app.Models;
using Microsoft.AspNetCore.Components;

namespace intelometry_app.Shared
{
    public class PriceHubDropdownBase : ComponentBase
    {
        [Inject] IPriceHubService PriceHubService { get; set; } = null!;

        protected string defaultValue = "Select Price Hub";

        [Parameter]
        public List<PriceHubModel>? PriceHubs { get; set; }

        [Parameter]
        public string? CurrentPriceHub { get; set; }

        [Parameter]
        public EventCallback<string> OnSelectPriceHub { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            PriceHubs = await PriceHubService.GetPriceHubDataAsync();
        }

        protected async Task SelectedPriceHubInternal(string? selectedPriceHub)
        {
            CurrentPriceHub = selectedPriceHub;
            await OnSelectPriceHub.InvokeAsync(selectedPriceHub);
        }
    }
}
