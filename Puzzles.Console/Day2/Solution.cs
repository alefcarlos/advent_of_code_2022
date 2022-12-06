namespace PuzzlesConsole.Day2;

public static class Day2Solution
{
    public record RoundResult(int Opponent1Score, int Opponent2Score);

    public record Round(EShape Opponent1Move, EShape Opponent2Move)
    {
        public ERoundWinner GetWinner()
        {
            return (Opponent1Move, Opponent2Move) switch
            {
                //Opponent1 win
                (EShape.Rock, EShape.Scissors) => ERoundWinner.Opponent1,
                (EShape.Scissors, EShape.Paper) => ERoundWinner.Opponent1,
                (EShape.Paper, EShape.Rock) => ERoundWinner.Opponent1,

                //Opponent2 win
                (EShape.Rock, EShape.Paper) => ERoundWinner.Opponent2,
                (EShape.Scissors, EShape.Rock) => ERoundWinner.Opponent2,
                (EShape.Paper, EShape.Scissors) => ERoundWinner.Opponent2,

                //Draw
                (EShape.Paper, EShape.Paper) => ERoundWinner.Draw,
                (EShape.Rock, EShape.Rock) => ERoundWinner.Draw,
                (EShape.Scissors, EShape.Scissors) => ERoundWinner.Draw,
            };
        }

        public RoundResult GetRoundScore()
        {
            const int winScore = 6;
            const int drawScore = 3;

            var winner = GetWinner();

            return winner switch
            {
                ERoundWinner.Draw => new RoundResult(drawScore + Opponent1Move.GetScore(), drawScore + Opponent2Move.GetScore()),
                ERoundWinner.Opponent1 => new RoundResult(winScore + Opponent1Move.GetScore(), Opponent2Move.GetScore()),
                ERoundWinner.Opponent2 => new RoundResult(Opponent1Move.GetScore(), winScore + Opponent2Move.GetScore()),
            };
        }
    }

    public enum EShape
    {
        Rock,
        Paper,
        Scissors
    }

    public static int GetScore(this EShape shape) => shape switch
    {
        EShape.Rock => 1,
        EShape.Paper => 2,
        EShape.Scissors => 3,
        _ => throw new NotImplementedException()
    };

    public enum ERoundWinner
    {
        Opponent1,
        Opponent2,
        Draw
    }

    public enum EOpponent2Instruction
    {
        MustLose,
        MustWin,
        MustDraw
    }

    public static async ValueTask RunAsync()
    {
        await RunPart1Async();
        await RunPart2Async();
    }

    public static async ValueTask RunPart1Async()
    {
        Console.WriteLine("Running Day2 puzzle");
        Console.WriteLine();

        var meScore = await ParseInput()
                                .SumAsync(x => x.GetRoundScore().Opponent2Score);

        Console.WriteLine($"What would your total score be if everything goes exactly according to your strategy guide? {meScore}");
    }

    public static async ValueTask RunPart2Async()
    {
        Console.WriteLine("Running Day2 puzzle");
        Console.WriteLine();

        var meScore = await ParseInput2()
                                .SumAsync(x => x.GetRoundScore().Opponent2Score);

        Console.WriteLine($"Following the Elf's instructions for the second column, what would your total score be if everything goes exactly according to your strategy guide? {meScore}");
    }

    internal static async IAsyncEnumerable<Round> ParseInput2()
    {
        await foreach (var line in File.ReadLinesAsync(@"Day2\input.txt"))
        {
            var options = line.Split(" ");

            var opponent1Move = options[0] switch
            {
                "A" => EShape.Rock,
                "B" => EShape.Paper,
                "C" => EShape.Scissors
            };

            var opponent2Instruction = options[1] switch
            {
                "X" => EOpponent2Instruction.MustLose,
                "Y" => EOpponent2Instruction.MustDraw,
                "Z" => EOpponent2Instruction.MustWin,
            };

            var opponent2Move = (opponent2Instruction, opponent1Move) switch
            {
                (EOpponent2Instruction.MustDraw, _) => opponent1Move,

                (EOpponent2Instruction.MustLose, EShape.Rock) =>EShape.Scissors,
                (EOpponent2Instruction.MustLose, EShape.Paper) =>EShape.Rock,
                (EOpponent2Instruction.MustLose, EShape.Scissors) =>EShape.Paper,

                (EOpponent2Instruction.MustWin, EShape.Rock) => EShape.Paper,
                (EOpponent2Instruction.MustWin, EShape.Paper) => EShape.Scissors,
                (EOpponent2Instruction.MustWin, EShape.Scissors) => EShape.Rock,
            };



            yield return new Round(opponent1Move, opponent2Move);
        }
    }

    internal static async IAsyncEnumerable<Round> ParseInput()
    {
        await foreach (var line in File.ReadLinesAsync(@"Day2\input.txt"))
        {
            var options = line.Split(" ");

            var opponent1Move = options[0] switch
            {
                "A" => EShape.Rock,
                "B" => EShape.Paper,
                "C" => EShape.Scissors
            };

            var opponent2Move = options[1] switch
            {
                "X" => EShape.Rock,
                "Y" => EShape.Paper,
                "Z" => EShape.Scissors,
            };


            yield return new Round(opponent1Move, opponent2Move);
        }
    }
}
