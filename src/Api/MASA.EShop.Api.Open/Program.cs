var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration);
builder.Services.AddCallerService();

var app = builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA EShop - HTTP API",
            Version = "v1",
            Description = "The EShop Service HTTP API"
        });
    })
    .AddServices(builder);

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA EShop Service HTTP API v1");
});

app.Run();
