using CourseTech.Application.DependencyInjection;
using CourseTech.ChatGptApi.DependencyInjection;
using CourseTech.DAL.DependencyInjection;
using CourseTech.DAL.Extensions;
using CourseTech.Domain.Settings;
using CourseTech.WebApi.Middlewares;
using Prometheus;
using Serilog;

namespace CourseTech.WebApi;

public static class Startup
{
    /// <summary>
    /// Настройка сервисов.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public static void ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
        services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));
        services.Configure<ChatGptSettings>(builder.Configuration.GetSection(nameof(ChatGptSettings)));

        services.AddEndpointsApiExplorer();
        services.UseHttpClientMetrics();

        services.AddControllersAndJsonConvertors();

        services.AddAuthenticationAndAuthorization(builder);
        services.AddSwagger();

        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

        services.AddDataAccessLayer(builder.Configuration);
        services.AddApplication();

        services.AddChatGpt();
    }

    /// <summary>
    /// Настройка middleware.
    /// </summary>
    /// <param name="app"></param>
    public static void ConfigureMiddlewares(this WebApplication app)
    {
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
            app.ApplyMigrations();
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
    }
}
