using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Api
{
    public class GetJohnWickQuote
    {
        private static readonly Quote[] Quotes = new[]
        {
            new Quote("Yeah, I'm thinking I'm back.", "John Wick (2014)"),
            new Quote("Guns. Lots of guns.", "John Wick: Chapter 3 (2019)"),
            new Quote("Be seeing you.", "John Wick (2014)"),
            new Quote("Consider this a professional courtesy.", "John Wick: Chapter 2 (2017)"),
            new Quote("Everything's got a price.", "John Wick (2014)"),
            new Quote("I'd like to make a reservation for 12.", "John Wick: Chapter 3 (2019)"),
            new Quote("You want a war? Or do you want to just give me a gun?", "John Wick: Chapter 2 (2017)"),
            new Quote("I'm not that guy anymore.", "John Wick (2014)"),
            new Quote("Consequences.", "John Wick: Chapter 3 (2019)"),
            new Quote("Si vis pacem, para bellum.", "John Wick: Chapter 3 (2019)"),
            new Quote("Fortunes favor the bold.", "John Wick: Chapter 4 (2023)"),
            new Quote("I have served. I will be of service.", "John Wick: Chapter 3 (2019)"),
            new Quote("Rules. Without them we'd live with the animals.", "John Wick: Chapter 2 (2017)"),
            new Quote("The marker is sacred.", "John Wick: Chapter 2 (2017)"),
            new Quote("I need something robust. Precise.", "John Wick (2014)"),
            new Quote("No business on Continental grounds.", "John Wick (2014)"),
            new Quote("Tick tock, Mr. Wick.", "John Wick: Chapter 3 (2019)"),
            new Quote("Welcome to the Continental.", "John Wick (2014)"),
            new Quote("A man has to look his best when it's time to get married... or buried.", "John Wick: Chapter 2 (2017)"),
            new Quote("Baba Yaga.", "John Wick (2014)")
        };

        private readonly Random _random = new Random();

        [Function("johnwick-quote")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "johnwick-quote")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<GetJohnWickQuote>();

            try
            {
                logger.LogInformation("Processing request for John Wick quote.");

                // Get a random quote
                var randomIndex = _random.Next(Quotes.Length);
                var selectedQuote = Quotes[randomIndex];

                // Create response
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                // Create response object
                var result = new
                {
                    quote = selectedQuote.Text,
                    movie = selectedQuote.Movie
                };

                await response.WriteAsJsonAsync(result);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing John Wick quote request");

                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");

                await errorResponse.WriteAsJsonAsync(new
                {
                    error = "An error occurred processing your request",
                    message = ex.Message
                });

                return errorResponse;
            }
        }

        private record Quote(string Text, string Movie);
    }
}