/**
 * GNU General Public License Version 3.0, 29 June 2007
 * TycheConfig
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
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

using System.Xml.Linq;

namespace DbConnect.Config
{
    /// <summary>
    /// Class for configuration of DbConnect
    /// </summary>
    public class TycheConfig
    {
        /// <summary>
        /// Gets ip
        /// </summary>
        public string Host { get; }
        
        /// <summary>
        /// Gets port
        /// </summary>
        public string Port { get; }
        
        /// <summary>
        /// Gets connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Creates new instance of <see cref="TycheConfig"/>
        /// </summary>
        /// <param name="path">Path</param>
        public TycheConfig(string path)
        {
            var xml = XDocument.Load(path);
            var root = xml.Element("configuration");
            this.Host = root.Element("host").Attribute("value").Value;
            this.Port = root.Element("port").Attribute("value").Value;
            this.ConnectionString = root.Element("conn").Attribute("value").Value;
        }
    }
}