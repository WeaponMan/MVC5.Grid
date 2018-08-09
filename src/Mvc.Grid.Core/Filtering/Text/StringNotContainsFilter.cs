using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NonFactors.Mvc.Grid.Filtering.Text
{
    public class StringNotContainsFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (String.IsNullOrEmpty(Value))
                return null;

            Expression value = Expression.Constant(Value.ToUpper());
            MethodInfo containsMethod = typeof(String).GetMethod("Contains");
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);

            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression contains = Expression.Call(toUpper, containsMethod, value);
            Expression notContains = Expression.Not(contains);
            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null, expression.Type));

            return Expression.AndAlso(notNull, notContains);
        }
    }
}
