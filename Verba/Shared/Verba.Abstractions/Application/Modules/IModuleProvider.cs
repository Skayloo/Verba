using System.Collections.Generic;
using System.Reflection;

namespace Verba.Abstractions.Application.Modules;

/// <summary>
/// Module provider
/// </summary>
public interface IModuleProvider
{
    IEnumerable<Assembly> ApplicationPartAssemblies { get; }
}