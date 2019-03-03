using System.Threading.Tasks;
using TycheBL;
using TycheBL.Models;

namespace BusinessLogicTest
{
    static class UsersBlTest
    {
        internal static async Task TestCreateUser()
        {
            var user = new User
            {
                FirstName = "UserFirstName",
                LastName = "UserLastName",
                Username = "username",
                PasswordHash = "password",
                Email = "username@gmail.com",
                ProfilePictureUrl = "empty"
            };

            var userbl = new UsersBL(Program.DataManager);
            var response = await userbl.CreateUser(user);
            Print.PrintDbResponse("CreateUser", response);
        }

        internal static async Task TestCreateVerificationForUser()
        {
            var verification = new Verification
            {
                Code = "verification_code",
                UserId = 100000,
                ValidOffset = 50
            };

            var userbl = new UsersBL(Program.DataManager);
            var response = await userbl.CreateVerificationForUser(verification);
            Print.PrintDbResponse("CreateVerificationForUser", response);
        }

        internal static async Task VerifiyUser()
        {
            var verification = new Verification
            {
                Code = "verification_code",
                UserId = 100000
            };

            var userbl = new UsersBL(Program.DataManager);
            var response = await userbl.VerifyUser(verification);
            Print.PrintDbResponse("VerifyUser", response);
        }

        internal static async Task TestGetUserById()
        {
            var userBl = new UsersBL(Program.DataManager);
            var response = await userBl.GetUserById(100000);
            Print.PrintDbResponse("GetUserById", response);
        }

        internal static async Task TestGetUserByUsername()
        {
            var userBl = new UsersBL(Program.DataManager);
            var response = await userBl.GetUsersByUsername("sev");
            Print.PrintDbResponse("GetUserByUsername", response);
        }
    }
}