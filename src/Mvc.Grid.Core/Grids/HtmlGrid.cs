using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NonFactors.Mvc.Grid
{
    public class HtmlGrid<T> : IHtmlGrid<T>
    {
        public IGrid<T> Grid { get; set; }
        public HtmlHelper Html { get; set; }
        public String PartialViewName { get; set; }

        public HtmlGrid(HtmlHelper html, IGrid<T> grid)
        {
            grid.Query = grid.Query ?? new NameValueCollection(html.ViewContext.HttpContext.Request.QueryString);
            grid.HttpContext = grid.HttpContext ?? html.ViewContext.HttpContext;
            PartialViewName = "MvcGrid/_Grid";
            Html = html;
            Grid = grid;
        }

        public virtual String ToHtmlString()
        {
            return Html.Partial(PartialViewName, Grid).ToHtmlString();
        }
    }
}
