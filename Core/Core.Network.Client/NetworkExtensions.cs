using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Network.Client
{
    public static class NetworkExtensions
    {
        public static IServiceCollection AddApis<T>(this IServiceCollection services, Assembly assembly, string baseUrlAddress) where T : DelegatingHandler
        {
            return services.AddRefitClients<T>(assembly.CreatableTypes().EndingWith("Api").AsInterfaces(), baseUrlAddress);
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            return services.RegisterAs(assembly.CreatableTypes().EndingWith("Handler").AsInterfaces(), lifetime);
        }
    }
}