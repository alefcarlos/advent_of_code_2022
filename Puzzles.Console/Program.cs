using PuzzlesConsole.Day1;

Console.WriteLine("Select the puzzle by day. Press 'q' to exit");

var solutionTask = Console.ReadLine() switch
{
   "q" or "" => Exit(),
   "1" => Day1Solution.RunAsync(),
};

await solutionTask;

static ValueTask Exit()
{
    Console.WriteLine("Bye ;)");
    return ValueTask.CompletedTask;
}