using System.Reflection;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

namespace App.API.Registers
{
    public static partial class Register
    {
        public static IServiceCollection AddSharedMediatrServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            RegisterHandlers(services);

            return services;
        }

        private static void RegisterHandlers(IServiceCollection services)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
                {
                    if (a.IsDynamic)
                        return false;

                    string? name = a.GetName().Name;
                    return !string.IsNullOrWhiteSpace(name) &&
                        (name.StartsWith("App.") || name.StartsWith("App.Core"));
                });

            IEnumerable<Type> handlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        (
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                        )
                    )
                );

            foreach (Type handlerType in handlerTypes)
            {
                foreach (Type @interface in handlerType.GetInterfaces())
                {
                    if (!@interface.IsGenericType) continue;

                    Type definition = @interface.GetGenericTypeDefinition();

                    if (definition == typeof(ICommandHandler<,>) ||
                        definition == typeof(IQueryHandler<,>))
                    {
                        services.AddScoped(@interface, handlerType);
                    }
                }
            }
        }
    }
}
