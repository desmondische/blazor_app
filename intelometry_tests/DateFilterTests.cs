using Bunit;
using intelometry_app.Shared;

namespace intelometry_tests
{
    public class DateFilterTests
    {
        [Fact]
        public void ParametersShouldHaveCorrectType()
        {
            using var ctx = new TestContext();

            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentState, false)
              .Add(p => p.StartDate, DateOnly.FromDateTime(DateTime.Today))
              .Add(p => p.EndDate, DateOnly.FromDateTime(DateTime.Today))
              .Add(p => p.CurrentDateFilter, "Trade Date")
              .Add(p => p.OnSelectDateRange, _ => { })
            );
        }

        [Fact]
        public void SelectorShouldChangeCurrentDateFilter()
        {
            string expected = "Delivery Date";

            // Arrange
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentDateFilter, "Trade Date")
            );

            var anchorElm = cut.FindAll("a")[2];

            // Act
            anchorElm.Click(type: expected);

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentDateFilter);
        }  

        [Fact]
        public void CheckboxShouldApplyStartDate()
        {
            var actual = DateOnly.FromDateTime(DateTime.Today);
            var expected = actual.AddDays(7);

            // Arrange
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentState, false)
              .Add(p => p.CurrentDateFilter, "Trade Date")
              .Add(p => p.StartDate, actual.AddDays(30))
              .Add(p => p.EndDate, actual.AddDays(60))
              .Add(p => p.OnSelectDateRange, selectedDateRange => actual = selectedDateRange.Item1)
            );

            var inputElm = cut.FindAll("input")[0];
            var cbElm = cut.FindAll("input")[2];

            // Act
            inputElm.Change(expected.ToString("yyyy-MM-dd"));
            cbElm.Change(true);

            // Assert
            Assert.Equal(expected, cut.Instance.StartDate);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckboxShouldApplyEndDate()
        {
            var actual = DateOnly.FromDateTime(DateTime.Today);
            var expected = actual.AddDays(7);

            // Arrange
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentState, false)
              .Add(p => p.CurrentDateFilter, "Trade Date")
              .Add(p => p.StartDate, actual.AddDays(30))
              .Add(p => p.EndDate, actual.AddDays(60))
              .Add(p => p.OnSelectDateRange, selectedDateRange => actual = selectedDateRange.Item2)
            );

            var inputElm = cut.FindAll("input")[1];
            var cbElm = cut.FindAll("input")[2];

            // Act
            inputElm.Change(expected.ToString("yyyy-MM-dd"));
            cbElm.Change(true);

            // Assert
            Assert.Equal(expected, cut.Instance.EndDate);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckboxShouldApplyDateFilter()
        {
            var actual = "";
            var expected = "Delivery Date";

            // Arrange
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentState, false)
              .Add(p => p.CurrentDateFilter, "-")
              .Add(p => p.StartDate, DateOnly.FromDateTime(DateTime.Today).AddDays(30))
              .Add(p => p.EndDate, DateOnly.FromDateTime(DateTime.Today).AddDays(60))
              .Add(p => p.OnSelectDateRange, selectedDateRange => actual = selectedDateRange.Item3)
            );

            var anchorElm = cut.FindAll("a")[2];
            var cbElm = cut.FindAll("input")[2];

            // Act
            anchorElm.Click(type: expected);
            cbElm.Change(true);

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentDateFilter);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckboxShouldChangeCurrentState()
        {
            bool actual = false;
            bool expected = true;

            // Arrange
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<DateFilter>(parameters => parameters
              .Add(p => p.CurrentState, false)
              .Add(p => p.CurrentDateFilter, "Trade Date")
              .Add(p => p.OnSelectDateRange, selectedDateRange => actual = selectedDateRange.Item4)
            );

            var cbElm = cut.FindAll("input")[2];

            // Act
            cbElm.Change(expected);

            // Assert
            Assert.Equal(expected, cut.Instance.CurrentState);
            Assert.Equal(expected, actual);
        }
    }
}
