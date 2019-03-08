using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TycheBL;
using TycheBL.Logic;
using TycheBL.Models;

namespace Test
{
    class Program
    {
        static string cnn = "Data Source=(local);Initial Catalog=TycheDB;Integrated Security=True";

        static void Print(string name, DbResponse dbResponse)
        {
            Console.WriteLine(name + ":");
            Console.WriteLine(JsonConvert.SerializeObject(dbResponse, Formatting.Indented));
            Console.ReadLine();
        }
        
        static async Task TestCreateUser()
        {
            using (var bl = new UsersBL(cnn))
            {
                var user = new User
                {
                    Username = "pablo",
                    FirstName = "Pablo",
                    LastName = "Fernandez",
                    Email = "pablo@gmail.com",
                    ProfilePictureUrl = "",
                    PasswordHash = "password"
                };

                var response = await bl.CreateUser(user);

                Print("CreateUser", response);
            };
        }

        static async Task TestCreateVerification()
        {
            using (var bl = new UsersBL(cnn))
            {
                var verification = new Verification
                {
                    Created = DateTime.Now,
                    Code = "code",
                    UserId = 100007,
                    ValidOffset = 30
                };

                var response = await bl.CreateVerificationForUser(verification);

                Print("CreateVerification", response);
            }
        }

        static async Task TestVerifyUser()
        {
            using (var bl = new UsersBL(cnn))
            {
                var verification = new Verification
                {
                    Created = DateTime.Now,
                    Code = "code",
                    UserId = 100007
                };

                var response = await bl.VerifyUser(verification);
                
                Print("VerifyUser", response);
            }
        }

        static async Task TestGetUserById()
        {
            using (var bl = new UsersBL(cnn))
            {
                var response = await bl.GetUserById(100007);

                Print("GetUserById", response);
            }
        }

        static async Task TestGetUsersByUsername()
        {
            using (var bl = new UsersBL(cnn))
            {
                var response = await bl.GetUsersByUsername("pab");

                Print("GetUsersByUsername", response);
            }
        }

        static async Task TestCreateMessage()
        {
            using (var bl = new MessagesBL(cnn))
            {
                var message = new Message
                {
                    Text = "NewMessage",
                    Created = DateTime.Now,
                    From = 100007,
                    To = 1
                };

                var response = await bl.CreateMessage(message);

                Print("CreateMessage", response);
            }
        }

        static async Task TestGetMessages()
        {
            using (var bl = new MessagesBL(cnn))
            {
                var filter = new MessageFilter
                {
                    ChatroomId = 1,
                    FromDate = DateTime.MinValue,
                    ToDate = DateTime.Now
                };

                var response = await bl.GetMessages(filter);

                Print("GetMessages", response);
            }
        }

        static void Main(string[] args)
        {
            TestCreateUser().Wait();
            TestCreateVerification().Wait();
            TestVerifyUser().Wait();
            TestGetUserById().Wait();
            TestGetUsersByUsername().Wait();
            TestCreateMessage().Wait();
            TestGetMessages().Wait();
        }
    }
}
