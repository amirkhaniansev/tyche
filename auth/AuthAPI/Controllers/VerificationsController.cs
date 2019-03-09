using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DbConnectClient;
using DbConnectClient.Models;
using LoggerService;
using CodeGeneratorService;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for verifications
    /// </summary>
    [ApiController]
    [Route("api/verificatios")]
    [Produces("application/json")]
    public class VerificationsController : ControllerBase
    {
        /// <summary>
        /// Posts new verification
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Verification verification)
        {
            try
            {
                if (verification == null)
                    return this.BadRequest();

                using (var codeGenerator = new CodeGenerator())
                {
                    verification.Code = codeGenerator.GenerateVerifyKey(16);
                    verification.Created = DateTime.Now;
                    verification.ValidOffset = 30;
                }

                var request = new Request<Verification>
                {
                    Input = verification,
                    Operation = Operation.CreateVerificationCode
                };

                var response = await App.DataClient.SendRequestAsync(request);

                if (response.ResponseCode == ResponseCode.Success)
                {
                    var logInfo = new LogInfo
                    {
                        LogType = LogType.Success,
                        Time = DateTime.Now,
                        Message = Constants.VerificationCodeCreated
                    };
                    App.Logger.Log(logInfo);

                    var userRequest = new Request<int>
                    {
                        Input = verification.UserId,
                        Operation = Operation.GetUserById
                    };

                    var userResponse = await App.DataClient.SendRequestAsync(userRequest);

                    if (userResponse.ResponseCode != ResponseCode.Success)
                    {
                        return this.BadRequest();
                    }

                    var user = userResponse.Data as User;
                    App.Mailer.Send(user.Email, verification.Code);

                    return this.Ok(response);
                }

                return this.BadRequest(response);
            }
            catch (Exception ex)
            {
                var logInfo = LogHelper.CreateLog(DateTime.Now, LogType.Fatal, Constants.InternalError, ex);
                App.Logger.Log(logInfo);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Updates verification info verifying user if key is correct and not expired.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        public async Task<IActionResult> Put([FromBody]Verification verification)
        {
            try
            {
                var request = new Request<Verification>
                {
                    Input = verification,
                    Operation = Operation.VerifyUser
                };

                var response = await App.DataClient.SendRequestAsync(request);

                if (response.ResponseCode != ResponseCode.Success)
                {
                    return this.BadRequest(response);
                }

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                App.Logger.Log(new LogInfo
                {
                    LogType = LogType.Fatal,
                    Time = DateTime.Now,
                    Exception = ex,
                    Message = Constants.InternalError
                });

                return new StatusCodeResult(500);
            }
        }

    }
}