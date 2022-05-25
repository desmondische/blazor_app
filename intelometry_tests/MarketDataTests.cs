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
        public void InitialRenderIfMarketDataIsNull()
        {
            // Arrange
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IMarketDataService>();
            Mock.Arrange(() => serviceMock.GetMarketDataAsync(
                Arg.IsAny<PaginationFilter>(), Arg.IsAny<DateRangeFilter>(), Arg.IsAny<string>())
            )
                .Returns(new TaskCompletionSource<PagedResponse<List<MarketDataModel>>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            // Act
            var cut = ctx.RenderComponent<MarketData>();

            // Assert
            var initialExpectedHtml = @"<p><em>Loading...</em></p>";

            cut.MarkupMatches(initialExpectedHtml);
        }

        [Fact]
        public void TableViewIfMarketDataIsPredefined()
        {
            // Arrange
            var message = "OK";
            var totalRecords = 1;
            List<string> columns = new() { 
                "Id", "Price hub", "Trade date", "Delivery start date",
                "Delivery end date", "High price $/MWh", "Low price $/MWh",
                "Wtd avg price $/MWh", "Change", "Daily volume MWh"
            };
            List<MarketDataModel> marketData = new()
            {
                new MarketDataModel { Id = 1, PriceHub = "Indiana Hub RT Peak", TradeDate = new DateTime(2021, 1, 7), DeliveryStartDate = new DateTime(2021, 1, 8),
                    DeliveryEndDate = new DateTime(2021, 1, 8), HighPrice = 35, LowPrice = 32.6, AvgPrice = 33.62, Change = 1.68, Volume = 12000
                },
                //new MarketDataModel { Id = 2, PriceHub = "Indiana Hub RT Peak", TradeDate = new DateTime(2021, 1, 12), DeliveryStartDate = new DateTime(2021, 1, 13),
                //    DeliveryEndDate = new DateTime(2021, 1, 13), HighPrice = 29.5, LowPrice = 28.5, AvgPrice = 29, Change = -4.62, Volume = 1600
                //},
                //new MarketDataModel { Id = 3, PriceHub = "Indiana Hub RT Peak", TradeDate = new DateTime(2021, 1, 21), DeliveryStartDate = new DateTime(2021, 1, 22),
                //    DeliveryEndDate = new DateTime(2021, 1, 22), HighPrice = 27, LowPrice = 26.5, AvgPrice = 26.75, Change = -2.25, Volume = 1600
                //},
                //new MarketDataModel { Id = 4, PriceHub = "Indiana Hub RT Peak", TradeDate = new DateTime(2021, 1, 26), DeliveryStartDate = new DateTime(2021, 1, 27),
                //    DeliveryEndDate = new DateTime(2021, 1, 27), HighPrice = 30, LowPrice = 30, AvgPrice = 30, Change = 3.25, Volume = 5600
                //},
                //new MarketDataModel { Id = 5, PriceHub = "Indiana Hub RT Peak", TradeDate = new DateTime(2021, 2, 4), DeliveryStartDate = new DateTime(2021, 2, 5),
                //    DeliveryEndDate = new DateTime(2021, 2, 5), HighPrice = 30.5, LowPrice = 30.5, AvgPrice = 30.5, Change = 0.5, Volume = 3200
                //},
            };
            PagedResponse<List<MarketDataModel>> response = new(marketData, totalRecords, message, columns);

            var paginationFilter = new PaginationFilter(1, 1);
            var dateFilter = new DateRangeFilter(
                    DateTime.Today.ToString("yyyy-MM-dd"),
                    DateTime.Today.ToString("yyyy-MM-dd"),
                    null, false
                );

            var serviceMock = Mock.Create<IMarketDataService>();
            var serviceMock2 = Mock.Create<IPriceHubService>();

            Mock.Arrange(() => serviceMock.GetMarketDataAsync(
                paginationFilter, dateFilter, null))
                .Returns(Task.FromResult(response));

            Mock.Arrange(() => serviceMock2.GetPriceHubDataAsync())
                .Returns(new TaskCompletionSource<List<PriceHubModel>>().Task);

            using var ctx = new TestContext();
            ctx.Services.AddSingleton(serviceMock);
            ctx.Services.AddSingleton(serviceMock2);

            // Act
            var cut = ctx.RenderComponent<MarketData>();
            var actualMarketDataTable = cut.FindComponent<MarketDataTable>();

            // Assert
            var expectedDataTable = ctx.RenderComponent<MarketDataTable>((nameof(MarketDataTable.Response), response));

            actualMarketDataTable.MarkupMatches(expectedDataTable.Markup);
        }
    }
}
