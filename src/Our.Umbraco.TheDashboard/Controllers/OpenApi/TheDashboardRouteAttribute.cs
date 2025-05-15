using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;

namespace Our.Umbraco.TheDashboard.Controllers.OpenApi;

internal class TheDashboardRouteAttribute : RouteAttribute
{
    public TheDashboardRouteAttribute(string template)
        : base($"[{Constants.Web.AttributeRouting.BackOfficeToken}]/api/the-dashboard/" + template.TrimStart('/'))
    {
    }
}
