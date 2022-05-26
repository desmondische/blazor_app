using Bunit;
using intelometry_app.Filters;
using intelometry_app.Interfaces;
using intelometry_app.Models;
using intelometry_app.Pages;
using intelometry_app.Shared;
using intelometry_app.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Telerik.JustMock;

namespace intelometry_tests
{
    public class MarketDataTests
    {
        [Fact]
        public void ParametersShouldHaveCorrectType()
        {
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IMarketDataService>();

            Mock.Arrange(() => serviceMock.GetMarketDataAsync(
                Arg.IsAny<PaginationFilter>(), Arg.IsAny<DateRangeFilter>(), Arg.IsAny<string>())
            )
                .Returns(new TaskCompletionSource<PagedResponse<List<MarketDataModel>>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            var cut = ctx.RenderComponent<MarketData>(parameters => parameters
              .Add(p => p.PriceHub, "Mid C Peak")
              .Add(p => p.StartDate, DateOnly.FromDateTime(DateTime.Today))
              .Add(p => p.EndDate, DateOnly.FromDateTime(DateTime.Today))
              .Add(p => p.DateFilterType, "Trade Date")
              .Add(p => p.Page, 1)
              .Add(p => p.PageSize, 10)
              .Add(p => p.IsDateApplied, true)
            );
        }

        [Fact]
        public void InitialRenderWhenMarketDataIsNull()
        {
            // Arrange
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IMarketDataService>();

            Mock.Arrange(() => serviceMock.GetMarketDataAsync(
                Arg.IsAny<PaginationFilter>(), Arg.IsAny<DateRangeFilter>(), Arg.IsAny<string>()))
                .Returns(new TaskCompletionSource<PagedResponse<List<MarketDataModel>>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            // Act
            var cut = ctx.RenderComponent<MarketData>();

            // Assert
            var initialExpectedHtml = @"<p><em>Loading...</em></p>";

            cut.MarkupMatches(initialExpectedHtml);
        }

        [Fact]
        public void InitialRenderWhenMarketDataIsPredefined()
        {
            // Arrange
            var ctx = new TestContext();

            var message = "OK";
            var totalRecords = 1;

            List<string> columns = new() {
                "Id", "Price hub", "Trade date", "Delivery start date",
                "Delivery end date", "High price $/MWh", "Low price $/MWh",
                "Wtd avg price $/MWh", "Change", "Daily volume MWh"
            };

            List<MarketDataModel> marketData = new()
            {
                new MarketDataModel { 
                    Id = 1, 
                    PriceHub = "Indiana Hub RT Peak", 
                    TradeDate = new DateTime(2021, 1, 7), 
                    DeliveryStartDate = new DateTime(2021, 1, 8),
                    DeliveryEndDate = new DateTime(2021, 1, 8), 
                    HighPrice = 35, 
                    LowPrice = 32.6, 
                    AvgPrice = 33.62, 
                    Change = 1.68, 
                    Volume = 12000
                }
            };

            PagedResponse<List<MarketDataModel>> response = new(marketData, totalRecords, message, columns);

            var serviceMock = Mock.Create<IMarketDataService>();
            Mock.Arrange(() => serviceMock.GetMarketDataAsync(
                Arg.IsAny<PaginationFilter>(), Arg.IsAny<DateRangeFilter>(), Arg.IsAny<string>()))
                .Returns(Task.FromResult(response));

            var serviceMock2 = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock2.GetPriceHubDataAsync())
                .Returns(new TaskCompletionSource<List<PriceHubModel>>().Task);

            ctx.Services.AddSingleton(serviceMock);
            ctx.Services.AddSingleton(serviceMock2);

            // Act
            var cut = ctx.RenderComponent<MarketData>();
            var actualDataTable = cut.FindComponent<MarketDataTable>();

            // Assert
            var expectedDataTable = ctx.RenderComponent<MarketDataTable>(
                (nameof(MarketDataTable.MarketData), response.Data),
                (nameof(MarketDataTable.Columns), response.Columns)
            );

            actualDataTable.MarkupMatches(expectedDataTable.Markup);
        }
    }
}
