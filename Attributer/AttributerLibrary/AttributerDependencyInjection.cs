using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AttributerLibrary
{
    public static class AttributerDependencyInjection
    {
        public static IServiceCollection AddScopedWithAttributer<IService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Service>(this IServiceCollection services) where IService : class
        {
            return services.AddScoped(sp => AttributerPatcher<IService, Service>.Get(sp));
        }

        public static IServiceCollection AddTransientWithAttributer<IService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Service>(this IServiceCollection services) where IService : class
        {
            return services.AddTransient(sp => AttributerPatcher<IService, Service>.Get(sp));
        }
        public static IServiceCollection AddSingletonWithAttributer<IService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Service>(this IServiceCollection services) where IService : class
        {
            return services.AddSingleton(sp => AttributerPatcher<IService, Service>.Get(sp));
        }
    }
}
