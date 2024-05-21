namespace PlanningProblemSolver.encoder;

/// <summary>
/// Simple representation of a state in a planning problem.
/// </summary>
public class BoundedPlanningProblem
{
    /// <summary>
    /// Initial state of the planning problem.
    /// </summary>
    public required IState InitialState { get; init; }

    /// <summary>
    /// Goal state of the planning problem.
    /// </summary>
    public required IState GoalState { get; init; }

    /// <summary>
    /// Actions that can be taken in the planning problem as a mapping of a unique action name to a tuple of the
    /// preconditions and effects of the action.
    ///
    /// The action key should be a legal variable name in the logic encoding (letters, numbers and underscores should be fine).
    /// </summary>
    public required Dictionary<string, (IState, IState)> Actions { get; init; }
}