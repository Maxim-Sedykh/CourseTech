using CourseTech.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder);

var app = builder.Build();

app.ConfigureMiddlewares();

app.Run();
