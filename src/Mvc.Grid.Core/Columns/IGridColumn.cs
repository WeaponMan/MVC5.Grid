using System;
using System.Linq.Expressions;
using System.Web;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumn
    {
        String Name { get; set; }
        String Format { get; set; }
        String CssClasses { get; set; }
        Boolean IsEncoded { get; set; }
        IHtmlString Title { get; set; }

        Boolean? IsSortable { get; set; }
        GridSortOrder? SortOrder { get; set; }
        GridSortOrder? FirstSortOrder { get; set; }
        GridSortOrder? InitialSortOrder { get; set; }

        String FilterName { get; set; }
        IGridColumnFilter Filter { get; }
        Boolean? IsFilterable { get; set; }
        Boolean? IsMultiFilterable { get; set; }

        IHtmlString ValueFor(IGridRow<Object> row);
    }

    public interface IGridColumn<T> : IGridProcessor<T>, IGridColumn
    {
        IGrid<T> Grid { get; }

        LambdaExpression Expression { get; }
        Func<T, Object> RenderValue { get; set; }

        new IGridColumnFilter<T> Filter { get; set; }
    }
}
