namespace PuzzlesConsole.Day1;

public static class Day1Solution
{
    public record Bag(int TotalCalories);

    public static async ValueTask RunAsync()
    {
        Console.WriteLine("Running Day1 puzzle");
        Console.WriteLine();

        var result = await RunPart1Async();
        Console.WriteLine($"Find the Elf carrying the most Calories.\nHow many total Calories is that Elf carrying? {result}");

        Console.WriteLine();
        result = await RunPart2Async();
        Console.WriteLine($"Find the top three Elves carrying the most Calories.\nHow many Calories are those Elves carrying in total? {result}");
    }

    internal static async ValueTask<int> RunPart1Async()
    {
        var bag = await ParseInput()
            .OrderByDescending(x => x.TotalCalories)
            .FirstAsync();

        return bag.TotalCalories;
    }

    internal static ValueTask<int> RunPart2Async()
    {
        return ParseInput()
            .OrderByDescending(x => x.TotalCalories)
            .Take(3)
            .SumAsync(x => x.TotalCalories);
    }

    internal static async IAsyncEnumerable<Bag> ParseInput()
    {
        int caloriesSum = 0;
        await foreach (var line in File.ReadLinesAsync(@"Day1\input.txt"))
        {
            if (line is "")
            {
                yield return new Bag(caloriesSum);
                caloriesSum = 0;
            }
            else
            {
                caloriesSum += int.Parse(line);
            }
        }
    }
}