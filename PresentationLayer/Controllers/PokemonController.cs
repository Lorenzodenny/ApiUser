using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace YourNamespace
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetSimplePokemonDetails(string name)
        {
            var client = _httpClientFactory.CreateClient("SitoPokemon"); // questo nome deve combaciare con quello registrato nel service con HttpClient
            var response = await client.GetAsync($"pokemon/{name}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var jsonElement = JsonDocument.Parse(jsonData).RootElement;

                var id = jsonElement.GetProperty("id").GetInt32();
                var pokemonName = jsonElement.GetProperty("name").GetString();
                var types = jsonElement.GetProperty("types").EnumerateArray();

                var simpleTypes = new List<string>();
                foreach (var typeElement in types)
                {
                    var typeDetail = typeElement.GetProperty("type");
                    simpleTypes.Add(typeDetail.GetProperty("name").GetString());
                }

                return Ok(new
                {
                    Id = id,
                    Name = pokemonName,
                    Types = simpleTypes
                });
            }

            return NotFound();
        }
    }
}
