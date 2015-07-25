using System.Collections.Generic;

namespace TheDashboard.Models
{
    public class DevDashboardViewModel
    {
        public DevDashboardViewModel()
        {
            HijackedRoutes = new List<HijackedRouteViewModel>();
            SurfaceControllers = new List<SurfaceControllerViewModel>();
            ApplicationEventHandlers = new List<ApplicationEventHandlerViewModel>();
            CustomEvents = new List<CustomEventViewModel>(); 
            ContentFinders = new List<ContentFinderViewModel>();
        }

        public List<HijackedRouteViewModel> HijackedRoutes { get; set; }

        public List<SurfaceControllerViewModel> SurfaceControllers { get; set; }

        public List<ApplicationEventHandlerViewModel> ApplicationEventHandlers { get; set; }

        public List<CustomEventViewModel> CustomEvents { get; set; }

        public List<ContentFinderViewModel> ContentFinders { get; set; }
    }
}