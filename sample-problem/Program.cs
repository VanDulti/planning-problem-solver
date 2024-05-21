using formal_models_exercise_31;
using PlanningProblemSolver.encoder;
using PlanningProblemSolver.solver;

var limBoole = new Limboole { ExecutablePath = "limbooleOSX" };
var solver = new Solver { Limboole = limBoole };

var initialState = new State
{
    IsVienna = false,
    IsLinz = true,
    IsSalzburg = false,
    IsInnsbruck = false,
    HasG1 = false,
    HasG2 = false,
    HasDeployedG1 = false,
    HasDeployedG2 = false
};
var goalState = new State { HasG1 = false, HasG2 = false, HasDeployedG1 = true, HasDeployedG2 = true };

var linz = new State { IsVienna = false, IsLinz = true, IsSalzburg = false, IsInnsbruck = false };
var vienna = new State { IsVienna = true, IsLinz = false, IsSalzburg = false, IsInnsbruck = false };
var salzburg = new State { IsVienna = false, IsLinz = false, IsSalzburg = true, IsInnsbruck = false };
var innsbruck = new State { IsVienna = false, IsLinz = false, IsSalzburg = false, IsInnsbruck = true };

var innsbruckG1NotDeployed = new State
    { IsVienna = false, IsLinz = false, IsSalzburg = false, IsInnsbruck = true, HasG1 = true, HasDeployedG1 = false };
var innsbruckG1Deployed = new State
    { IsVienna = false, IsLinz = false, IsSalzburg = false, IsInnsbruck = true, HasG1 = false, HasDeployedG1 = true };
var salzburgG2NotDeployed = new State
    { IsVienna = false, IsLinz = false, IsSalzburg = true, IsInnsbruck = false, HasG2 = true, HasDeployedG2 = false };
var salzburgG2Deployed = new State
    { IsVienna = false, IsLinz = false, IsSalzburg = true, IsInnsbruck = false, HasG2 = false, HasDeployedG2 = true };

Console.WriteLine("Exercise 31: Bounded Planning Problem");
SolveExercise31();
Console.WriteLine("Exercise 32: Modified Bounded Planning Problem");
SolveExercise32();
return;

void SolveExercise31()
{
    /*
     * # Exercise 31: Bounded Planning Problem
     *
     * ## Task
     * A transport company has a single truck to delivers goods from / to Linz, Vienna, Innsbruck, and Salzburg. Starting point is Linz where good G1 is located that should go to Innsbruck. Good G2 is located in Vienna and should go to Salzburg. From Linz it is possible to go to Vienna or to Salzburg. From Vienna it is possible to go to Salzburg or Linz and from Salzburg it is possible to go to Innsbruck.
     * There is no restriction on the number of goods that can be transported at the same time. Make reasonable assumptions for properties of the problem not described above. Formulate this problem as a planning problem P and solve (P, n) incrementally.
     * What is the smallest value of n for which a solution exists?
     * Hint: It is recommended to write a script to generate the formula. It might also be more convenient
     * to run a local version of Limboole.
     *
     * ## My Assumptions:
     * 1. The truck can carry multiple goods at the same time.
     * 2. Each step of the truck is considered the same length, regardless of the distance between the cities.
     * 3. Items have to be picked up in a separate step (HasG1, HasG2)
     * 4. Items have to be dropped off in a separate step (HasDeployedG1, HasDeployedG2)
     *
     * Otherwise, the problem would get pretty complicated.
     */
    var linzWithoutG1 = new State
        { IsVienna = false, IsLinz = true, IsSalzburg = false, IsInnsbruck = false, HasG1 = false };
    var linzWithG1 = new State
        { IsVienna = false, IsLinz = true, IsSalzburg = false, IsInnsbruck = false, HasG1 = true };
    var viennaWithoutG2 = new State
        { IsVienna = true, IsLinz = false, IsSalzburg = false, IsInnsbruck = false, HasG2 = false };
    var viennaWithG2 = new State
        { IsVienna = true, IsLinz = false, IsSalzburg = false, IsInnsbruck = false, HasG2 = true };

    var actions = new Dictionary<string, (IState, IState)>
    {
        { "linzToVienna", (linz, vienna) },
        { "linzToSalzburg", (linz, salzburg) },
        { "viennaToLinz", (vienna, linz) },
        { "viennaToSalzburg", (vienna, salzburg) },
        { "salzburgToInnsbruck", (salzburg, innsbruck) },
        { "addG1", (linzWithoutG1, linzWithG1) },
        { "addG2", (viennaWithoutG2, viennaWithG2) },
        { "deployG2", (salzburgG2NotDeployed, salzburgG2Deployed) },
        { "deployG1", (innsbruckG1NotDeployed, innsbruckG1Deployed) }
    };

    var problem = new BoundedPlanningProblem
    {
        InitialState = initialState,
        GoalState = goalState,
        Actions = actions
    };

    if (!solver.Solve(problem, out var model, 15))
    {
        Console.WriteLine("No solution found. Try increasing the maxN parameter.");
        return;
    }

    File.WriteAllTextAsync("31-model.boole", model);
}

