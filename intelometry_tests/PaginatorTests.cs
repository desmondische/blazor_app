using Bunit;
using intelometry_app.Shared;

namespace intelometry_tests
{
    public class PaginatorTests
    {
        [Fact]
        public void ParametersShouldHaveCorrectType()
        {
            using var ctx = new TestContext();

            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPage, 1)
              .Add(p => p.CurrentPageSize, 15)
              .Add(p => p.Range, 3)
              .Add(p => p.TotalPages, 100)
              .Add(p => p.TotalRecords, 1318)
              .Add(p => p.OnSelectPage, _ => { })
              .Add(p => p.OnSelectPageSize, _ => { })
            );
        }

        [Fact]
        public void FirstPageButtonShouldSetFirstPageWhenClicked()
        {
            int expected = 1;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 50));

            var btnElm = cut.FindAll("button")[0];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void PreviousPageButtonShouldDecreaseWhenClicked()
        {
            int expected = 2;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 3));

            var btnElm = cut.FindAll("button")[1];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void NextPageButtonShouldIncrementWhenClicked()
        {
            int expected = 2;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 1));

            var btnElm = cut.FindAll("button")[5];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void LastPageButtonShouldSetLastPageWhenClicked()
        {
            int expected = 100;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 1));

            var btnElm = cut.FindAll("button")[6];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void ThirdPageButtonShouldSetPageWhenClicked()
        {
            int expected = 3;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 1));

            var btnElm = cut.FindAll("button")[4];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void EighthPageButtonShouldSetPageWhenClicked()
        {
            int expected = 8;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.TotalPages, 100)
              .Add(p => p.CurrentPage, 10));

            var btnElm = cut.FindAll("button")[2];

            // Act
            btnElm.Click();

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPage);
        }

        [Fact]
        public void PageSizeSelectorShouldChangeCurrentPageSize()
        {
            int actual = 0;
            int expected = 20;

            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPageSize, 10)
              .Add(p => p.OnSelectPageSize, selectedPageSize => actual = selectedPageSize)
            );

            var optElm = cut.Find("select");

            // Act
            optElm.Change(expected);

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentPageSize);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InfoShouldBePrintedCorrectly()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPage, 1)
              .Add(p => p.TotalPages, 10)
              .Add(p => p.TotalRecords, 100)
            );

            var smallElm = cut.Find("small");

            // Assert
            smallElm.MarkupMatches("<small>1 of 10 pages (100 items)</small>");
        }

        [Fact]
        public void InfoPageValueShouldIncrementWhenNextPageButtonClicked()
        {
            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPage, 1)
              .Add(p => p.TotalPages, 10)
              .Add(p => p.TotalRecords, 100)
            );

            var smallElm = cut.Find("small");
            var btnElm = cut.FindAll("button")[5];

            // Act
            btnElm.Click();

            // Assert
            smallElm.MarkupMatches("<small>2 of 10 pages (100 items)</small>");
        }

        [Fact]
        public void InfoPageValueShouldDecreaseWhenPreviousPageButtonClicked()
        {
            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPage, 5)
              .Add(p => p.TotalPages, 10)
              .Add(p => p.TotalRecords, 100)
            );

            var smallElm = cut.Find("small");
            var btnElm = cut.FindAll("button")[1];

            // Act
            btnElm.Click();

            // Assert
            smallElm.MarkupMatches("<small>4 of 10 pages (100 items)</small>");
        }

        [Fact]
        public void InfoTotalRecordsValueShouldChangeOnRerenderWithNewParameters()
        {
            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Paginator>(parameters => parameters
              .Add(p => p.CurrentPage, 5)
              .Add(p => p.TotalPages, 10)
              .Add(p => p.TotalRecords, 100)
            );

            var smallElm = cut.Find("small");
            smallElm.MarkupMatches("<small>5 of 10 pages (100 items)</small>");

            // Act
            cut.SetParametersAndRender(parameters => parameters
              .Add(p => p.CurrentPage, 1)
              .Add(p => p.TotalPages, 5)
              .Add(p => p.TotalRecords, 50)
            );

            // Assert
            smallElm.MarkupMatches("<small>1 of 5 pages (50 items)</small>");
        }
    }
}