using AccessCore.Repository;
using AccessCore.Repository.MapInfos;
using AccessCore.SpExecuters;
using System;
using System.Threading.Tasks;

namespace BusinessLogicTest
{
    class Program
    {
        internal static XmlMapInfo MapInfo;
        internal static MsSqlSpExecuter MsSqlSpExecuter;
        internal static DataManager DataManager;

        static void Main(string[] args)
        {
            MapInfo = new XmlMapInfo("map.xml");
            MapInfo.SetMapInfo();
            MsSqlSpExecuter = new MsSqlSpExecuter("Data Source=(local);Initial Catalog=TycheDB;Integrated Security=True");
            DataManager = new DataManager(MsSqlSpExecuter, MapInfo);

            TestUserBl().Wait();
        }

        static async Task TestUserBl()
        {
            await UsersBlTest.TestCreateUser();
            await UsersBlTest.TestCreateVerificationForUser();
            await UsersBlTest.TestGetUserById();
            await UsersBlTest.TestGetUserByUsername();
            await UsersBlTest.VerifiyUser();
        }
    }
}
