using Microsoft.AspNetCore.Components;

namespace intelometry_app.Shared
{
    public class DateFilterBase : ComponentBase
    {
        protected List<string> DateOptions = new() { "Trade Date", "Delivery Date" };

        [Parameter]
        public bool CurrentState { get; set; }

        [Parameter]
        public string? CurrentDateFilter { get; set; }

        [Parameter]
        public DateOnly StartDate { get; set; }

        [Parameter]
        public DateOnly EndDate { get; set; }

        [Parameter]
        public EventCallback<(DateOnly, DateOnly, string, bool)> OnSelectDateRange { get; set; }

        protected void SelectDateFilter(string? selectedDateFilter)
        {
            CurrentDateFilter = selectedDateFilter;
            SetCurrentStateToFalse();
        }

        protected async Task ApplyDateFilter(ChangeEventArgs e)
        {
            if (e.Value is bool state && CurrentDateFilter is not null)
            {
                CurrentState = state;
                await OnSelectDateRange.InvokeAsync((StartDate, EndDate, CurrentDateFilter, CurrentState));
            }
        }

        protected void SetCurrentStateToFalse() => CurrentState = false;
    }
}
