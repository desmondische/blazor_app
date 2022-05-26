using intelometry_app.Filters;
using intelometry_app.Interfaces;
using intelometry_app.Models;
using intelometry_app.Wrappers;
using Microsoft.AspNetCore.Components;

namespace intelometry_app.Pages
{
    public class MarketDataBase : ComponentBase
    {
        [Inject] IMarketDataService MarketService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        protected int page;
        protected int pageSize;
        protected int totalPages;
        protected string? priceHub;
        protected string? dateFilterType;
        protected bool isFilterApplied;
        protected DateOnly startDate;
        protected DateOnly endDate;
        protected PagedResponse<List<MarketDataModel>>? response;

        [Parameter]
        [SupplyParameterFromQuery(Name = "priceHub")]
        public string? PriceHub { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "page")]
        public int? Page { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "pageSize")]
        public int? PageSize { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "startDate")]
        public DateOnly? StartDate { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "endDate")]
        public DateOnly? EndDate { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "filterType")]
        public string? DateFilterType { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "dateApplied")]
        public bool? IsDateApplied { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            // Paginator filters
            page = Page ?? 1;
            pageSize = PageSize ?? 10;

            var paginationFilter = new PaginationFilter(page, pageSize);

            // Price hub filter
            priceHub = PriceHub ?? null;

            // Trade/Delivery dates range filters
            isFilterApplied = IsDateApplied ?? false;
            dateFilterType = DateFilterType ?? null;
            startDate = StartDate ?? DateOnly.FromDateTime(DateTime.Today);
            endDate = EndDate ?? DateOnly.FromDateTime(DateTime.Today);

            var dateFilter = new DateRangeFilter(startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"), dateFilterType, isFilterApplied);

            // Main data & additional data
            response = await MarketService.GetMarketDataAsync(paginationFilter, dateFilter, priceHub);
            totalPages = (int)Math.Ceiling(response.TotalRecords / (double)pageSize);
        }

        protected void UpdatePage(int pageNumber)
        {
            Page = pageNumber;
            NavigateTo();
        }

        protected void UpdatePageSize(int pageSizeNumber)
        {
            PageSize = pageSizeNumber;
            UpdatePage(1);
        }

        protected void UpdatePriceHub(string priceHubTitle)
        {
            PriceHub = priceHubTitle;
            UpdatePage(1);
        }

        protected void UpdateDateFilter((DateOnly, DateOnly, string, bool) args)
        {
            StartDate = args.Item1;
            EndDate = args.Item2;
            DateFilterType = args.Item3;
            IsDateApplied = args.Item4;

            if (IsDateApplied == false)
                IsDateApplied = null;

            UpdatePage(1);
        }

        protected void Reset()
        {
            Page = null;
            PageSize = null;
            PriceHub = null;
            StartDate = null;
            EndDate = null;
            DateFilterType = null;
            IsDateApplied = null;
            NavigateTo();
        }

        protected void NavigateTo()
        {
            var address = NavigationManager.GetUriWithQueryParameters(
                new Dictionary<string, object?>
                {
                    ["page"] = Page,
                    ["pageSize"] = PageSize,
                    ["priceHub"] = PriceHub,
                    ["dateApplied"] = IsDateApplied,
                    ["filterType"] = DateFilterType,
                    ["startDate"] = StartDate,
                    ["endDate"] = EndDate,
                }
            );

            NavigationManager.NavigateTo(address);
        }
    }
}
