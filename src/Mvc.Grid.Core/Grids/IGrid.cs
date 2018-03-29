using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace NonFactors.Mvc.Grid
{
    public interface IGrid
    {
        String Name { get; set; }
        String EmptyText { get; set; }
        String SourceUrl { get; set; }
        String CssClasses { get; set; }
        GridFilterMode FilterMode { get;set; }
        String FooterPartialViewName { get; set; }
        GridHtmlAttributes Attributes { get; set; }

        NameValueCollection Query { get; set; }
        HttpContextBase HttpContext { get; set; }

        IGridColumns<IGridColumn> Columns { get; }

        IGridRows<Object> Rows { get; }

        IGridPager Pager { get; }
    }

    public interface IGrid<T> : IGrid
    {
        IList<IGridProcessor<T>> Processors { get; set; }
        IQueryable<T> Source { get; set; }

        new IGridColumnsOf<T> Columns { get; }
        new IGridRowsOf<T> Rows { get; }

        new IGridPager<T> Pager { get; set; }
    }
}
