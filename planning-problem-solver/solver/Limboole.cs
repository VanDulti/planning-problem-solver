using System.Diagnostics;

namespace PlanningProblemSolver.Solver;

/// <summary>
/// Limboole implementation that uses a local limboole executable to check the satisfiability of a formula.
/// </summary>
public class Limboole : ILimboole
{
    /// <summary>
    /// Path to the limboole executable.
    /// </summary>
    // ReSharper disable once StringLiteralTypo
    public string ExecutablePath { get; init; } = "limboole";

    public bool CheckSatisfiability(string formula, out string model)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = ExecutablePath,
            Arguments = "-s",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.Start();
        process.StandardInput.Write(formula);
        process.StandardInput.Close();
        model = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return model.StartsWith("% SATISFIABLE formula (satisfying assignment follows)");
    }
}