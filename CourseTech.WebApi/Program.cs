using CourseTech.Domain.Settings;
using CourseTech.WebApi;
using Serilog;
using CourseTech.Application.DependencyInjection;
using CourseTech.DAL.DependencyInjection;
using CourseTech.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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

app.Run();
