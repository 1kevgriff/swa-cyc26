using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api
{
    public class GetDateTime
    {
        [Function("datetime")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "datetime")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<GetDateTime>();
            logger.LogInformation("Processing request for current date/time in Virginia.");

            // Get Eastern Time Zone (Virginia is in Eastern Time)
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            // Format the datetime
            string formattedDateTime = easternTime.ToString("MMMM dd, yyyy h:mm:ss tt");
            string timeZoneName = easternZone.IsDaylightSavingTime(easternTime) ? "EDT" : "EST";

            // Create response
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            // Create response object
            var result = new
            {
                dateTime = $"{formattedDateTime} {timeZoneName}",
                timeZone = easternZone.DisplayName,
                utcOffset = easternZone.GetUtcOffset(easternTime).ToString()
            };

            response.WriteAsJsonAsync(result);

            return response;
        }
    }
}