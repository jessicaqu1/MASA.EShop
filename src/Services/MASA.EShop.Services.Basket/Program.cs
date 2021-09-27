var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddLazyWebApplication(builder)
    .AddScoped<IBasketRepository,BasketRepository>()
    .AddUow<IntegrationEventLogContext>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=IntegrationEventLog;User ID=sa;Password=sa"))
    .AddDaprEventBus<IntegrationEventLogService>(options=> {
        options.UseEventBus(builder.Services,AppDomain.CurrentDomain.GetAssemblies());
    })
    .AddServices();

app.Run();

