using Newtonsoft.Json;

namespace Website.Models
{
    public class Culture
    {
        public string Title { get; set; }
        public string Description { get; set; }
        
        [JsonProperty("LeftImg")]
        public string LeftImage { get; set; }

        [JsonProperty("RightImg")]
        public string RightImage { get; set; }
    }
}
