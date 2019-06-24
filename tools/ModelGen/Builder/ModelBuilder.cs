/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ModelBuilder
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
using ModelGen.Constants;
using ModelGen.Database;
using ModelGen.Models;

namespace ModelGen.Builder
{
    internal class ModelBuilder
    {
        private ClassBuilder builder;

        private string nameSpace;

        private string baseName;

        public ModelBuilder(string nameSpace, string baseName)
        {
            this.nameSpace = nameSpace;
            this.baseName = baseName;

            this.builder = new ClassBuilder();
        }

        public string Build(string name, IColumnObject model, bool clear = true)
        {
            var namespaces = model.Columns
                .Select(column => Configuration.Default.Types[column.Type].Namespace)
                .Distinct()
                .ToArray();

            this.builder = this.builder.AddNamespaces(namespaces)
                .StartNamespace(nameSpace)
                .StartClass(name, baseName);

            var type = default(Type);
            var typeName = default(string);
            foreach(var column in model.Columns)
            {
                type = Configuration.Default.Types[column.Type];
                typeName = 
                    Configuration.Default.UseFriendlyTypeNames &&
                    Configuration.Default.FriendlyTypeNames.TryGetValue(type, out var tName)
                    ? tName
                    : type.Name;

                if (column.IsNullable && type.IsValueType)
                    typeName += Symbols.QuestionMark;

                this.builder = this.builder.AddProperty(column.Name, typeName);
            }

            var result =  this.builder.Build();
            if (clear)
                this.builder = this.builder.Clear();

            return result;
        }
    }
}