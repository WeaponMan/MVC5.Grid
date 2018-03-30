using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace NonFactors.Mvc.Grid
{
    public class GridColumn<T, TValue> : BaseGridColumn<T, TValue> where T : class
    {
        private Boolean FilterIsSet { get; set; }
        public override IGridColumnFilter<T, TValue> Filter
        {
            get
            {
                if (!FilterIsSet)
                    Filter = MvcGrid.Filters.GetFilter(this);

                return base.Filter;
            }
            set
            {
                base.Filter = value;
                FilterIsSet = true;
            }
        }

        public GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)
        {
            Grid = grid;
            IsEncoded = true;
            Expression = expression;
            Title = GetTitle(expression);
            ProcessorType = GridProcessorType.Pre;
            ExpressionValue = expression.Compile();
            Sort = new GridColumnSort<T, TValue>(this);
            Name = ExpressionHelper.GetExpressionText(expression);
        }

        public override IQueryable<T> Process(IQueryable<T> items)
        {
            return Sort.Apply(Filter.Apply(items));
        }
        public override IHtmlString ValueFor(IGridRow<Object> row)
        {
            Object value = GetValueFor(row);

            if (value == null)
                return MvcHtmlString.Empty;

            if (value is IHtmlString content)
                return content;

            if (Format != null)
                value = String.Format(Format, value);

            if (IsEncoded)
                return new HtmlString(WebUtility.HtmlEncode(value.ToString()));

            return new HtmlString(value.ToString());
        }

        private IHtmlString GetTitle(Expression<Func<T, TValue>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;
            DisplayAttribute display = body?.Member.GetCustomAttribute<DisplayAttribute>();

            return new HtmlString(display?.GetShortName());
        }
        private Object GetValueFor(IGridRow<Object> row)
        {
            try
            {
                if (RenderValue != null)
                    return RenderValue(row.Model as T);

                return ExpressionValue(row.Model as T);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
