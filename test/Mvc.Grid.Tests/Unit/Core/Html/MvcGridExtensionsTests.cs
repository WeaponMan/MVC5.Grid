using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
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
        public void AjaxGrid_RendersPartial()
        {
            IView view = Substitute.For<IView>();
            IViewEngine engine = Substitute.For<IViewEngine>();
            ViewEngineResult result = Substitute.For<ViewEngineResult>(view, engine);
            engine.FindPartialView(Arg.Any<ControllerContext>(), "MvcGrid/_AjaxGrid", Arg.Any<Boolean>()).Returns(result);
            view.When(sub => sub.Render(Arg.Any<ViewContext>(), Arg.Any<TextWriter>())).Do(sub =>
            {
                GridHtmlAttributes attributes = sub.Arg<ViewContext>().ViewData.Model as GridHtmlAttributes;

                Assert.Equal("DataSource", attributes["data-source-url"]);
                Assert.Equal("mvc-grid", attributes["class"]);
                Assert.Equal(2, attributes.Count);

                sub.Arg<TextWriter>().Write("Rendered");
            });

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            String actual = html.AjaxGrid("DataSource").ToHtmlString();
            String expected = "Rendered";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AjaxGrid_RendersPartialWithHtmlAttributes()
        {
            IView view = Substitute.For<IView>();
            IViewEngine engine = Substitute.For<IViewEngine>();
            ViewEngineResult result = Substitute.For<ViewEngineResult>(view, engine);
            engine.FindPartialView(Arg.Any<ControllerContext>(), "MvcGrid/_AjaxGrid", Arg.Any<Boolean>()).Returns(result);
            view.When(sub => sub.Render(Arg.Any<ViewContext>(), Arg.Any<TextWriter>())).Do(sub =>
            {
                GridHtmlAttributes attributes = sub.Arg<ViewContext>().ViewData.Model as GridHtmlAttributes;

                Assert.Equal("DataSource", attributes["data-source-url"]);
                Assert.Equal("classy mvc-grid", attributes["class"]);
                Assert.Equal(1, attributes["data-id"]);
                Assert.Equal(3, attributes.Count);

                sub.Arg<TextWriter>().Write("Rendered");
            });

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            String actual = html.AjaxGrid("DataSource", new { @class = "classy", data_source_url = "Test", data_id = 1 }).ToHtmlString();
            String expected = "Rendered";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
