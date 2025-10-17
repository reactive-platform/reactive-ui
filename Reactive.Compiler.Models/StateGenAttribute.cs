namespace Reactive.Compiler;

/// <summary>
/// An attribute that is used to specify state generator params.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class StateGenAttribute : Attribute {
    /// <summary>
    /// Defines the state of the compiler feature. Defaults to true.
    /// Use this to override derived settings.
    /// </summary>
    public bool Enabled { get; init; }
    
    /// <summary>
    /// Defines patterns that are used to generate states.
    /// Default value is s{} where {} is the initial state name.
    /// </summary>
    public string[]? Patterns { get; init; }
}