using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LoggerService;

namespace TycheApiUtilities
{
    public class TycheApiController : ControllerBase
    {
        private Logger logger;

        public TycheApiController(Logger logger)
        {
            this.logger = logger;
        }

        [NonAction]
        public ObjectResult ApiErrorResponse(string logMessage)
        {
            var log = new LogInfo
            {
                LogType = LogType.Fatal,
                Message = logMessage,
                Time = DateTime.Now
            };

            var response = new Response
            {
                Content = Messages.InternalError,
                ResponseCode = 500
            };

            return this.ApiResponse(HttpStatusCode.InternalServerError, response, log);
        }

        [NonAction]
        public ObjectResult ApiResponse(HttpStatusCode httpStatusCode, Response response, LogInfo logInfo = null)
        {
            if (logInfo != null)
                this.logger.Log(logInfo);

            var json = JsonConvert.SerializeObject(response);
            return this.StatusCode((int)httpStatusCode, json);
        }
    }
}