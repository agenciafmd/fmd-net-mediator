using System.Reflection;

namespace Fmd.Net.Mediator.Extensions;

public class MediatorAssemblyRegistrar
{
    private readonly List<Assembly> _assemblies = [];
    public IEnumerable<Assembly> Assemblies => _assemblies;

    public void AddAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
    }

    public void AddAssembly(string assemblyName)
    {
        _assemblies.AddRange(Assembly.Load(assemblyName));
    }
}