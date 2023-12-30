using System.Text.Json.Serialization;
using Hike.API.Filter;
using Hike.Data.DbContext;
using Hike.Data.Extensions;
using Hike.UseCases.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using NetTopologySuite.IO.Converters;
using Hike.API.Hubs;

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

builder.Services.AddSignalR();

builder.Services.ConfigureSwaggerGen(options => { options.SchemaFilter<LineStringSchemaFilter>(); });

//Dependency Injection of Hike.Domain + Hike.Data
builder.Services.AddLogic().AddData(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

var authority = "https://securetoken.google.com/" + builder.Configuration["FireBase:ProjectId"];

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = authority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authority,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["FireBase:ProjectId"],
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => { options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MapHub>("/map-hub");

using (IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    ApplicationDbContext? context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    try
    {
        context?.Database.Migrate();
    }
    catch (MySqlException)
    {
    }
}

app.Run();

namespace Hike.API
{
    public class HikeApiProgram
    {

    }
}