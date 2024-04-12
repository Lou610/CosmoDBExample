using Newtonsoft.Json;

namespace GeekGroveAPI.Models
{
    public class Character
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")] 
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
