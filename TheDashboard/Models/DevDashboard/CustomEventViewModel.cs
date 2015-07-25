namespace TheDashboard.Models
{
    using System.Collections.Generic;

    public class CustomEventViewModel
    {
        public string ServiceName { get; set; }

        public string EventName { get; set; }

        public IEnumerable<Handler> Handlers { get; set; }

        public class Handler
        {
            public string Name { get; set; }

            public string Namespace { get; set; }    
        }
    }
}