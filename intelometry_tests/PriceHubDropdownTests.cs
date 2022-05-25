using Bunit;
using intelometry_app.Interfaces;
using intelometry_app.Models;
using intelometry_app.Shared;
using Microsoft.Extensions.DependencyInjection;
using Telerik.JustMock;

namespace intelometry_tests
{
    public class PriceHubDropdownTests
    {
        [Fact]
        public void ParametersShouldHaveCorrectType()
        {
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(new TaskCompletionSource<List<PriceHubModel>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            var cut = ctx.RenderComponent<PriceHubDropdown>(parameters => parameters
              .Add(p => p.CurrentPriceHub, "Mid C Peak")
              .Add(p => p.OnSelectPriceHub, _ => { })
            );
        }

        [Fact]
        public void SelectorIfPricehubsAreNull()
        {
            // Arrange
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(new TaskCompletionSource<List<PriceHubModel>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            // Act
            var cut = ctx.RenderComponent<PriceHubDropdown>();

            // Assert
            var initialExpectedHtml = @"<div class=""btn-group"">
                                        <button class=""btn dropdown-toggle btn-outline-primary"" 
                                        type=""button"" id=""defaultDropdown"" data-bs-toggle=""dropdown"" 
                                        data-bs-auto-close=""true"" aria-expanded=""false"">
                                        Select Price Hub</button>
                                        <ul class=""dropdown-menu"" aria-labelledby=""defaultDropdown"">
                                        <li><a class=""dropdown-item"" blazor:onclick=""1"">-</a></li></ul></div>";

            cut.MarkupMatches(initialExpectedHtml);
        }

        [Fact]
        public void SelectorIfPricehubsArePredefined()
        {
            // Arrange
            var priceHubs = new List<PriceHubModel>() { 
                new PriceHubModel { Id = 1, Title = "Palo Verde Peak" },
                new PriceHubModel { Id = 2, Title = "Mid C Peak" },
                new PriceHubModel { Id = 3, Title = "Indiana Hub RT Peak" }
            };

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(Task.FromResult(priceHubs));

            var ctx = new TestContext();
            ctx.Services.AddSingleton(serviceMock);

            // Act
            var cut = ctx.RenderComponent<PriceHubDropdown>();

            // Assert
            var expectedPriceHubDropdown = ctx.RenderComponent<PriceHubDropdown>((nameof(PriceHubDropdown.PriceHubs), priceHubs));
            cut.MarkupMatches(expectedPriceHubDropdown.Markup);
        }

        [Fact]
        public void SelectorShouldChangeCurrentPriceHub()
        {
            string actual = "";
            string expected = "Mid C Peak";

            // Arrange
            var priceHubs = new List<PriceHubModel>() {
                new PriceHubModel { Id = 1, Title = "Palo Verde Peak" },
                new PriceHubModel { Id = 2, Title = "Mid C Peak" },
                new PriceHubModel { Id = 3, Title = "Indiana Hub RT Peak" }
            };

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(Task.FromResult(priceHubs));

            var ctx = new TestContext();
            ctx.Services.AddSingleton(serviceMock);
            var cut = ctx.RenderComponent<PriceHubDropdown>(parameters => parameters
              .Add(p => p.CurrentPriceHub, null)
              .Add(p => p.OnSelectPriceHub, selectedPriceHub => actual = selectedPriceHub)
            );

            var anchorElm = cut.FindAll("a")[2];

            // Act
            anchorElm.Click(type: expected);

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPriceHub);
            Assert.Equal(expected, actual);
        }
    }
}
