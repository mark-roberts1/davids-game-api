using Microsoft.Extensions.DependencyInjection;

namespace Davids.Game.DependencyInjection;

public class ServiceAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Transient) : Attribute
{
    public ServiceLifetime Lifetime => lifetime;
    public Type Type => type;
}

public class ServiceAttribute<T>(ServiceLifetime lifetime = ServiceLifetime.Transient) : ServiceAttribute(typeof(T), lifetime)
{

}
