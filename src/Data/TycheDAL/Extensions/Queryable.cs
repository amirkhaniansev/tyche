using System;
using System.Linq;
using Tyche.TycheDAL.Models;
using Tyche.TycheDAL.Filtration;

namespace Tyche.TycheDAL.Extensions
{
    public static class Queryable
    {
        public static IQueryable<TModel> Filter<TModel>(this IQueryable<TModel> query, Filter<TModel> filter)
            where TModel : DbModel
        {
            if (query == null)
                throw new ArgumentNullException("Query");

            if (filter == null)
                throw new ArgumentNullException("Filter");

            return filter.FilterQuery(query);
        }
    }
}