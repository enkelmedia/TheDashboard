namespace TheDashboard.Data.DTO
{
    using System.Collections.Generic;

    public class CustomEventDto
    {
        public string EventName { get; set; }

        public IEnumerable<ReflectedClassDto> Handlers { get; set; }
    }
}