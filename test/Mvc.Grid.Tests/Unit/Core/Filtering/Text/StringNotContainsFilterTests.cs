using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using NonFactors.Mvc.Grid.Filtering.Text;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringNotContainsFilterTests : BaseGridFilterTests
    {
        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            Assert.Null(new StringNotContainsFilter { Value = value }.Apply(expression));
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            StringNotContainsFilter filter = new StringNotContainsFilter { Value = "Est" };
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && !model.Name.ToUpper().Contains("EST"));
            IQueryable actual = Filter(items, filter.Apply(expression.Body), expression);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
