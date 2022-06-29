using Newtonsoft.Json;

namespace DogFactsWeb.Models
{
    public class DogFactsResponse
    {
        [JsonProperty("facts")]
        public string[] Facts { get; set; }
    }
}
