using Newtonsoft.Json;

namespace Website.Models
{
    public class System
    {
        public string Name { get; set; }

        [JsonProperty("Img")]
        public string Image { get; set; }
        public string Usage { get; set; }
        public string[] Content { get; set; }
        public Resource[] Resources { get; set; }
    }

    public class Resource
    {
        public string Text { get; set; }
        public string Link { get; set; }
    }
}
