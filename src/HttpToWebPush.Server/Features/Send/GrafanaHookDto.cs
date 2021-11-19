using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HttpToWebPush.Server.Features.Send;

/// <summary>
///     Based on http://docs.grafana.org/alerting/notifications/
/// </summary>
[SwaggerSchemaFilter(typeof(GrafanaHookSchemaFilter))]
public readonly record struct GrafanaHookDto
(
    string Title,
    string RuleName,
    string RuleUrl,
    string Message
);

public class GrafanaHookSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            [nameof(GrafanaHookDto.Title)] = new OpenApiString("My Message"),
            [nameof(GrafanaHookDto.RuleName)] = new OpenApiString("My Alert Rule"),
            [nameof(GrafanaHookDto.RuleUrl)] = new OpenApiString("http://my.grafana.instance"),
            [nameof(GrafanaHookDto.Message)] = new OpenApiString("My Panel Title")
        };
    }
}