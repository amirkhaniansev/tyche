using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace modelgen
{
    internal class Configuration
    {
        private static Configuration configuration;

        public static Configuration Default
        {
            get
            {
                if (configuration == null)
                {
                    configuration = JsonConvert.DeserializeObject<Configuration>(
                        File.ReadAllText("./Configuration.json"));

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
                }

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

                return configuration;
            }
        }

        public string Server { get; set; }

        public string Database { get; set; }

        public bool UseIntegratedSecurity { get; set; }

        public bool UseFriendlyTypeNames { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ContextPath { get; set; }

        public string ProjectPath { get; set; }

        public string ModelsPath { get; set; }

        [JsonIgnore]
        public Dictionary<SqlType, Type> Types { get; set; }

        [JsonIgnore]
        public Dictionary<Type, string> FriendlyTypeNames { get; set; }        
    }
}