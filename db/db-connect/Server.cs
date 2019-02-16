/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Server
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

using AccessCore.SpExecuters;
using AccessCore.Repository;
using AccessCore.Repository.MapInfos;
using DbConnect.Config;
using DbConnect.Models;

namespace DbConnect
{
    /// <summary>
    /// Class for holding global information
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Gets or sets Tyche Config
        /// </summary>
        public static TycheConfig TycheConfig { get; set; } 

        /// <summary>
        /// Gets or sets Data Manager
        /// </summary>
        public static DataManager DataManager { get; set; }

        /// <summary>
        /// Gets or sets Xml Map Information
        /// </summary>
        public static XmlMapInfo XmlMapInfo { get; set; }

        /// <summary>
        /// Gets or sets sp executer
        /// </summary>
        public static MsSqlSpExecuter MsSqlSpExecuter { get; set; }
        
        /// <summary>
        /// Gets or sets Server
        /// </summary>
        public static DataServer DataServer { get; set; }

        /// <summary>
        /// Initializes globals
        /// </summary>
        public static void Initialize()
        {
            TycheConfig = new TycheConfig("./Config/config.xml");
            XmlMapInfo = new XmlMapInfo("Config/map.xml");
            MsSqlSpExecuter = new MsSqlSpExecuter(TycheConfig.ConnectionString);
            DataManager = new DataManager(MsSqlSpExecuter, XmlMapInfo);
        }

        /// <summary>
        /// Prepares the server for starting.
        /// </summary>
        public static void Prepare()
        {
            DataServer = new DataServer(DataManager, TycheConfig.Host, TycheConfig.Port)
                .AddDataOperation<ChatCreationDescriptor>(DbOperationType.CreateChatRooom)
                .AddDataOperation<UserVerificationDescriptor>(DbOperationType.VerifyUser)
                .AddDataOperation<VerificationCodeDescriptor>(DbOperationType.CreateVerificationCode);
        }

        /// <summary>
        /// Starts server
        /// </summary>
        public static void Start()
        {
            DataServer.Run();
        }
    }
}