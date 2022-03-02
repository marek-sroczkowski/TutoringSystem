using System;
using System.Linq;
using System.Linq.Expressions;

namespace TutoringSystem.Application.Helpers
{
    public class ExpressionMerger
    {
        public static void MergeExpression<T>(ref Expression<Func<T, bool>> expression, Expression<Func<T, bool>> newExpression) where T : class
        {
            if (expression is null)
            {
                expression = newExpression;
            }

            var visitor = new ParameterUpdateVisitor(newExpression.Parameters.First(), expression.Parameters.First());
            newExpression = visitor.Visit(newExpression) as Expression<Func<T, bool>>;
            var binExp = Expression.And(expression.Body, newExpression.Body);
            expression = Expression.Lambda<Func<T, bool>>(binExp, newExpression.Parameters);
        }
    }
}