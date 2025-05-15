using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Api.Common.OpenApi;

namespace Our.Umbraco.TheDashboard.Controllers.OpenApi;

/// <summary>
/// This is used to generate nice operation IDs in our swagger json file
/// So that the generated TypeScript client has nice method names and not too verbose
/// https://docs.umbraco.com/umbraco-cms/tutorials/creating-a-backoffice-api/umbraco-schema-and-operation-ids#operation-ids
/// </summary>
internal class TheDashboardOperationIdHandler : OperationIdHandler
{
    public TheDashboardOperationIdHandler(IOptions<ApiVersioningOptions> apiVersioningOptions) : base(apiVersioningOptions)
    {
    }

    protected override bool CanHandle(ApiDescription apiDescription, ControllerActionDescriptor controllerActionDescriptor)
    {
        return controllerActionDescriptor.ControllerTypeInfo.Namespace?.StartsWith("Our.Umbraco.TheDashboard", comparisonType: StringComparison.InvariantCultureIgnoreCase) is true;
    }

    public override string Handle(ApiDescription apiDescription) => $"{apiDescription.ActionDescriptor.RouteValues["action"]}";
}
