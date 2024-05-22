using PlanningProblemSolver.Encoder;

namespace PlanningProblemSolver.Example;

/// <summary>
/// A custom IState implementation for the sample problem that enforces a specific set of possible state variables.
/// </summary>
public class State : IState
{
    public bool? IsLinz { get; init; }
    public bool? IsVienna { get; init; }
    public bool? IsSalzburg { get; init; }
    public bool? IsInnsbruck { get; init; }
    public bool? HasDeployedG1 { get; init; }
    public bool? HasDeployedG2 { get; init; }
    public bool? HasG1 { get; init; }
    public bool? HasG2 { get; init; }
    
    public Dictionary<string, bool?> StateVariables => new()
    {
        { "IsLinz", IsLinz },
        { "IsVienna", IsVienna },
        { "IsSalzburg", IsSalzburg },
        { "IsInnsbruck", IsInnsbruck },
        { "HasDeployedG1", HasDeployedG1 },
        { "HasDeployedG2", HasDeployedG2 },
        { "HasG1", HasG1 },
        { "HasG2", HasG2 }
    };
}