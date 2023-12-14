using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hike.API.Filter;

public class LineStringSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(LineString)) return;

        var options = new JsonSerializerOptions();
        options.Converters.Add(new GeoJsonConverterFactory());
        var exampleFeatureString = JsonSerializer.Serialize(
            new LineString(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(3, 4)
            })
            , options);
        schema.Example = new OpenApiString(exampleFeatureString);
        schema.Default = new OpenApiString(exampleFeatureString);
    }
}