using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RMapper.Configurations;
using RMapper.Implementation;
using RMapper.Interfaces;

namespace RMapper.DependencyInjection;

/// <summary>
/// Extensión para inyectar servicio de mapeo.
/// </summary>
public static class MapperServiceExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMapper(Assembly assembly, 
            Action<MapperOptions>? options = null)
        {
            if(options is not null)
                options(new MapperOptions());
        
            var profiles =  assembly.GetTypes().Where(e => e.IsSubclassOf(typeof(Profile)) && !e.IsAbstract).ToList();
            profiles.ForEach(profile => Activator.CreateInstance(profile));
            Profile.ExecuteBuilders();
        
            services.AddSingleton<IMapper, Mapper>();
            return services;
        }
    }
}

/// <summary>
/// Clase para definir opciones
/// </summary>
public sealed class MapperOptions
{
    /// <summary>
    /// Metodo que define la configuración de IgnoreBasicPropertiesFailedCast.
    /// </summary>
    public void IgnoreBasicPropertiesFailedCast() => MapperSettings.IgnoreBasicPropertiesFailedCast = true;
}