using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Website.Models;
using Website.Services;

namespace Website.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly JsonFileEventService eventService;
        public IEnumerable<Event> Events { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, JsonFileEventService eventService)
        {
            this.logger = logger;
            this.eventService = eventService;
        }

        public void OnGet() => LoadEvents();

        public void OnPut() => LoadEvents();

        public void OnPost() => LoadEvents();

        private void LoadEvents()
        {
            this.Events = eventService.GetEvents();
            foreach (var _event in Events)
            {
                System.Diagnostics.Debug.WriteLine($"Name: {_event.Name}, Description: {_event.Description}");
            }
        }
    }
}
