using CourseTech.DAL.DependencyInjection;
using CourseTech.Application.DependencyInjection;
using CourseTech.Api.Middlewares;
using CourseTech.Domain.Settings;
using Serilog;

namespace CourseTech.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

        builder.Services.AddControllers();

        builder.Services.AddAuthenticationAndAuthorization(builder);
        builder.Services.AddSwagger();

        // To Do поменять на что-то интереснее
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddDataAccessLayer(builder.Configuration);
        builder.Services.AddApplication();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseTech Swagger v1.0");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "CourseTech Swagger v2.0");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        //app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
