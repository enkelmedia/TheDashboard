using Umbraco.Cms.Api.Management.OpenApi;

namespace Our.Umbraco.TheDashboard.Controllers.OpenApi;



internal class TheDashboardOperationSecurityFilter : BackOfficeSecurityRequirementsOperationFilterBase
{
    protected override string ApiName => TheDashboardApiConfiguration.ApiName;
}
