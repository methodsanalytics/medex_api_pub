using System;
using System.Linq;
using System.Linq.Expressions;

namespace MedicalExaminer.Common.Queries
{
    /// <summary>
    /// Utility
    /// </summary>
    /// <remarks>Helper methods for joining expressions.</remarks>
    public static class Utility
    {
        /// <summary>
        /// Compose.
        /// </summary>
        /// <typeparam name="T">Type of model being queried.</typeparam>
        /// <param name="first">Left side of the expression.</param>
        /// <param name="second">Right side of the expression.</param>
        /// <param name="merge">Type of composition.</param>
        /// <returns>Resulting expression (first) op (second)</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((firstParamExp, i) =>
            new
            {
                    firstParamExp, secondParamExp = second.Parameters[i]
            }).ToDictionary(p => p.secondParamExp, p => p.firstParamExp);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// And Operation.
        /// </summary>
        /// <remarks>If either side is null the result is just the other side effectively eliminating the expression.</remarks>
        /// <remarks>AndAlso converts to logical AND vs the binary &amp; operator.</remarks>
        /// <typeparam name="T">Type of model.</typeparam>
        /// <param name="first">Left side</param>
        /// <param name="second">Right side</param>
        /// <returns>Resulting expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null)
            {
                return second;
            }

            if (second == null)
            {
                return first;
            }

            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Or Operation.
        /// </summary>
        /// <remarks>If either side is null the result is just the other side effectively eliminating the expression.</remarks>
        /// <remarks>OrElse converts to logical OR vs the binary | operator.</remarks>
        /// <typeparam name="T">Type of model.</typeparam>
        /// <param name="first">Left side</param>
        /// <param name="second">Right side</param>
        /// <returns>Resulting expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null)
            {
                return second;
            }

            if (second == null)
            {
                return first;
            }

            return first.Compose(second, Expression.OrElse);
        }
    }
}
