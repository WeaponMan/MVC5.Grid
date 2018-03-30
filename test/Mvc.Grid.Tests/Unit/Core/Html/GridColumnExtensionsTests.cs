﻿using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnExtensionsTests
    {
        private IGridColumn<GridModel, String> column;

        public GridColumnExtensionsTests()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            column = Substitute.For<IGridColumn<GridModel, String>>();
            column.Expression.Returns(expression);

            column.Filter = new GridColumnFilter<GridModel, String>(column);
        }

        #region RenderedAs<T, TValue>(this IGridColumn<T, TValue> column, Func<T, Object> value)

        [Fact]
        public void RenderedAs_SetsRenderValue()
        {
            Func<GridModel, Object> expected = (model) => model.Name;
            Func<GridModel, Object> actual = column.RenderedAs(expected).RenderValue;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RenderedAs_ReturnsColumn()
        {
            IGridColumn actual = column.RenderedAs(model => model.Name);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region MultiFilterable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isMultiple)

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MultiFilterable_SetsIsMulti(Boolean isMulti)
        {
            Boolean? actual = column.MultiFilterable(isMulti).Filter.IsMulti;
            Boolean? expected = isMulti;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, null, true)]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, null, null)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void MultiFilterable_SetsIsEnabled(Boolean isMulti, Boolean? filterEnabled, Boolean? isEnabled)
        {
            column.Filter.IsEnabled = filterEnabled;

            Boolean? actual = column.MultiFilterable(isMulti).Filter.IsEnabled;
            Boolean? expected = isEnabled;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MultiFilterable_ReturnsColumn()
        {
            IGridColumn actual = column.MultiFilterable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Filterable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isFilterable)

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Filterable_SetsIsEnabled(Boolean isEnabled)
        {
            Boolean? actual = column.Filterable(isEnabled).Filter.IsEnabled;
            Boolean? expected = isEnabled;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Filterable_ReturnsColumn()
        {
            IGridColumn actual = column.Filterable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region FilteredAs<T, TValue>(this IGridColumn<T, TValue> column, String filterName)

        [Fact]
        public void FilteredAs_SetsFilterName()
        {
            String actual = column.FilteredAs("Numeric").Filter.Name;
            String expected = "Numeric";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilteredAs_ReturnsColumn()
        {
            IGridColumn actual = column.FilteredAs("Numeric");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region InitialSort<T, TValue>(this IGridColumn<T, TValue> column, GridSortOrder order)

        [Fact]
        public void InitialSort_SetsInitialSortOrder()
        {
            GridSortOrder? actual = column.InitialSort(GridSortOrder.Desc).Sort.InitialOrder;
            GridSortOrder? expected = GridSortOrder.Desc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InitialSort_ReturnsColumn()
        {
            IGridColumn actual = column.InitialSort(GridSortOrder.Desc);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region FirstSort<T, TValue>(this IGridColumn<T, TValue> column, GridSortOrder order)

        [Fact]
        public void FirstSort_SetsFirstOrder()
        {
            GridSortOrder? actual = column.FirstSort(GridSortOrder.Desc).Sort.FirstOrder;
            GridSortOrder? expected = GridSortOrder.Desc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstSort_ReturnsColumn()
        {
            IGridColumn actual = column.FirstSort(GridSortOrder.Desc);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Sortable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isSortable)

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Sortable_SetsIsEnabled(Boolean isEnabled)
        {
            Boolean? actual = column.Sortable(isEnabled).Sort.IsEnabled;
            Boolean? expected = isEnabled;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sortable_ReturnsColumn()
        {
            IGridColumn actual = column.Sortable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Encoded<T, TValue>(this IGridColumn<T, TValue> column, Boolean isEncoded)

        [Fact]
        public void Encoded_SetsIsEncoded()
        {
            Assert.True(column.Encoded(true).IsEncoded);
        }

        [Fact]
        public void Encoded_ReturnsColumn()
        {
            IGridColumn actual = column.Encoded(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Formatted<T, TValue>(this IGridColumn<T, TValue> column, String format)

        [Fact]
        public void Formatted_SetsFormat()
        {
            String actual = column.Formatted("Format").Format;
            String expected = "Format";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Formatted_ReturnsColumn()
        {
            IGridColumn actual = column.Formatted("Format");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Css<T, TValue>(this IGridColumn<T, TValue> column, String cssClasses)

        [Fact]
        public void Css_SetsCssClasses()
        {
            String actual = column.Css("column-class").CssClasses;
            String expected = "column-class";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Css_ReturnsColumn()
        {
            IGridColumn actual = column.Css("column-class");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Titled<T, TValue>(this IGridColumn<T, TValue> column, Object value)

        [Fact]
        public void Titled_SetsHtmlContentTitle()
        {
            IHtmlString expected = new HtmlString("HtmlContent Title");
            IHtmlString actual = column.Titled(expected).Title;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Titled_SetsObjectTitle()
        {
            String actual = column.Titled(new Object()).Title.ToString();
            String expected = new Object().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Titled_SetsNullTitle()
        {
            Assert.Null(column.Titled(null).Title.ToString());
        }

        [Fact]
        public void Titled_SetsTitle()
        {
            String actual = column.Titled("Title").Title.ToString();
            String expected = "Title";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Titled_ReturnsColumn()
        {
            IGridColumn actual = column.Titled("Title");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Named<T, TValue>(this IGridColumn<T, TValue> column, String name)

        [Fact]
        public void Named_SetsName()
        {
            String actual = column.Named("Name").Name;
            String expected = "Name";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Named_ReturnsColumn()
        {
            IGridColumn actual = column.Named("Name");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion
    }
}
