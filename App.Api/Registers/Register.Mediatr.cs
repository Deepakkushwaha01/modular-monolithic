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
            IEnumerable<Assembly> assemblies = GetApplicationAssemblies();

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

        private static IEnumerable<Assembly> GetApplicationAssemblies()
        {
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            Queue<Assembly> pending = new(AppDomain.CurrentDomain.GetAssemblies());
            if (entryAssembly is not null)
                pending.Enqueue(entryAssembly);

            HashSet<string> visited = new(StringComparer.OrdinalIgnoreCase);
            List<Assembly> assemblies = [];

            while (pending.TryDequeue(out Assembly? assembly))
            {
                if (assembly.IsDynamic || !visited.Add(assembly.FullName ?? assembly.GetName().Name!))
                    continue;

                string? name = assembly.GetName().Name;
                if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("App.", StringComparison.Ordinal))
                    continue;

                assemblies.Add(assembly);

                foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
                {
                    if (reference.Name?.StartsWith("App.", StringComparison.Ordinal) != true)
                        continue;

                    try
                    {
                        pending.Enqueue(Assembly.Load(reference));
                    }
                    catch (FileNotFoundException)
                    {
                        // Ignore optional application references that are not deployed.
                    }
                }
            }

            return assemblies;
        }
    }
}
