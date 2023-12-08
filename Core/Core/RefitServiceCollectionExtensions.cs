using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;

namespace Core
{
    public static class RefitServiceCollectionExtensions
    {
        public static IServiceCollection AddRefitClient<THandler, TApi>(this IServiceCollection services, string baseUrlAddress) where THandler : DelegatingHandler where TApi : class
        {
            services.AddRefitClient(typeof(TApi), baseUrlAddress)
                .AddHttpMessageHandler<THandler>();

            return services;
        }

        public static IServiceCollection AddRefitClient<TApi>(this IServiceCollection services, string baseUrlAddress) where TApi : class
        {
            AddRefitClient(services, typeof(TApi), baseUrlAddress);

            return services;
        }

        public static IHttpClientBuilder AddRefitClient(this IServiceCollection services, Type serviceType, string baseUrlAddress)
        {
            IHttpClientBuilder httpBuilder = services.AddRefitClient(serviceType, provider => new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                })
            })
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrlAddress))
                .AddPolicyHandler(GetRetryPolicy());

            return httpBuilder;
        }

        public static IServiceCollection AddRefitClients<THandler>(this IServiceCollection services, IEnumerable<ServiceCollectionExtensions.ServiceTypeAndImplementationTypePair> pairs,
            string baseUrlAddress) where THandler : DelegatingHandler
        {
            foreach (ServiceCollectionExtensions.ServiceTypeAndImplementationTypePair pair in pairs.Where(x => x.ServiceTypes.Count > 0))
            {
                foreach (Type serviceType in pair.ServiceTypes)
                {
                    services.AddRefitClient(serviceType, baseUrlAddress)
                        .AddHttpMessageHandler<THandler>();
                }
            }

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));
        }
    }
}