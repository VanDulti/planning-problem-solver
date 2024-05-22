# Planning Problem Solver

This is a simple planning problem solver that encodes a bounded planning problem in propositional logic and
uses https://fmv.jku.at/limboole/ to solve it.

I did this for my Formal Models class at JKU.

![CodeQL]
(https://github.com/VanDulti/planning-problem-solver/actions/workflows/codeql.yml/badge.svg)

![Build]
(https://github.com/VanDulti/planning-problem-solver/actions/workflows/dotnet.yml/badge.svg)

## planning-problem-solver

Offers a simple API for encoding a planning problem as a propositional formula and solving it using Limboole.

First, you need to specify your bounded planning problem.
A bounded planning problem consists of:

- a set of states
- a set of actions
- initial state
- goal state

You only have to specify the initial state, goal state and actions (states are inferred from the actions).

To specify a state you can either use the predefined `SimpleState` class or implement `IState` (for an example
see `example-problem`) yourself.

Usage of `SimpleState` to specify states for a planning problem:

```csharp
IState initialState = new SimpleState
{
    StateVariables = new Dictionary<string, bool?>
    {
        { "stateVariable", true },
        { "anotherVariable", false },
        { "thirdVariable", null }
    }
};
IState goalState = ... // use your imagination

BoundedPlanningProblem problem = new BoundedPlanningProblem
{
    InitialState = initialState,
    GoalState = goalState,
    Actions = new Dictionary<string, (IState, IState)>
    {
        { "moveInitialToGoal", (initialState, goalState) },
        ... // more actions as needed
    }
};
```

To encode the planning problem as a propositional formula use the `LogicEncoder` class:

```csharp
LogicEncoder encoder = new LogicEncoder { Problem = problem };
string formula = encoder.Encode();
```

Then check if the formula is satisfiable and get the model if it is.
To use limboole you need to have it installed on your system and specify the executable name/path when creating
your `Limboole` instance.

```csharp
LimBoole limBoole = new LimBoole { ExecutablePath = "limbooleOSX" };
LimBoole isSatisfiable = LimBoole.CheckSatisfiability(formula, out string model);
if (isSatisfiable)
{
    Console.WriteLine($"No solution found.");
    return;
}
Console.WriteLine($"Solution found. Model: {model}");
```

Alternatively you can use the `Solver` class which wraps the above functionality:

```csharp
BoundedPlanningProblem problem = ... // create your bounded planning problem
Solver solver = new Solver { LimBoole = limBoole };
if (!solver.Solve(problem, out string model, 15))
{
    Console.WriteLine("No solution found. Try increasing the maxN parameter.");
    return;
}
```

For sample usage see `sample-problem`.

## sample-problem

My solution to the assignment. It encodes a simple planning problem as a logical formula and solves it using
Limboole.

