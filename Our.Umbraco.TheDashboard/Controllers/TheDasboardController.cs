using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.TheDashboard.Counters.Collections;
using Our.Umbraco.TheDashboard.Mapping;
using Our.Umbraco.TheDashboard.Models.Dtos;
using Our.Umbraco.TheDashboard.Models.Frontend;
using Our.Umbraco.TheDashboard.Security;
using Our.Umbraco.TheDashboard.Services;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using Our.Umbraco.TheDashboard.Controllers.OpenApi;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Common.Filters;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common.Authorization;
using Microsoft.AspNetCore.Http;


namespace Our.Umbraco.TheDashboard.Controllers;

internal static class EndpointCampaignConfiguration
{
    public const string RouteSegment = "dashboard";
    public const string GroupName = "TheDashboard";
}

[ApiController]
[PluginController("TheDashboard")]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
[JsonOptionsName(Constants.JsonOptionsNames.BackOffice)]
[MapToApi(TheDashboardApiConfiguration.ApiName)]
[TheDashboardRoute(EndpointCampaignConfiguration.RouteSegment)]
[ApiExplorerSettings(GroupName = EndpointCampaignConfiguration.GroupName)]
public class TheDashboardController : ControllerBase
{
    private readonly AppCaches _appCaches;
    private readonly IScopeProvider _scopeProvider;
    private readonly ITheDashboardService _dashboardService;
    private readonly DashboardCountersCollection _dashboardCountersCollection;
    private readonly IUserService _userService;
    private readonly IBackOfficeSecurity _security;
    private readonly IEntityService _entityService;
    private readonly IHttpClientFactory _httpClientFactory;

    public TheDashboardController(AppCaches appCaches, 
        IScopeProvider scopeProvider,
        ITheDashboardService dashboardService, 
        DashboardCountersCollection dashboardCountersCollection,
        IUserService userService,
        IBackOfficeSecurity security,
        IEntityService entityService,
        IHttpClientFactory httpClientFactory
    )
    {
        _appCaches = appCaches;
        _scopeProvider = scopeProvider;
        _dashboardService = dashboardService;
        _dashboardCountersCollection = dashboardCountersCollection;
        _userService = userService;
        _security = security;
        _entityService = entityService;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("get-all-recent-activities")]
    [ProducesResponseType(typeof(RecentActivitiesFrontendModel), StatusCodes.Status200OK)]
    public RecentActivitiesFrontendModel GetAllRecentActivities()
    {
            
        var model = new RecentActivitiesFrontendModel();
        model.AllItems = new List<RecentActivityFrontendModel>();
        model.YourItems = new List<RecentActivityFrontendModel>();

        var allRecentDtos = _dashboardService.GetEntries();

        var accessService = new UserToNodeAccessHelper(_security.CurrentUser, _userService,_entityService,_appCaches, allRecentDtos);
        var filteredDtos = allRecentDtos.Where(x => accessService.HasAccessTo(x)).ToList();

        model.AllItems = CreateFrontendModelsFrom(filteredDtos);

        // filter to only my own
        var filteredMyActivitiesDtos = filteredDtos.Where(x => x.UserId == _security.CurrentUser.Id).ToList();
        model.YourItems = CreateFrontendModelsFrom(filteredMyActivitiesDtos);

        return model;
    }

    [HttpGet("get-pending")]
    [ProducesResponseType(typeof(PendingContentNotScheduledFrontendModel), StatusCodes.Status200OK)]
    public PendingContentNotScheduledFrontendModel GetPending()
    {
        var model = new PendingContentNotScheduledFrontendModel();
        model.Items = new List<RecentActivityFrontendModel>();
            
        var allRecentDtos = _dashboardService.GetPending();

        var accessService = new UserToNodeAccessHelper(_security.CurrentUser, _userService,_entityService,_appCaches, allRecentDtos);
        var filteredDtos = allRecentDtos.Where(x => accessService.HasAccessTo(x)).ToList();

        model.Items = CreateFrontendModelsFrom(filteredDtos);
        model.Count = model.Items.Count;
        return model;
    }

    [HttpGet("get-counters")]
    [ProducesResponseType(typeof(CountersFrontendModel), StatusCodes.Status200OK)]
    public CountersFrontendModel GetCounters()
    {
        var model = new CountersFrontendModel();

        using (var scope = _scopeProvider.CreateScope())
        {
            foreach (var counter in _dashboardCountersCollection)
            {
                model.Counters.Add(counter.GetModel(scope));
            }
        }

        return model;
    }

    private List<RecentActivityFrontendModel> CreateFrontendModelsFrom(List<LogEntryDto> dtos)
    {
        var maxCount = 10;
        var mapper = new LogEntryToRecentActivityMapper(_appCaches, _httpClientFactory);

        // Should return a list of models containing unique items for the nodeId.
        var list = new List<RecentActivityFrontendModel>();

        for (int i = 0; i < dtos.Count; i++)
        {
            var dto = dtos[i];
            // If we already have a item in the list with the same nodeId this means that the latest activity
            // for this nodeid already has been added.
            if (!list.Any(x => x.NodeId == dto.NodeId))
            {
                // try to create a view model, this will return null when the model is not of a valid type.
                var vm = mapper.Map(dto);
                if (vm != null)
                {
                    list.Add(vm);
                }
            }

            if (list.Count >= maxCount)
                return list;

        }

        return list;

    }

}
