/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Program
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

using System.IO;
using System.Threading.Tasks;
using ModelGen.Builder;
using ModelGen.Constants;
using ModelGen.Database;

namespace ModelGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scheme = new Scheme();

            await scheme.InitializeQueries();
            await scheme.InitializeTables();
            await scheme.InitializeFunctions();
            await scheme.InitializeProcedures();

            if (!Directory.Exists(Configuration.Default.ProjectPath))
                return;

            var modelsPath = $@"{Configuration.Default.ProjectPath}/{Paths.Models}";
            var functionModelsPath = $@"
                {Configuration.Default.ProjectPath}/
                {Paths.Models}/
                {Paths.FunctionModels}";

            if (Directory.Exists(modelsPath))
                Directory.Delete(modelsPath, true);

            Directory.CreateDirectory(modelsPath);
            Directory.CreateDirectory(functionModelsPath);

            await FileBuilder.CreateModels(
                Configuration.Default.TableModelNamespace,
                Configuration.Default.BaseModel,
                modelsPath,
                model => model.Name.Substring(0, model.Name.Length - 1),
                scheme.Tables);

            await FileBuilder.CreateModels(
                Configuration.Default.FunctionModelNamespace,
                Configuration.Default.BaseModel,
                functionModelsPath,
                model => model.Name.Substring(4, model.Name.Length - 5),
                scheme.Functions);
        }
    }
}