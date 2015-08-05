namespace TheDashboard.Data.DTO
{
    using System.Collections.Generic;

    public class ControllerDetailDto : ReflectedClassDto
    {
        public IEnumerable<string> ActionMethods { get; set; }
    }
}