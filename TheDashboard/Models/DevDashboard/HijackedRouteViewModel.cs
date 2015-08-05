namespace TheDashboard.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class HijackedRouteViewModel : BaseControllerViewModel
    {
        public string DocumentType
        {
            get { return Name.Replace("Controller", string.Empty); }
        }

        public string Templates
        {
            get { return ActionMethods.Contains("Index") ? "All" : string.Join(", ", ActionMethods); }
        }
    }
}