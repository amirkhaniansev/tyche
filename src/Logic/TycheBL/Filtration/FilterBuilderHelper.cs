/**
 * GNU General Public License Version 3.0, 29 June 2007
 * FilterBuilderHelper
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
using System.Linq.Expressions;
using System.Reflection;
using Tyche.TycheBL.Constants;

using Filter = System.Collections.Generic.Dictionary<string, object>;

namespace Tyche.TycheBL.Filtration
{
    internal static class FilterBuilderHelper
    {
        internal static BlockInput GetBlockParameterExpressions(Type type)
        {
            return new BlockInput
            {
                ModelVariable = Expression.Parameter(type),
                ReturnValue = Expression.Parameter(typeof(bool))
            };
        }

        internal static LambdaInput GetLambdaParameterExpressions()
        {
            return new LambdaInput
            {
                ModelParameter = Expression.Parameter(typeof(object)),
                FilterParameter = Expression.Parameter(typeof(Filter))
            };
        }

        internal static List<Expression> GetBlockExpressions(MethodInfo containsMethodInfo, Type type, BlockInput blockInput, LambdaInput lambdaInput)
        {
            var properties = type.GetProperties();

            var block = new List<Expression>();

            var modelConvert = Expression.TypeAs(lambdaInput.ModelParameter, type);
            var modelAssign = Expression.Assign(blockInput.ModelVariable, modelConvert);

            var trueConstant = Expression.Constant(true);
            var returnValueSetTrue = Expression.Assign(blockInput.ReturnValue, trueConstant);

            block.Add(modelAssign);
            block.Add(returnValueSetTrue);

            foreach (var property in properties)
            {
                block.Add(GetIfContainsThenExpression(
                    containsMethodInfo, 
                    property, 
                    blockInput, 
                    lambdaInput));
            }

            var boolType = typeof(bool);
            var target = Expression.Label(boolType);
            var lambdaReturn = Expression.Return(target, blockInput.ReturnValue, boolType);
            var label = Expression.Label(target, Expression.Default(boolType));

            block.Add(lambdaReturn);
            block.Add(label);

            return block;
        }

        internal static ConditionalExpression GetIfContainsThenExpression(
            MethodInfo containsMethodInfo,
            PropertyInfo propertyInfo,
            BlockInput blockInput, 
            LambdaInput lambdaInput)
        {
            var containsParameter = Expression.Constant(propertyInfo.Name);
            var containsCall = Expression.Call(
                lambdaInput.FilterParameter, containsMethodInfo, containsParameter);

            var value = Expression.Property(
                lambdaInput.FilterParameter, BlConstants.Item, Expression.Constant(propertyInfo.Name));

            var valueConvert = Expression.Convert(value, propertyInfo.PropertyType);

            var propertyValue = Expression.Property(blockInput.ModelVariable, propertyInfo);

            var equals = Expression.Equal(propertyValue, valueConvert);
            var and = Expression.And(blockInput.ReturnValue, equals);
            var andInit = Expression.Assign(blockInput.ReturnValue, and);

            return Expression.IfThen(containsCall, andInit);
        }
    }
}