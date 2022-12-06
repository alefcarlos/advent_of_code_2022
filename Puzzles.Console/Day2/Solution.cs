using System.Security.Cryptography.X509Certificates;
using static PuzzlesConsole.Day1.Day1Solution;

namespace PuzzlesConsole.Day2;

public static class Day2Solution
{
    public static int GetScore(this EShape shape) => shape switch
    {
        EShape.Rock => 1,
        EShape.Paper => 2,
        EShape.Scissors => 3,
        _ => throw new NotImplementedException()
    };

    public record RoundResult(int Opponent1Score, int Opponent2Score);

    public record Round(EShape Opponent1Option, EShape Opponent2Option)
    {
        public ERoundWinner GetWinner()
        {
            return (Opponent1Option, Opponent2Option) switch
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
            var winner = GetWinner();

            return winner switch
            {
                ERoundWinner.Draw => new RoundResult(3 + Opponent1Option.GetScore(), 3 + Opponent2Option.GetScore()),
                ERoundWinner.Opponent1 => new RoundResult(6 + Opponent1Option.GetScore(), Opponent2Option.GetScore()),
                ERoundWinner.Opponent2 => new RoundResult(Opponent1Option.GetScore(), 6 + Opponent2Option.GetScore()),
            };
        }
    }

    public enum EShape
    {
        Rock,
        Paper,
        Scissors
    }

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
        Console.WriteLine("Running Day1 puzzle");
        Console.WriteLine();

        //Opponent2 = Me

        var meScore = await ParseInput()
                                .SumAsync(x => x.GetRoundScore().Opponent2Score);

        Console.WriteLine($"What would your total score be if everything goes exactly according to your strategy guide? {meScore}");

    }

    internal static async IAsyncEnumerable<Round> ParseInput()
    {
        await foreach (var line in File.ReadLinesAsync(@"Day2\input.txt"))
        {
            var options = line.Split(" ");

            var opponent1Option = options[0] switch
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

            var opponent2Option = (opponent2Instruction, opponent1Option) switch
            {
                (EOpponent2Instruction.MustDraw, _) => opponent1Option,

                (EOpponent2Instruction.MustLose, EShape.Rock) =>EShape.Scissors,
                (EOpponent2Instruction.MustLose, EShape.Paper) =>EShape.Rock,
                (EOpponent2Instruction.MustLose, EShape.Scissors) =>EShape.Paper,

                (EOpponent2Instruction.MustWin, EShape.Rock) => EShape.Paper,
                (EOpponent2Instruction.MustWin, EShape.Paper) => EShape.Scissors,
                (EOpponent2Instruction.MustWin, EShape.Scissors) => EShape.Rock,
            };



            yield return new Round(opponent1Option, opponent2Option);
        }
    }
}
