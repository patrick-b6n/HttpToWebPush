using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HttpToWebPush.Server.Features.Send;

/// <summary>
///     Based on http://docs.grafana.org/alerting/notifications/
/// </summary>
[SwaggerSchemaFilter(typeof(GrafanaHookSchemaFilter))]
public class GrafanaHookModel
{
    public string Title { get; init; } = null!;
    public string RuleName { get; init; } = null!;
    public string RuleUrl { get; init; } = null!;
    public string Message { get; init; } = null!;
}

public class GrafanaHookSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            [nameof(GrafanaHookModel.Title)] = new OpenApiString("My Message"),
            [nameof(GrafanaHookModel.RuleName)] = new OpenApiString("My Alert Rule"),
            [nameof(GrafanaHookModel.RuleUrl)] = new OpenApiString("http://my.grafana.instance"),
            [nameof(GrafanaHookModel.Message)] = new OpenApiString("My Panel Title")
        };
    }
}