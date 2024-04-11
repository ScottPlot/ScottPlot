using System.ComponentModel;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Allows init-only setters in older .NET versions
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit { }

/// <summary>
/// Allows required members in older .NET versions
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class RequiredMemberAttribute : Attribute { }

/// <summary>
/// Allows required members in older .NET versions
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class CompilerFeatureRequiredAttribute : Attribute
{
    public CompilerFeatureRequiredAttribute(string _) { }
}
