using System;

namespace Verba.Abstractions.Application.Modules;

/// <summary>
/// Marker assembly as module
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class ModuleAttribute : Attribute
{
    public ModuleAttribute(Type moduleType = null)
    {         
        ModuleType = moduleType;
    }

    public Type ModuleType { get; }
}