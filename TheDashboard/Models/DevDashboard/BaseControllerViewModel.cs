namespace TheDashboard.Models
{
    using System.Collections.Generic;

    public abstract class BaseControllerViewModel : BaseClassViewModel
    {
        public IEnumerable<string> ActionMethods { get; set; }
    }
}