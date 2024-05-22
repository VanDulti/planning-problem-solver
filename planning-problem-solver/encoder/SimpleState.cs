namespace PlanningProblemSolver.Encoder;

/// <summary>
/// A simple state that holds any number of dictionary state variables.
/// If you want to force a specific set of state variables including names and optional ones,
/// implement <see cref="IState"/> yourself.
/// </summary>
public class SimpleState : IState
{
    /// <summary>
    /// The state variables of this state.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required Dictionary<string, bool?> StateVariables { get; init; }
}