using PlanningProblemSolver.Encoder;

namespace PlanningProblemSolver.Solver;

/// <summary>
/// A solver that uses a limboole instance to repeatedly check the satisfiability of a formula with increasing n
/// until a solution is found or a maximum n is reached.
/// </summary>
public class Solver : ISolver
{
    /// <summary>
    /// The limboole instance to use for checking satisfiability.
    /// </summary>
    public required ILimboole Limboole { get; init; }

    /// <summary>
    /// Solve the given bounded planning problem by checking the satisfiability of a formula with increasing n.
    /// For testing purposes, this will write the formula to file for each n and simple log to the console.  
    /// </summary>
    public bool Solve(BoundedPlanningProblem boundedPlanningProblem, out string? model, int maxN = 100)
    {
        model = null;
        var encoder = new LogicEncoder { Problem = boundedPlanningProblem };
        for (var i = 1; i < maxN; i++)
        {
            Console.WriteLine($"n={i}");
            var formula = encoder.Encode(i);
            File.WriteAllText($"formula-{i}.boole", formula);
            var isSatisfiable = Limboole.CheckSatisfiability(formula, out model);
            if (isSatisfiable)
            {
                Console.WriteLine($"Solution found for n = {i}.");
                return true;
            }

            Console.WriteLine($"No solution found for n = {i}.");
        }

        return false;
    }
}