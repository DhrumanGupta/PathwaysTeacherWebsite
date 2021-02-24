using Newtonsoft.Json;

namespace EventJsonEncoder
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Id { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [JsonProperty("imgDefault")]
        public string DefaultImage { get; set; }
        [JsonProperty("imgHover")]
        public string HoverImage { get; set; }
    }
}
