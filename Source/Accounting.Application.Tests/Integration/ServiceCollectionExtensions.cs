using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Tests.Integration
{
    public static class ServiceCollectionExtensions
    {
        public static void RemoveService<T>(this IServiceCollection services)
        {
            var service = services.Single(
                d => d.ServiceType == typeof(T));

            services.Remove(service);
        }

        public static void RemoveService<S, T>(this IServiceCollection services)
        {
            var service = services.Single(
                d => d.ServiceType == typeof(S) && d.ImplementationType == typeof(T));

            services.Remove(service);
        }

        public static void RemoveInstances<T>(this IServiceCollection services)
        {
            var serviceList = services.Where(
                d => d.ServiceType == typeof(T)
                    && d.ImplementationInstance != null
                ).ToList();

            if (serviceList.Count == 0)
                throw new ArgumentException("Expected at least one matching service.");

            serviceList.ForEach(s => services.Remove(s));
        }
    }
}
