using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MASA.EShop.Api.Caller
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCallerService(this IServiceCollection services)
        {
            services.AddHttpClient();
            var callerServices = Assembly.GetCallingAssembly().GetTypes()
                                .Where(a => a.IsAssignableTo(typeof(ServiceCaller)));

            foreach (var callerService in callerServices)
            {
                services.AddScoped(callerService);
            }

            return services;
        }
    }
}
