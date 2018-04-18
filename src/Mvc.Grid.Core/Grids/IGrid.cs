using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace NonFactors.Mvc.Grid
{
    public interface IGrid
    {
        String Id { get; set; }
        String Name { get; set; }
        String EmptyText { get; set; }
        String SourceUrl { get; set; }
        String CssClasses { get; set; }

        ViewContext ViewContext { get; set; }
        GridFilterMode FilterMode { get;set; }
        NameValueCollection Query { get; set; }
        String FooterPartialViewName { get; set; }
        GridHtmlAttributes Attributes { get; set; }

        IGridColumns<IGridColumn> Columns { get; }
        IGridRows<Object> Rows { get; }
        IGridPager Pager { get; }
    }

    public interface IGrid<T> : IGrid
    {
        IQueryable<T> Source { get; set; }
        HashSet<IGridProcessor<T>> Processors { get; set; }

        new IGridColumnsOf<T> Columns { get; }
        new IGridRowsOf<T> Rows { get; }
        new IGridPager<T> Pager { get; set; }
    }
}
