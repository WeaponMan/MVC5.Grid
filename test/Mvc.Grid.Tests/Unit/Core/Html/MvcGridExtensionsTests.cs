using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class MvcGridExtensionsTests
    {
        private static HtmlHelper html;

        static MvcGridExtensionsTests()
        {
            html = HtmlHelperFactory.CreateHtmlHelper("");
        }

        #region Grid<T>(this HtmlHelper html, IEnumerable<T> source)

        [Fact]
        public void Grid_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid(new GridModel[0]).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = new GridModel[0].AsQueryable();
            IEnumerable<GridModel> actual = html.Grid(expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Grid<T>(this HtmlHelper html, String partialViewName, IEnumerable<T> source)

        [Fact]
        public void Grid_PartialViewName_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid("_Partial", new GridModel[0]).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = new GridModel[0].AsQueryable();
            IEnumerable<GridModel> actual = html.Grid("_Partial", expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithPartialViewName()
        {
            String actual = html.Grid("_Partial", new GridModel[0]).PartialViewName;
            String expected = "_Partial";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AjaxGrid(this HtmlHelper, String dataSource, Object htmlAttributes = null)

        [Fact]
        public void AjaxGrid_Div()
        {
            String expected = "<div class=\"mvc-grid\" data-source-url=\"DataSource\"></div>";
            String actual = html.AjaxGrid("DataSource").ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AjaxGrid_AttributedDiv()
        {
            String actual = html.AjaxGrid("DataSource", new { @class = "classy", data_source_url = "Test", data_id = 1 }).ToString();
            String expected = "<div class=\"mvc-grid classy\" data-id=\"1\" data-source-url=\"DataSource\"></div>";

            Assert.Equal(expected, actual);
        }
        
        #endregion
    }
}
