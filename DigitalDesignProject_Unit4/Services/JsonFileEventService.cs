using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using Website.Models;
using Newtonsoft.Json;

namespace Website.Services
{
    public class JsonFileEventService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public JsonFileEventService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "events.json"); }
        }

        public IEnumerable<Event> GetEvents()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonConvert.DeserializeObject<Event[]>(jsonFileReader.ReadToEnd());
            }
        }
    }
}
