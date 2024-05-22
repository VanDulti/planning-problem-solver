namespace PlanningProblemSolver.Encoder;

/// <summary>
/// Encodes a planning problem into a logic encoding that can be solved by a SAT solver.
/// </summary>
public class LogicEncoder
{
    /// <summary>
    /// The planning problem to encode.
    /// </summary>
    public required BoundedPlanningProblem Problem { get; init; }

    /// <summary>
    /// Encodes the planning problem into a logic encoding that can be solved by a SAT solver.
    /// </summary>
    /// <param name="n">The number of steps to encode the problem for.</param>
    /// <returns>A string representation of the planning problem as a logic encoding.</returns>
    public string Encode(int n)
    {
        var initialStateEncoding = Problem.InitialState.ToCnf(0);
        var goalStateEncoding = Problem.GoalState.ToCnf(n);

        List<string> stepsEncoded = [];
        for (var i = 0; i < n; i++)
        {
            stepsEncoded.Add(EncodeStep(i));
        }

        var stepsJoined = string.Join(" & \n", stepsEncoded);

        return $"""
                % initial/goal state
                {initialStateEncoding} &
                {goalStateEncoding} {(stepsJoined.Length > 0 ? "&" : "")}

                % steps

                {stepsJoined}
                """;
    }

    private string EncodeStep(int step)
    {
        var actions = Problem.Actions.Keys.Select(x => EncodeAction(x, step));
        var actionsJoined = string.Join(" &\n", actions);

        var atMostOneActionPerStepEncoding = EncodeAtMostOneActionPerStep(step);
        var atMostOneActionPerStepJoined = string.Join(" &\n", atMostOneActionPerStepEncoding);

        var atLeastOneActionPerStepEncoding = Problem.Actions.Keys.Select(x => $"{x}_{step}");
        var atLeastOneActionPerStepJoined = string.Join(" | ", atLeastOneActionPerStepEncoding);

        var frameAxioms = EncodeFrameAxioms(step);
        var frameAxiomsJoined = string.Join(" &\n", frameAxioms);

        return $"""
                %%%%%%%%%%%% step {step} %%%%%%%%%%%%

                % actions

                {actionsJoined} &

                % at most one action per step

                {atMostOneActionPerStepJoined} &

                % at least one action per step

                ({atLeastOneActionPerStepJoined}) &

                % frame axioms

                {frameAxiomsJoined}

                """;
    }

    private IEnumerable<string> EncodeFrameAxioms(int step)
    {
        Dictionary<(string, bool?, bool?), List<string>> modifiedStateVariables = new();
        foreach (var (name, (from, to)) in Problem.Actions)
        {
            var stateVariables = from.StateVariables
                .Select(before => (variable: before.Key, before: before.Value, after: to.StateVariables[before.Key]));

            foreach (var (variableName, before, after) in stateVariables)
            {
                if (before is null || after is null || before == after)
                {
                    continue;
                }

                modifiedStateVariables.TryAdd((variableName, before, after), []);
                modifiedStateVariables[(variableName, before, after)].Add(name);
            }
        }

        return modifiedStateVariables.Select(modifiedStateVariable =>
        {
            var (variableName, before, after) = modifiedStateVariable.Key;
            var actions = modifiedStateVariable.Value.Select(action => $"{action}_{step}");
            var actionsJoined = string.Join(" | ", actions);
            return
                $"(({IState.StateVariableToLiteral(variableName, before, step)} & {IState.StateVariableToLiteral(variableName, after, step + 1)}) -> ({actionsJoined}))";
        });
    }

    private IEnumerable<string> EncodeAtMostOneActionPerStep(int step)
    {
        var actionNames = Problem.Actions.Keys;
        var disjunctions = new List<string>();
        foreach (var i in actionNames)
        {
            foreach (var j in actionNames)
            {
                if (i == j)
                {
                    continue;
                }

                disjunctions.Add($"(-{i}_{step} | -{j}_{step})");
            }
        }

        return disjunctions;
    }

    private string EncodeAction(string name, int step)
    {
        var (pre, post) = Problem.Actions[name];
        var preEncoding = pre.ToCnf(step);
        var postEncoding = post.ToCnf(step + 1);
        return $"({name}_{step} -> ({preEncoding} & {postEncoding}))";
    }
}