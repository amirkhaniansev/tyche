using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace modelgen
{
    internal sealed class Scheme
    {
        private Dictionary<string, string> queries;

        private string connectionString;
        
        public IEnumerable<Table> Tables { get; set; }

        public IEnumerable<Function> Functions { get; set; }

        public IEnumerable<Procedure> Procedures { get; set; }
        
        public Scheme ()
        {
            var setting = Configuration.Default;
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = setting.Server,
                InitialCatalog = setting.Database
            };

            if (setting.UseIntegratedSecurity)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = setting.Username;
                builder.Password = setting.Password;
            }

            this.connectionString = builder.ToString();
        }

        public async Task InitializeQueries()
        {
            this.queries = new Dictionary<string, string>
            {
                ["FunctionColumns"]         = await File.ReadAllTextAsync($"./Queries/FunctionColumns.sql"),
                ["Functions"]               = await File.ReadAllTextAsync($"./Queries/Functions.sql"),
                ["FunctionsParameters"]     = await File.ReadAllTextAsync($"./Queries/FunctionsParameters.sql"),
                ["Procedures"]              = await File.ReadAllTextAsync($"./Queries/Procedures.sql"),
                ["ProceduresParameters"]    = await File.ReadAllTextAsync($"./Queries/ProceduresParameters.sql"),
                ["Tables"]                  = await File.ReadAllTextAsync($"./Queries/Tables.sql"),
                ["TablesColumns"]           = await File.ReadAllTextAsync($"./Queries/TablesColumns.sql")
            };
        }

        public async Task InitializeTables()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                this.Tables = await connection.QueryAsync<Table>(
                    this.queries["Tables"]);

                var columns = await connection.QueryAsync<TableColumn>(
                    this.queries["TablesColumns"]);
                foreach (var table in this.Tables)
                    table.Columns = columns.Where(c => c.TableId == table.Id).ToList();
            }
        }

        public async Task InitializeFunctions()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                this.Functions = await connection.QueryAsync<Function>(
                    this.queries["Functions"]);

                var parameters = await connection.QueryAsync<FunctionParameter>(
                    this.queries["FunctionsParameters"]);
                var columns = await connection.QueryAsync<FunctionColumn>(
                    this.queries["FunctionColumns"]);

                foreach(var function in this.Functions)
                {
                    function.Parameters = parameters
                        .Where(p => p.FunctionId == function.Id)
                        .ToList();
                    function.Columns = columns
                        .Where(c => c.FunctionId == function.Id)
                        .ToList();
                }
            }
        }

        public async Task InitializeProcedures()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                this.Procedures = await connection.QueryAsync<Procedure>(
                    this.queries["Procedures"]);

                var parameters = await connection.QueryAsync<ProcedureParameter>(
                    this.queries["ProceduresParameters"]);
                foreach (var procedure in this.Procedures)
                    procedure.Parameters = parameters
                        .Where(p => p.ProcedureId == procedure.Id)
                        .ToList();
            }
        }
    }
}