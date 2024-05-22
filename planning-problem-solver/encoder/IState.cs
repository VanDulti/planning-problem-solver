namespace PlanningProblemSolver.Encoder;

/**
 * Represents a state in the planning problem.
 * A state is a mapping of state variables to their values.
 */
public interface IState
{
    public Dictionary<string, bool?> StateVariables { get; }

    /// <summary>
    /// Converts the state to a CNF formula at a given step using & as the logical AND operator.
    /// Appends _<paramref name="step"/> to the variable name to indicate the step.
    /// </summary>
    /// <param name="step">The step to append to each state variable to distinct between steps.</param>
    /// <returns>A string representation of this state as a cnf formula.</returns>
    public string ToCnf(int step)
    {
        var literals = StateVariables
            .Select(stateVariable => StateVariableToLiteral(stateVariable.Key, stateVariable.Value, step))
            .Where(literal => literal is not null);
        var cnf = string.Join(" & ", literals);
        return $"({cnf})";
    }

    /// <summary>
    /// Converts a state variable to a literal at a given step.
    /// </summary>
    /// <param name="variable">The name of the state variable.</param>
    /// <param name="value">The value of the state variable.</param>
    /// <param name="step">The step to append to the variable name to distinct between steps.</param>
    /// <returns>A string representation of the state variable as a literal at the given step.</returns>
    public static string? StateVariableToLiteral(string variable, bool? value, int step) => value switch
    {
        true => $"{variable}_{step}",
        false => $"-{variable}_{step}",
        null => null
    };
}