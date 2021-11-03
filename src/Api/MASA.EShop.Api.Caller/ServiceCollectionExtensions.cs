namespace MASA.EShop.Api.Caller;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCallerService(this IServiceCollection services)
    {
        services.AddHttpClient();
        var callerServiceTypes = Assembly.GetCallingAssembly().GetTypes()
                            .Where(a => a.IsAssignableTo(typeof(ServiceCaller)));

        foreach (var callerServiceType in callerServiceTypes)
        {
            services.AddScoped(callerServiceType);
        }

        return services;
    }
}