void SolveExercise32()
{
    /*
     * # Exercise 32: Modified Bounded Planning Problem
     *
     * ## Task
     * Modify your solution from Exercise 31 to only allow the truck to carry one parcel at a time.
     * Use Limboole again. What is the result?
     *
     * ## My Assumptions:
     * * 1. The truck can carry only one good at a time.
     * 2. Each step of the truck is considered the same length, regardless of the distance between the cities.
     * 3. Items have to be picked up in a separate step (HasG1, HasG2)
     * 4. Items have to be dropped off in a separate step (HasDeployedG1, HasDeployedG2)
     *
     * This is of course not possible in the given directed graph, as we can't go back from Salzburg to Linz or from Innsbruck to Salzburg to pick up the second good.
     * Therefore, I had to modify the problem a bit to make it solvable:
     * I added the possibility to go from Salzburg to Linz, Salzburg to Vienna and from Innsbruck to Salzburg

     * Otherwise, the problem would get pretty complicated.
     */
    var linzWithoutG1 = new State
        { IsVienna = false, IsLinz = true, IsSalzburg = false, IsInnsbruck = false, HasG1 = false, HasG2 = false };
    var linzWithG1 = new State
        { IsVienna = false, IsLinz = true, IsSalzburg = false, IsInnsbruck = false, HasG1 = true, HasG2 = false };
    var viennaWithoutG2 = new State
        { IsVienna = true, IsLinz = false, IsSalzburg = false, IsInnsbruck = false, HasG1 = false, HasG2 = false };
    var viennaWithG2 = new State
        { IsVienna = true, IsLinz = false, IsSalzburg = false, IsInnsbruck = false, HasG1 = false, HasG2 = true };

    var actions = new Dictionary<string, (IState, IState)>
    {
        { "linzToVienna", (linz, vienna) },
        { "linzToSalzburg", (linz, salzburg) },
        { "viennaToLinz", (vienna, linz) },
        { "viennaToSalzburg", (vienna, salzburg) },
        { "salzburgToLinz", (salzburg, linz) }, // new
        { "salzburgToVienna", (salzburg, vienna) }, // new
        { "innsbruckToSalzburg", (innsbruck, salzburg) }, // new
        { "salzburgToInnsbruck", (salzburg, innsbruck) },
        { "addG1", (linzWithoutG1, linzWithG1) },
        { "addG2", (viennaWithoutG2, viennaWithG2) },
        { "deployG2", (salzburgG2NotDeployed, salzburgG2Deployed) },
        { "deployG1", (innsbruckG1NotDeployed, innsbruckG1Deployed) }
    };

    var problem = new BoundedPlanningProblem
    {
        InitialState = initialState,
        GoalState = goalState,
        Actions = actions
    };

    if (!solver.Solve(problem, out var model, 15))
    {
        Console.WriteLine("No solution found. Try increasing the maxN parameter.");
        return;
    }

    File.WriteAllTextAsync("32-model.boole", model);
}