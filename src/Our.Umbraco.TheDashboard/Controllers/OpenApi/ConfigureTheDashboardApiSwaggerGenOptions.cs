using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Our.Umbraco.TheDashboard.Controllers.OpenApi;

internal class ConfigureTheDashboardApiSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
	public void Configure(SwaggerGenOptions swaggerGenOptions)
	{
		swaggerGenOptions.SwaggerDoc(
			TheDashboardApiConfiguration.ApiName,
			new OpenApiInfo
			{
				Title = TheDashboardApiConfiguration.ApiTitle,
				Version = "Latest",
				Description = $"Backoffice API for The Dashboard, for our internal use contracts might break at any time."
			});

		
		swaggerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Our.Umbraco.TheDashboard.xml"));

		//swaggerGenOptions.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

        // Enable Umbraco authentication for the Swagger document, this adds "http" and "bearer" support to the generated clients
        swaggerGenOptions.OperationFilter<TheDashboardOperationSecurityFilter>();

    }
}
