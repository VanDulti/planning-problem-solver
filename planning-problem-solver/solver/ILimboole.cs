namespace PlanningProblemSolver.solver;

/// <summary>
/// Offers access to a boolean satisfiability checker.
/// </summary>
public interface ILimboole
{
    /// <summary>
    /// Checks the satisfiability of a given formula and generates a model if the formula is satisfiable.
    /// </summary>
    /// <param name="formula">The limboole syntax encoded formula to be sat-checked.</param>
    /// <param name="model">Output parameter that contains a model if this method returns true. Otherwise, the model is undefined.</param>
    /// <returns>True if the formula is satisfiable, false otherwise.</returns>
    bool CheckSatisfiability(string formula, out string model);
}