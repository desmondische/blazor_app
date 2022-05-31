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
        public int CurrentPriceHub { get; set; }

        [Parameter]
        public EventCallback<int> OnSelectPriceHub { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            PriceHubs = await PriceHubService.GetPriceHubDataAsync();
        }

        protected async Task SelectedPriceHubInternal(int selectedPriceHub)
        {
            if (selectedPriceHub == 0) 
                CurrentPriceHub = 0;
            else CurrentPriceHub = selectedPriceHub;
            
            await OnSelectPriceHub.InvokeAsync(selectedPriceHub);
        }
    }
}
