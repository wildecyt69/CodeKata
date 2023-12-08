using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceCollectionExtensions
    {
        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types) =>
            types.ToList().Select(t => new ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types,
            params Type[] interfaces)
        {
            // optimisation - if we have 3 or more interfaces, then use a dictionary
            if (interfaces.Length >= 3)
            {
                var lookup = interfaces.ToDictionary(x => x, _ => true);
                return
                    types.ToList().Select(
                        t =>
                            new ServiceTypeAndImplementationTypePair(
                                t.GetInterfaces().Where(iface => lookup.ContainsKey(iface)).ToList(), t));
            }

            return
                types.ToList().Select(
                    t =>
                        new ServiceTypeAndImplementationTypePair(
                            t.GetInterfaces().Where(iface => interfaces.Contains(iface)).ToList(), t));
        }

        public static IEnumerable<ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
        {
            return types.Select(t => new ServiceTypeAndImplementationTypePair(new List<Type> { t }, t));
        }

        public static IEnumerable<Type> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .ExceptionSafeGetTypes()
                .Select(t => t.GetTypeInfo())
                .Where(t => !t.IsAbstract && t.IsClass)
                .Where(t => t.DeclaredConstructors.Any(c => !c.IsStatic && c.IsPublic))
                .Select(t => t.AsType());
        }

        public static IEnumerable<Type> DoesNotInherit(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(x => !baseType.IsAssignableFrom(x));
        }

        public static IEnumerable<Type> DoesNotInherit<TBase>(this IEnumerable<Type> types)
            where TBase : Attribute
        {
            return types.DoesNotInherit(typeof(TBase));
        }

        public static IEnumerable<Type> EndingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.EndsWith(endingWith));
        }

        public static IEnumerable<Type> ExceptionSafeGetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                if (Debugger.IsAttached) Debugger.Break();

                return Array.Empty<Type>();
            }
        }

        public static IEnumerable<Type> Inherits(this IEnumerable<Type> types, Type baseType)
        {
            return types.Where(baseType.IsAssignableFrom);
        }

        public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types)
        {
            return types.Inherits(typeof(TBase));
        }

        public static IServiceCollection RegisterAs(this IServiceCollection services,
            IEnumerable<ServiceTypeAndImplementationTypePair> pairs, ServiceLifetime lifetime)
        {
            foreach (var pair in pairs)
            {
                if (pair.ServiceTypes.Count == 0) continue;

                foreach (var serviceType in pair.ServiceTypes)
                    services.Add(new ServiceDescriptor(serviceType, pair.ImplementationType, lifetime));
            }

            return services;
        }

        public static IServiceCollection RegisterAsScoped(this IServiceCollection services,
            IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            return services.RegisterAs(pairs, ServiceLifetime.Scoped);
        }

        public static IServiceCollection RegisterAsSingleton(this IServiceCollection services,
            IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            return services.RegisterAs(pairs, ServiceLifetime.Singleton);
        }

        public static IServiceCollection RegisterAsTransient(this IServiceCollection services,
            IEnumerable<ServiceTypeAndImplementationTypePair> pairs)
        {
            return services.RegisterAs(pairs, ServiceLifetime.Transient);
        }

        public static IEnumerable<Type> StartingWith(this IEnumerable<Type> types, string endingWith)
        {
            return types.Where(x => x.Name.StartsWith(endingWith));
        }

        public class ServiceTypeAndImplementationTypePair
        {
            public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
            {
                ImplementationType = implementationType;
                ServiceTypes = serviceTypes;
            }

            public Type ImplementationType { get; }
            public List<Type> ServiceTypes { get; }
        }
    }
}