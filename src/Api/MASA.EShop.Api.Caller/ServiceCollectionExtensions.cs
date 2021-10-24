using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MASA.EShop.Api.Caller
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCallerService(this IServiceCollection services, Action<CallerServiceOptions> callerOptions)
        {
            var callerServices = Assembly.GetCallingAssembly().GetTypes()
                                .Where(a => a.IsAssignableTo(typeof(ICaller)));

            services.Configure(callerOptions);

            foreach (var callerService in callerServices)
            {
                if (callerService.BaseType == typeof(HttpClientCaller))
                {

                }
                else if (callerService.BaseType == typeof(DaprClientCaller))
                {

                }
                services.AddSingleton(callerService);
            }

            return services;
        }
    }
}
