using PuzzlesConsole.Day1;
using PuzzlesConsole.Day2;

Console.WriteLine("Select the puzzle by day. Press 'q' to exit");

var solutionTask = Console.ReadLine() switch
{
   "q" or "" => Exit(),
   "1" => Day1Solution.RunAsync(),
   "2" => Day2Solution.RunAsync(),
    _ => NotImplementedPuzzle()
};

await solutionTask;

static ValueTask NotImplementedPuzzle()
{
    Console.WriteLine("This day is not implemented yet :(");
    return ValueTask.CompletedTask;
}

static ValueTask Exit()
{
    Console.WriteLine("Bye ;)");
    return ValueTask.CompletedTask;
}