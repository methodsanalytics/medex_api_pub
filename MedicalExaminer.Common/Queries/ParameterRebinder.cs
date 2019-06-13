using System.Collections.Generic;
using System.Linq.Expressions;

namespace MedicalExaminer.Common.Queries
{
    /// <summary>
    /// for linq to sql expression its not possible to pass in 2 predicates without
    /// reconstructing the expression and rebinding to the shared parameters, this
    /// class acts to rebind the same parameter between 2 expressions and map them to
    /// a single shared parameter so cosmos can interpret the expression
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// Initialise a new instance of <see cref="ParameterRebinder"/>.
        /// </summary>
        /// <param name="map">Map.</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replace parameters.
        /// </summary>
        /// <param name="map">Map.</param>
        /// <param name="exp">Parameter expression.</param>
        /// <returns>Expression.</returns>
        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Visit Parameter.
        /// </summary>
        /// <param name="p">Parameter expression.</param>
        /// <returns>Expression.</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (map.TryGetValue(p, out ParameterExpression replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
