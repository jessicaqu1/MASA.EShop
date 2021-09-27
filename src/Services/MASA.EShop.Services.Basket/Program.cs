var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddLazyWebApplication(builder)
    .AddScoped<IBasketRepository, BasketRepository>()
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
      options.UseEventBus(builder.Services, AppDomain.CurrentDomain.GetAssemblies())
             .UseUow<IntegrationEventLogContext>(builder.Services, dbOptions => dbOptions.UseSqlServer("Data Source=localhost;Initial Catalog=IntegrationEventLog;User ID=sa;Password=sa"));
    })
    .AddServices();

app.Run();

