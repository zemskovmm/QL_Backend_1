using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database
{
    class OrBuilder<T>
    {
        private readonly IQueryable<T> _q;
        private readonly List<Expression<Func<T, bool>>> _predicates = new List<Expression<Func<T, bool>>>();

        public OrBuilder(IQueryable<T> q)
        {
            _q = q;
        }

        public OrBuilder<T> Or(Expression<Func<T, bool>> predicate)
        {
            _predicates.Add(predicate);
            return this;
        }

        public IQueryable<T> GetWhereQueryable()
        {
            if (_predicates.Count == 0)
                return _q;
            if (_predicates.Count == 1)
                return _q.Where(_predicates[0]);

            var arg = Expression.Parameter(typeof(T), "arg");
            Expression expr = Expression.Invoke(_predicates[0], arg);
            for (var c = 1; c < _predicates.Count; c++)
                expr = Expression.Or(expr, Expression.Invoke(_predicates[c], arg));
            var lambda = Expression.Lambda<Func<T, bool>>(expr, arg);
            return _q.Where(lambda);
        }
    }

    static class OrBuilderExtensions
    {
        public static OrBuilder<T> ToOrBuilder<T>(this IQueryable<T> q) => new OrBuilder<T>(q);
    }
}
