using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Website.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Id { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [JsonPropertyName("imgDefault")]
        public string DefaultImage { get; set; }
        [JsonPropertyName("imgHover")]
        public string HoverImage { get; set; }
    }
}
