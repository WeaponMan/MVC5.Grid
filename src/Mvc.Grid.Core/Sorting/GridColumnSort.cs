using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnSort<T, TValue> : IGridColumnSort<T, TValue>
    {
        public Boolean? IsEnabled { get; set; }

        public virtual GridSortOrder? Order
        {
            get
            {
                if (!OrderIsSet)
                {
                    String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                    if (String.Equals(Column.Grid.Query[prefix + "sort"], Column.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        String order = Column.Grid.Query[prefix + "order"];

                        if ("asc".Equals(order, StringComparison.OrdinalIgnoreCase))
                            Order = GridSortOrder.Asc;
                        else if ("desc".Equals(order, StringComparison.OrdinalIgnoreCase))
                            Order = GridSortOrder.Desc;
                        else
                            Order = null;
                    }
                    else if (Column.Grid.Query[prefix + "sort"] == null)
                    {
                        Order = InitialOrder;
                    }
                    else
                    {
                        Order = null;
                    }
                }

                return OrderValue;
            }
            set
            {
                OrderValue = value;
                OrderIsSet = true;
            }
        }
        private Boolean OrderIsSet { get; set; }
        private GridSortOrder? OrderValue { get; set; }

        public GridSortOrder? FirstOrder { get; set; }
        public GridSortOrder? InitialOrder { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnSort(IGridColumn<T, TValue> column)
        {
            Column = column;
            IsEnabled = column.Expression.Body is MemberExpression ? (Boolean?)null : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true || Order == null)
                return items;


            if (Order == GridSortOrder.Asc)
                return items.OrderBy(NotNullOrEmptyExpression(Column.Expression)).ThenBy(Column.Expression);

            return items.OrderByDescending(NotNullOrEmptyExpression(Column.Expression)).ThenByDescending(Column.Expression);
        }

        static Expression<Func<TSource, bool>> NotNullOrEmptyExpression<TSource, TKey>(Expression<Func<TSource, TKey>> expression)
        {
            Expression<Func<TSource, bool>> notNullExpression;
            var typeTKey = typeof(TKey);
           
            if (typeTKey == typeof(string))
            {
                notNullExpression = (entity) => expression.Call()(entity) != null && !expression.Call()(entity).Equals(string.Empty);
            }
            else
            {
                notNullExpression = (entity) => expression.Call()(entity) != null;
            }
            return notNullExpression.SubstituteMarker();
        }
    }

    public static class ExpressionExtensions
    {
        public static TFunc Call<TFunc>(this Expression<TFunc> expression)
        {
            throw new InvalidOperationException("This method should never be called. It is a marker for replacing.");
        }

        public static Expression<TFunc> SubstituteMarker<TFunc>(this Expression<TFunc> expression)
        {
            var visitor = new SubstituteExpressionCallVisitor();
            return (Expression<TFunc>)visitor.Visit(expression);
        }
    }


    public class SubstituteParameterVisitor : ExpressionVisitor
    {
        private readonly LambdaExpression _expressionToVisit;
        private readonly Dictionary<ParameterExpression, Expression> _substitutionByParameter;

        public SubstituteParameterVisitor(Expression[] parameterSubstitutions, LambdaExpression expressionToVisit)
        {
            _expressionToVisit = expressionToVisit;
            _substitutionByParameter = expressionToVisit
                    .Parameters
                    .Select((parameter, index) => new { Parameter = parameter, Index = index })
                    .ToDictionary(pair => pair.Parameter, pair => parameterSubstitutions[pair.Index]);
        }

        public Expression Replace()
        {
            return Visit(_expressionToVisit.Body);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression substitution;
            if (_substitutionByParameter.TryGetValue(node, out substitution))
            {
                return Visit(substitution);
            }
            return base.VisitParameter(node);
        }
    }

    public class SubstituteExpressionCallVisitor : ExpressionVisitor
    {
        private readonly MethodInfo _markerDesctiprion;


        public SubstituteExpressionCallVisitor()
        {
            _markerDesctiprion = typeof(ExpressionExtensions)
                .GetMethod(nameof(ExpressionExtensions.Call))
                .GetGenericMethodDefinition();
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            var isMarkerCall = node.Expression.NodeType == ExpressionType.Call &&
                               IsMarker((MethodCallExpression)node.Expression);
            if (isMarkerCall)
            {
                var parameterReplacer = new SubstituteParameterVisitor(node.Arguments.ToArray(),
                    Unwrap((MethodCallExpression)node.Expression));
                var target = parameterReplacer.Replace();
                return Visit(target);
            }
            return base.VisitInvocation(node);
        }

        private LambdaExpression Unwrap(MethodCallExpression node)
        {
            var target = node.Arguments[0];
            return (LambdaExpression)Expression.Lambda(target).Compile().DynamicInvoke();
        }

        private bool IsMarker(MethodCallExpression node)
        {
            return node.Method.IsGenericMethod &&
                   node.Method.GetGenericMethodDefinition() == _markerDesctiprion;
        }
    }
}
