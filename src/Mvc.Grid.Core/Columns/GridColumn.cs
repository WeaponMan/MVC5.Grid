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
        private Boolean SortOrderIsSet { get; set; }
        public override GridSortOrder? SortOrder
        {
            get
            {
                if (SortOrderIsSet)
                    return base.SortOrder;

                String prefix = String.IsNullOrEmpty(Grid.Name) ? "" : Grid.Name + "-";
                if (Grid.Query[prefix + "Sort"] == Name)
                {
                    String orderValue = Grid.Query[prefix + "Order"];
                    if (Enum.TryParse(orderValue, out GridSortOrder order))
                        SortOrder = order;
                }
                else if (Grid.Query[prefix + "Sort"] == null)
                {
                    SortOrder = InitialSortOrder;
                }

                SortOrderIsSet = true;

                return base.SortOrder;
            }
            set
            {
                base.SortOrder = value;
                SortOrderIsSet = true;
            }
        }

        private Boolean FilterIsSet { get; set; }
        public override IGridColumnFilter<T> Filter
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
            Name = ExpressionHelper.GetExpressionText(expression);
            IsSortable = expression.Body is MemberExpression ? (Boolean?)null : false;
        }

        public override IQueryable<T> Process(IQueryable<T> items)
        {
            items = Filter.Apply(items);

            if (IsSortable != true || SortOrder == null)
                return items;

            if (SortOrder == GridSortOrder.Asc)
                return items.OrderBy(Expression);

            return items.OrderByDescending(Expression);
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
