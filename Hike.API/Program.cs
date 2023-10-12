using System.Text.Json;
using System.Text.Json.Serialization;
using Hike.API;
using Hike.Data.Extensions;
using Hike.Logic.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(options =>
{
    options.SchemaFilter<LineStringSchemaFilter>();
});

//Dependency Injection of Hike.Logic + Hike.Data
builder.Services.AddLogic().AddData();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Hike.API
{
    public class LineStringSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(LineString))
            {
                return;
            }
        
            var options = new JsonSerializerOptions();
            options.Converters.Add(new GeoJsonConverterFactory());
            var exampleFeatureString = JsonSerializer.Serialize(
                new LineString(new[]
                {
                    new Coordinate(1, 2),
                    new Coordinate(3, 4),
                })
                , options);
            schema.Example = new OpenApiString(exampleFeatureString);
            schema.Default = new OpenApiString(exampleFeatureString);
        }
    }
}