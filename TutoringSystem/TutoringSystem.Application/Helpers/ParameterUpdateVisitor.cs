using System.Linq.Expressions;

namespace TutoringSystem.Application.Helpers
{
    public class ParameterUpdateVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression oldParameter;
        private readonly ParameterExpression newParameter;

        public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            this.oldParameter = oldParameter;
            this.newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ReferenceEquals(node, oldParameter) ? newParameter : base.VisitParameter(node);
        }
    }
}