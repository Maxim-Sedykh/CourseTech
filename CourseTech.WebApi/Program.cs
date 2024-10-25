using CourseTech.Application.Converters;
using CourseTech.Application.DependencyInjection;
using CourseTech.DAL.DependencyInjection;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Settings;
using CourseTech.WebApi;
using CourseTech.WebApi.Middlewares;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.UseHttpClientMetrics();

builder.Services.AddControllersAndJsonConvertors();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseTech Swagger v1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "CourseTech Swagger v2.0");
        c.RoutePrefix = string.Empty;
    });
    //app.ApplyMigrations();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseMetricServer();
app.UseHttpMetrics();

app.MapGet("/random-number", () => Results.Ok(Random.Shared.Next(0, 10)));

app.UseAuthentication();
app.UseAuthorization();

app.MapMetrics();
app.MapControllers();

app.Run();
