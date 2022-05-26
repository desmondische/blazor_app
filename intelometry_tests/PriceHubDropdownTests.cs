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
            int expected = 1;

            // Arrange
            using var ctx = new TestContext();

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(new TaskCompletionSource<List<PriceHubModel>>().Task);

            ctx.Services.AddSingleton(serviceMock);

            // Act
            var cut = ctx.RenderComponent<PriceHubDropdown>();

            // Assert
            var listElm = cut.FindAll("li");

            Assert.Equal(expected, listElm.Count);
        }

        [Fact]
        public void SelectorIfPricehubsArePredefined()
        {
            // Arrange
            var ctx = new TestContext();

            var priceHubs = new List<PriceHubModel>() { 
                new PriceHubModel { Id = 1, Title = "Palo Verde Peak" },
                new PriceHubModel { Id = 2, Title = "Mid C Peak" },
                new PriceHubModel { Id = 3, Title = "Indiana Hub RT Peak" }
            };

            var serviceMock = Mock.Create<IPriceHubService>();
            Mock.Arrange(() => serviceMock.GetPriceHubDataAsync())
                .Returns(Task.FromResult(priceHubs));

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
