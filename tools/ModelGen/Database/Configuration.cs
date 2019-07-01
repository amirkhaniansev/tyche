/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Configuration
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
using Newtonsoft.Json;

namespace ModelGen.Database
{
    internal class Configuration
    {
        private static Configuration configuration;

        public static Configuration Default
        {
            get => configuration;

            set
            {
                configuration = value;

                configuration.Types = new Dictionary<SqlType, Type>
                {
                    [SqlType.BigInt] = typeof(long),
                    [SqlType.Binary] = typeof(byte[]),
                    [SqlType.Bit] = typeof(bool),
                    [SqlType.Char] = typeof(string),
                    [SqlType.Date] = typeof(DateTime),
                    [SqlType.DateTime] = typeof(DateTime),
                    [SqlType.DateTime2] = typeof(DateTimeOffset),
                    [SqlType.DateTimeOffset] = typeof(DateTimeOffset),
                    [SqlType.Decimal] = typeof(decimal),
                    [SqlType.Float] = typeof(double),
                    [SqlType.Image] = typeof(byte[]),
                    [SqlType.Int] = typeof(int),
                    [SqlType.Money] = typeof(decimal),
                    [SqlType.NChar] = typeof(string),
                    [SqlType.NText] = typeof(string),
                    [SqlType.Numeric] = typeof(decimal),
                    [SqlType.NVarChar] = typeof(string),
                    [SqlType.Real] = typeof(float),
                    [SqlType.SmallDateTime] = typeof(DateTime),
                    [SqlType.SmallInt] = typeof(short),
                    [SqlType.SmallMoney] = typeof(decimal),
                    [SqlType.SqlVariant] = typeof(object),
                    [SqlType.Text] = typeof(string),
                    [SqlType.Time] = typeof(TimeSpan),
                    [SqlType.TimeStamp] = typeof(byte[]),
                    [SqlType.TinyInt] = typeof(byte),
                    [SqlType.UniqueIdentifier] = typeof(Guid),
                    [SqlType.VarBinary] = typeof(byte[]),
                    [SqlType.VarChar] = typeof(string)
                };


                configuration.FriendlyTypeNames = new Dictionary<Type, string>
                {
                    [typeof(bool)] = "bool",
                    [typeof(byte)] = "byte",
                    [typeof(short)] = "short",
                    [typeof(int)] = "int",
                    [typeof(long)] = "long",
                    [typeof(byte[])] = "byte[]",
                    [typeof(string)] = "string",
                    [typeof(float)] = "float",
                    [typeof(double)] = "double",
                    [typeof(decimal)] = "decimal",
                    [typeof(object)] = "object"
                };
            }
        }

        public string Server { get; set; }

        public string Database { get; set; }

        public string TableModelNamespace { get; set; }

        public string FunctionModelNamespace { get; set; }
        
        public bool UseIntegratedSecurity { get; set; }

        public bool UseFriendlyTypeNames { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ContextPath { get; set; }

        public string ProjectPath { get; set; }
        
        public string BaseModel { get; set; }

        public string BaseModelNamespace { get; set; }

        public string QueriesPath { get; set; }

        [JsonIgnore]
        public Dictionary<SqlType, Type> Types { get; set; }

        [JsonIgnore]
        public Dictionary<Type, string> FriendlyTypeNames { get; set; }        
    }
}