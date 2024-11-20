
using System.Collections.ObjectModel;
using System.Reflection;

namespace AppointmentScheduler.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class MetadataAttribute(params string[] value) : Attribute
{
    private readonly string[] _value = value;
    public IList<string> Value => new ReadOnlyCollection<string>(_value);
}