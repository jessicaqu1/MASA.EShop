
using MASA.BuildingBlocks.Dispatcher.IntegrationEvents.Logs;
using MASA.Contrib.Data.Uow.EF;
using MASA.Contrib.Dispatcher.Events;
using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
using MASA.EShop.Services.Basket.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

