namespace Puzzles.Console.Day1;

public static class Day1Solution
{
    public record Bag(int TotalCalories);

    public static async ValueTask<int> RunPart1Async()
    {
        var bag = await ParseInput()
            .OrderByDescending(x => x.TotalCalories)
            .Take(1)
            .FirstAsync();

        return bag.TotalCalories;
    }

    public static ValueTask<int> RunPart2Async()
    {
        return ParseInput()
            .OrderByDescending(x => x.TotalCalories)
            .Take(3)
            .SumAsync(x => x.TotalCalories);
    }

    public static async IAsyncEnumerable<Bag> ParseInput()
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