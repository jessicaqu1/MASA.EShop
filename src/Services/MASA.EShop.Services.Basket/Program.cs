var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddLazyWebApplication(builder)
    .AddScoped<IBasketRepository,BasketRepository>()
    .AddUow<IntegrationEventLogContext>(options => options.UseSqlServer(""))
    .AddDaprEventBus<IntegrationEventLogService>(options=> {
        options.UseEventBus(builder.Services,AppDomain.CurrentDomain.GetAssemblies())
        .UseEFEventLog(builder.Services);
    })
    .AddServices();

app.Run();

