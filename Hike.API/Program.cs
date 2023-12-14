using System.Text.Json.Serialization;
using Hike.API.Filter;
using Hike.Data.Extensions;
using Hike.UseCases.Extensions;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.IO.Converters;

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

builder.Services.ConfigureSwaggerGen(options => { options.SchemaFilter<LineStringSchemaFilter>(); });

//Dependency Injection of Hike.Domain + Hike.Data
builder.Services.AddLogic().AddData(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

var authority = "https://securetoken.google.com/" + builder.Configuration["FireBase:AppId"];

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = authority;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authority,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["FireBase:AppId"],
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

app.Run();