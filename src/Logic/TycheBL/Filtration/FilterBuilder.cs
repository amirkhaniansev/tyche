/**
 * GNU General Public License Version 3.0, 29 June 2007
 * FilterBuilder
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tyche.TycheBL.Constants;

using Filter          = System.Collections.Generic.Dictionary<string, object>;
using FilterPredicate = System.Func<object, System.Collections.Generic.Dictionary<string, object>, bool>;

namespace Tyche.TycheBL.Filtration
{
    internal class FilterBuilder
    {
        private readonly Assembly assembly;

        private readonly Type modelBaseType;

        private readonly Type[] modelTypes;

        public FilterBuilder(string assemblyName, Type baseType)
        {
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentException(BlConstants.AssemblyName);

            if (baseType == null)
                throw new ArgumentNullException(BlConstants.BaseType);

            var assembly = Assembly.GetAssembly(baseType);

            this.assembly = assembly;
            this.modelBaseType = baseType;

            this.modelTypes = assembly.GetTypes()
                .Where(type => type.BaseType == baseType)
                .ToArray();
        }

        public FilterBuilder(Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(BlConstants.Types);

            if (types.Length == 0)
                throw new ArgumentException(BlConstants.TypesEmpty);

            this.modelTypes = types;
        }
        
        public ReadOnlyDictionary<Type, FilterPredicate> Build()
        {
            var filters = new Dictionary<Type, FilterPredicate>(this.modelTypes.Length);

            var boolType = typeof(bool);
            var dictionaryType = typeof(Filter);
            var containsMethodInfo = dictionaryType.GetMethod(BlConstants.ContainsKey);
            var lambdaInput = FilterBuilderHelper.GetLambdaParameterExpressions();

            var blockInput = default(BlockInput);
            var block = default(List<Expression>);
            var target = default(LabelTarget);
            var lambdaReturn = default(GotoExpression);
            var label = default(LabelExpression);
            var blockExpression = default(BlockExpression);
            var lambdaExpression = default(Expression<Func<object, Filter, bool>>);
            var lambda = default(Func<object, Filter, bool>);

            foreach (var type in this.modelTypes)
            {
                blockInput = FilterBuilderHelper.GetBlockParameterExpressions(type);
                block = FilterBuilderHelper.GetBlockExpressions(
                    containsMethodInfo,
                    type,
                    blockInput,
                    lambdaInput);

                target = Expression.Label(boolType);
                lambdaReturn = Expression.Return(target, blockInput.ReturnValue, boolType);
                label = Expression.Label(target, Expression.Default(boolType));

                blockExpression = Expression.Block(blockInput.ToList(), block);
                lambdaExpression = Expression.Lambda<Func<object, Filter, bool>>(
                    blockExpression, lambdaInput.ToList());

                lambda = lambdaExpression.Compile();

                filters.Add(type, lambda);
            }

            return new ReadOnlyDictionary<Type, FilterPredicate>(filters);
        }       
    }
}