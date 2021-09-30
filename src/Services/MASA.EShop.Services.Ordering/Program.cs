var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddLazyWebApplication(builder)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA eShop - Ordering HTTP API",
            Version = "v1",
            Description = "The Ordering Service HTTP API"
        });
    }).AddSwaggerGenNewtonsoftSupport()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
        options.UseEventBus(AppDomain.CurrentDomain.GetAssemblies())
             .UseUow<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=localhost;Initial Catalog=IntegrationEventLog;User ID=sa;Password=sa"))
             .UseEventLog<OrderingContext>();
    })
    .AddServices();

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA eShop Service HTTP API v1");
});

app.Run();
