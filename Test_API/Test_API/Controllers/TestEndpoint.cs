using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEndpoint : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response> Get()
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri("https://catfact.ninja/");
                var response = await _httpClient.GetAsync("fact");
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var jsonSettings = new JsonSerializerSettings()
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    };

                    return JsonConvert.DeserializeObject<Response>(responseBody, jsonSettings);
                }

                return null;
            };
        }
    }

    public class Response
    {
        public string? Fact { get; set; }

        public int Length { get; set; }
    }
}
