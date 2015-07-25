using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TheDashboard.Api.Attributes;
using TheDashboard.Data;
using TheDashboard.Models;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

#pragma warning disable 612,618

#pragma warning restore 612,618

namespace TheDashboard.Api
{
    [IsBackOffice]
    [CamelCaseController]
    public class TheDevDashboardController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public DevDashboardViewModel GetViewModel()
        {
            var umbracoRepository = new UmbracoRepository();
            var dashboardViewModel = new DevDashboardViewModel();

            dashboardViewModel.HijackedRoutes =
                umbracoRepository.GetControllersAssignableFrom(typeof (IRenderMvcController), true)
                    .Select(x => new HijackedRouteViewModel
                    {
                        Name = x.Name,
                        Namespace = x.Namespace,
                        ActionMethods = x.ActionMethods,
                    })
                    .ToList();

            dashboardViewModel.SurfaceControllers =
                umbracoRepository.GetControllersAssignableFrom(typeof(SurfaceController))
                    .Select(x => new SurfaceControllerViewModel
                    {
                        Name = x.Name,
                        Namespace = x.Namespace,
                        ActionMethods = x.ActionMethods,
                    })
                    .ToList();

            dashboardViewModel.ApplicationEventHandlers =
                umbracoRepository.GetApplicationEventHandlers()
                    .Select(x => new ApplicationEventHandlerViewModel
                    {
                        Name = x.Name,
                        Namespace = x.Namespace,
                    })
                    .ToList();

            dashboardViewModel.ContentFinders =
                umbracoRepository.GetContentFinders()
                    .Select(x => new ContentFinderViewModel
                    {
                        Name = x.Name,
                        Namespace = x.Namespace,
                    })
                    .ToList();

            dashboardViewModel.CustomEvents.AddRange(GetCustomEventsForType(umbracoRepository,
                ApplicationContext.Current.Services.ContentService,
                "ContentService"));
            dashboardViewModel.CustomEvents.AddRange(GetCustomEventsForType(umbracoRepository,
                ApplicationContext.Current.Services.MediaService,
                "MediaService"));
            dashboardViewModel.CustomEvents.AddRange(GetCustomEventsForType(umbracoRepository,
                ApplicationContext.Current.Services.MemberService,
                "MemberService"));

            return dashboardViewModel;
        }

        private IEnumerable<CustomEventViewModel> GetCustomEventsForType(UmbracoRepository umbracoRepository, 
            IService serviceInstance, 
            string serviceName)
        {
            return umbracoRepository.GetCustomEvents(serviceInstance)
                .Select(x => new CustomEventViewModel
                {
                    ServiceName = serviceName,
                    EventName = x.EventName,
                    Handlers = x.Handlers
                        .Select(y => new CustomEventViewModel.Handler
                        {
                            Name = y.Name,
                            Namespace = y.Namespace,
                        })
                });
        }
    }
}