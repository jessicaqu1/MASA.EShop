using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Reflection;

namespace MASA.EShop.Api.Caller
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCallerService(this IServiceCollection services, Action<CallerServiceOptions>? callerOptions = null)
        {
            var callerServices = Assembly.GetCallingAssembly().GetTypes()
                                .Where(a => a.IsAssignableTo(typeof(ICaller)));

            var callerServiceOptions = new CallerServiceOptions();
            callerOptions?.Invoke(callerServiceOptions);

            foreach (var callerService in callerServices)
            {
                if (callerService.BaseType == typeof(HttpClientCaller))
                {
                    (bool ok, string baseAddress) = callerServiceOptions.GetBaseAddress(callerService.Name);
                    if (ok)
                    {
                        services.AddHttpClient(callerService.Name, client => client.BaseAddress = new Uri(baseAddress));
                    }
                    else
                    {
                        services.AddHttpClient(callerService.Name);
                    }
                    services.Configure<HttpClientFactoryOptions>(callerService.Name, options =>
                    {
                        options.HttpMessageHandlerBuilderActions.Add(b => b.AdditionalHandlers.Add());
                    });
                }
                else if (callerService.BaseType == typeof(DaprClientCaller))
                {

                }
            }

            return services;
        }
    }
}
