using DMed_Razor.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MyApp.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTest()
        {

            string apiKey = "4a34abb2aeb910c90072879c8cc44841f259a4a878e580472b3a56a28f319f61";
            string testTitle = "My Test";
            string testInstructions = "This is a test created using the HackerRank API.";
            string testDuration = "60"; // Test duration in minutes

            try
            {
                // Create the test
                string testId = await CreateTest(apiKey, testTitle, testInstructions, testDuration);
                return Ok("Test created successfully with ID: " + testId);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        public async Task<string> CreateTest(string apiKey, string testTitle, string testInstructions, string testDuration)
        {
            string apiUrl = "https://www.hackerrank.com/api/tests";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

                // Prepare the request body
                var requestBody = new
                {
                    test = new
                    {
                        name = testTitle,
                        instructions = testInstructions,
                        duration = testDuration,
                        allow_solutions_after_end = false
                    }
                };

                // Convert the request body to JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request to create the test
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Extract the test ID from the response
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                    string testId = responseData?.data?.id;

                    return testId;
                }
                else
                {
                    throw new Exception("Failed to create the test. Status code: " + response.StatusCode);
                }
            }
        }
        [HttpPost("publish")]
        public async Task<IActionResult> PublishTest(int testId)
        {
            var url = _configuration["HackerRankApiUrl"] + "/v3/tests/" + testId + "/publish";
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + _configuration["HackerRankApiKey"] }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", "Bearer " + _configuration["HackerRankApiKey"]);

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetTestResults(int testId)
        {
            var url = _configuration["HackerRankApiUrl"] + "/v3/tests/" + testId + "/results";
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + _configuration["HackerRankApiKey"] }
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + _configuration["HackerRankApiKey"]);

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var results = JsonConvert.DeserializeObject<List<TestResult>>(await response.Content.ReadAsStringAsync());
                return Ok(results);
            }
            else
            {
                return BadRequest();
            }
        }
    }

    public class Test
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class TestResult
    {
        public int UserId { get; set; }
        public int Score { get; set; }
    }
}
