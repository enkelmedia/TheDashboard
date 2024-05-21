using Umbraco.Cms.Api.Common.OpenApi;

namespace Our.Umbraco.TheDashboard.Controllers.OpenApi;

internal class TheDashboardSchemaIdHandler : SchemaIdHandler
{
    public override bool CanHandle(Type type)
    {
        if (type.Namespace?.StartsWith("Our.Umbraco.TheDashboard") is true)
            return true;

        return false;

    }

    public override string Handle(Type type) => UmbracoSchemaId(type);
}
