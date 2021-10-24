using Microsoft.Extensions.DependencyInjection;

namespace MASA.EShop.Api.Caller
{
    internal class CallerHttpClientBuilder : IHttpClientBuilder
    {
        public CallerHttpClientBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}
