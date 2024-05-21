using PlanningProblemSolver.encoder;

namespace PlanningProblemSolver.solver;

/// <summary>
/// Interface for a solver that can solve a bounded planning problem in one go.
/// </summary>
public interface ISolver
{
    /// <summary>
    /// Solves the given bounded planning problem by trying up to <paramref name="maxN"/> steps.
    /// Provides a model if the problem is solvable.
    /// </summary>
    /// <param name="boundedPlanningProblem">The problem to solve.</param>
    /// <param name="model">Out parameter that is populated with a model if this method returns true. Otherwise, this is undefined (usually null).</param>
    /// <param name="maxN">The maximum numbers of steps to try before declaring the problem as unsolvable.</param>
    /// <returns>true if a solution was found in <paramref name="maxN"/> steps, false otherwise.</returns>
    bool Solve(BoundedPlanningProblem boundedPlanningProblem, out string? model, int maxN = 100);
}