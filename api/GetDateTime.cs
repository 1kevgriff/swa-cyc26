using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Api
{
    public class GetDateTime
    {
        [Function("datetime")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "datetime")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<GetDateTime>();

            try
            {
                logger.LogInformation("Processing request for current date/time in Virginia.");

                // Get Eastern Time Zone (Virginia is in Eastern Time)
                // Use IANA timezone ID for Linux/Azure compatibility
                TimeZoneInfo easternZone;
                try
                {
                    // Try IANA timezone ID first (for Linux/Azure)
                    easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    logger.LogInformation("Using IANA timezone: America/New_York");
                }
                catch (TimeZoneNotFoundException)
                {
                    try
                    {
                        // Fall back to Windows timezone ID
                        easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        logger.LogInformation("Using Windows timezone: Eastern Standard Time");
                    }
                    catch (TimeZoneNotFoundException ex)
                    {
                        logger.LogError(ex, "Could not find timezone");
                        // Use UTC as last resort
                        easternZone = TimeZoneInfo.Utc;
                        logger.LogWarning("Using UTC as fallback");
                    }
                }

                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

                // Format the datetime
                string formattedDateTime = easternTime.ToString("MMMM dd, yyyy h:mm:ss tt");
                string timeZoneName = easternZone.Id == "America/New_York" || easternZone.Id == "Eastern Standard Time"
                    ? (easternZone.IsDaylightSavingTime(easternTime) ? "EDT" : "EST")
                    : "UTC";

                // Create response
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                // Create response object
                var result = new
                {
                    dateTime = $"{formattedDateTime} {timeZoneName}",
                    timeZone = easternZone.DisplayName,
                    utcOffset = easternZone.GetUtcOffset(easternTime).ToString(),
                    serverInfo = $"Running on {Environment.OSVersion.Platform}"
                };

                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing datetime request");

                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");

                await errorResponse.WriteAsJsonAsync(new
                {
                    error = "An error occurred processing your request",
                    message = ex.Message,
                    type = ex.GetType().Name
                });

                return errorResponse;
            }
        }
    }
}