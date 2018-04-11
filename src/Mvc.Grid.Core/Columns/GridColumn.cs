using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NonFactors.Mvc.Grid
{
    public class GridColumn<T, TValue> : IGridColumn<T, TValue> where T : class
    {
        public IGrid<T> Grid { get; set; }

        public String Name { get; set; }
        public String Format { get; set; }
        public String CssClasses { get; set; }
        public Boolean IsEncoded { get; set; }
        public IHtmlString Title { get; set; }

        public Func<T, Object> RenderValue { get; set; }
        public GridProcessorType ProcessorType { get; set; }
        public Func<T, TValue> ExpressionValue { get; set; }
        public Expression<Func<T, TValue>> Expression { get; set; }

        IGridColumnSort IGridColumn.Sort => Sort;
        public virtual IGridColumnSort<T, TValue> Sort { get; set; }

        IGridColumnFilter IGridColumn.Filter => Filter;
        public virtual IGridColumnFilter<T, TValue> Filter { get; set; }

        public GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)
        {
            Grid = grid;
            IsEncoded = true;
            Expression = expression;
            Name = GetName(expression);
            Title = GetTitle(expression);
            ProcessorType = GridProcessorType.Pre;
            ExpressionValue = expression.Compile();
            Sort = new GridColumnSort<T, TValue>(this);
            Filter = new GridColumnFilter<T, TValue>(this);
        }

        public virtual IQueryable<T> Process(IQueryable<T> items)
        {
            return Sort.Apply(Filter.Apply(items));
        }
        public virtual IHtmlString ValueFor(IGridRow<Object> row)
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
        private String GetName(Expression<Func<T, TValue>> expression)
        {
            return String.Join("-", Regex.Split(ExpressionHelper.GetExpressionText(expression), "(?<!^)(?=[A-Z])")).ToLower();
        }
        private String GetEnumValue(Type type, String value)
        {
            return type
                .GetMember(value)
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? value;
        }
        private Object GetValueFor(IGridRow<Object> row)
        {
            try
            {
                if (RenderValue != null)
                    return RenderValue(row.Model as T);

                Type type = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
                if (type.GetTypeInfo().IsEnum)
                    return GetEnumValue(type, ExpressionValue(row.Model as T).ToString());

                return ExpressionValue(row.Model as T);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
