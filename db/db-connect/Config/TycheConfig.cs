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