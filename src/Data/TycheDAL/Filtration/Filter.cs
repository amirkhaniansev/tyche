using System;
using System.Linq;
using System.Collections.Generic;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheDAL.Filtration
{
    public partial class Filter<TModel> : FilterBase<TModel>
        where TModel : DbModel
    {
        private readonly Dictionary<string, object> filters;

        public Filter()
        {
            this.filters = new Dictionary<string, object>();
        }

        public Filter(Dictionary<string, object> filters) : this()
        {
            foreach (var filter in filters)
                this.AddFilter(filter.Key, filter.Value);
        }

        public void AddFilter(string propertyName, object value)
        {
            if (!Properties.ContainsKey(propertyName))
                throw new ArgumentException("Filter");

            this.filters.Add(propertyName, value);
        }
        
        public void AddFilter(params object[] filters)
        {
            if (filters.Length % 2 != 0)
                throw new ArgumentException("Filter amount");

            for (var i = 0; i < filters.Length - 1; i += 2)
                this.AddFilter(filters[i] as string, filters[i + 1]);
        }
        
        public void SetFilter(string propertyName, object value)
        {
            this.filters[propertyName] = value;
        }

        public void RemoveFilter(string propertyName)
        {
            this.filters.Remove(propertyName);
        }

        public void RemoveFilter(params string[] filters)
        {
            if (filters.Length % 2 != 0)
                throw new ArgumentException("Filter amount");

            foreach (var filter in filters)
                this.RemoveFilter(filter);
        }

        public IQueryable<TModel> FilterQuery(IQueryable<TModel> query)
        {
            return query.Where(model => Predicate(model, this.filters));
        }

        public object this[string key]
        {
            get => this.filters[key];

            set => this.filters[key] = value;
        }
        
    }
}