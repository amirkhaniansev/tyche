/**
 * GNU General Public License Version 3.0, 29 June 2007
 * FilterBase
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheDAL.Filtration
{
    public class FilterBase<TModel> where TModel : DbModel
    {
        protected readonly static Type ModelType;

        protected readonly static Type DictionaryType;

        protected readonly static Type BoolType;

        protected readonly static Type Filterable;

        protected readonly static Type NonFilterable;

        protected readonly static Dictionary<string, PropertyInfo> Properties;

        protected readonly static Func<TModel, Dictionary<string, object>, bool> Predicate;
        
        static FilterBase()
        {
            ModelType = typeof(TModel);
            BoolType = typeof(bool);
            Filterable = typeof(FilterableAttribute);
            NonFilterable = typeof(NonFilterableAttribute);
            DictionaryType = typeof(Dictionary<string, object>);

            var properties = ModelType.GetProperties();
            
            if(ModelType.GetCustomAttribute(Filterable) == null)
            {
                properties = properties
                    .Where(p => p.GetCustomAttribute(Filterable) != null)
                    .ToArray();
            }
            else
            {
                properties = properties
                    .Where(p => p.GetCustomAttribute(NonFilterable) == null)
                    .ToArray();
            }
            
            Properties = properties.ToDictionary(property => property.Name);

            var model = Expression.Parameter(ModelType);
            var filter = Expression.Parameter(DictionaryType);

            var lambdaInput = new List<ParameterExpression>
            {
                model, filter
            };

            var result = Expression.Parameter(BoolType);
            var blockkInput = new List<ParameterExpression>
            {
                result
            };

            var block = new List<Expression>();

            var trueValue = Expression.Constant(true, BoolType);
            var setResultTrue = Expression.Assign(result, trueValue);
            block.Add(setResultTrue);
            
            foreach (var property in properties)
            {
                block.Add(Expression.IfThen(
                    Expression.Call(
                        filter,
                        DictionaryType.GetMethod("ContainsKey"),
                        Expression.Constant(property.Name)),
                    Expression.AndAssign(
                        result,
                        Expression.Equal(
                            Expression.Property(
                                model,
                                property),
                            Expression.Convert(
                                Expression.Property(
                                    filter,
                                    "Item",
                                    Expression.Constant(property.Name)),
                                property.PropertyType)))));
            }

            var target = Expression.Label(BoolType);
            var lambdaReturn = Expression.Return(target, result, BoolType);
            var label = Expression.Label(target, Expression.Default(BoolType));

            block.Add(lambdaReturn);
            block.Add(label);

            var blockExpression = Expression.Block(blockkInput, block);
            var lambdaExpression = Expression.Lambda<Func<TModel, Dictionary<string, object>, bool>>(
                blockExpression, lambdaInput);

            Predicate = lambdaExpression.Compile();
        }
    }
}