using Newtonsoft.Json;

namespace Website.Models
{
    public class Pioneer
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        [JsonProperty("ImgPath")]
        public string ImagePath { get; set; }
    }
}
