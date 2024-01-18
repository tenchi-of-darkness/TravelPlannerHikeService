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
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

builder.Services.AddSignalR(x => x.SupportedProtocols = new List<string>{"json"});

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
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/api/map-hub")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(x =>
{
    x.MapControllers();

    x.MapHub<MapHub>("/api/map-hub");
});

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