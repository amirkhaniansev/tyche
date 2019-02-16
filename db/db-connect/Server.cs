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
            TycheConfig = new TycheConfig("Config/config.xml");
            XmlMapInfo = new XmlMapInfo("Config/map.xml");
            MsSqlSpExecuter = new MsSqlSpExecuter(TycheConfig.ConnectionString);
            DataManager = new DataManager(MsSqlSpExecuter, XmlMapInfo);
        }

        /// <summary>
        /// Prepares the server for starting.
        /// </summary>
        public static void Prepare()
        {
            DataServer = DataServer
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