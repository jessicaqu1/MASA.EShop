var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services
    .AddLazyWebApplication(builder)
    //todo 
    .AddScoped<IBasketRepository,BasketRepository>()
    .AddDbContext<IntegrationEventLogContext>(options => options.UseSqlServer(""))
    .AddUow<BasketDbContext>(options => options.UseSqlServer(""))
    .AddEventBus(AppDomain.CurrentDomain.GetAssemblies())
    .AddDaprEventBus<IntegrationEventLogService>()
    .AddServices();

var app = builder.Services.BuildServiceProvider().GetRequiredService<WebApplication>();

app.Run();

