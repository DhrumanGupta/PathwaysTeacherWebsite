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

        [JsonProperty("img")]
        public string Image { get; set; }
    }
}
