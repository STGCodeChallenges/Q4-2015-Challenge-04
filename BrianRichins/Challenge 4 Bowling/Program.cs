using System;
using System.Linq;

namespace Bowling
{
    class Program
    {
        private static Round CurrRound { get; set; }


        static void Main(string[] args)
        {
            // start round
            Console.WriteLine("Welcome to a new round! Enter 'score' to display the current player's score, 'card' for a full scorecard, or 'quit' to end the round.");
            Console.WriteLine("Enter number of players (1-10):");

            int Players = 1;
            bool CancelRound = !GetValidInput(new Frame(), ref Players);

            if (!CancelRound && Players > 0)
            {
                CurrRound = new Round(Players);
                // play round
                while (!CurrRound.IsRoundOver && !CurrRound.QuitNow)
                {
                    for (int i = 0; i < CurrRound.Games.Count; i++)
                    {
                        var ThisFrame = CurrRound.Games[i].Frames.Last();
                        while (!ThisFrame.IsComplete && !CurrRound.IsRoundOver && !CurrRound.Games[i].IsGameOver && !CurrRound.QuitNow)
                        {
                            Console.WriteLine("Player {0}, Frame {1}, Ball {2}:", i + 1, CurrRound.Games[i].Frames.Count, CurrRound.Games[i].CurrentFrame.Balls.Count + 1);
                            CurrRound.QuitNow = !NewBallAndContinue(CurrRound.Games[i]);
                        }

                        if (CurrRound.QuitNow)
                            break;
                    }
                }
            }

            // end round
            int WinnerScore = CurrRound.GetWinnerScore();
            CurrRound.PrintScores();

            Console.WriteLine("The winning score was {0}. Thanks for playing!", WinnerScore /*, Winner.Score */);
            Console.ReadKey();
        }

        // static methods
        public static bool GetValidInput(Frame frame, ref int result)
        {
            string input = Console.ReadLine();
            bool IsInvalid = false;

            switch (input.ToLower())
            {
                case "card":
                    CurrRound.PrintScores();
                    return GetValidInput(frame, ref result);
                case "score":
                    var CurrGame = CurrRound.Games.First(g => g.Frames.Any(f => f == frame));
                    Console.WriteLine(CurrGame.Score);
                    return GetValidInput(frame, ref result);
                case "quit":
                    result = (int)Command.Quit;
                    return true;
                default:
                    if (!frame.ValidateScoreInput(input, ref result))
                    {
                        Console.WriteLine("Please enter a valid number (0-{0}) or command ('score', 'card', or 'quit'):", frame.MaxPossible());
                        return GetValidInput(frame, ref result);
                    }
                    else
                        return true;
            }
        }

        public static bool NewBallAndContinue(Game game)
        {
            int Score = 0;
            GetValidInput(game.CurrentFrame, ref Score);

            if (Score == (int)Command.Quit)
                return false;   //quit
            else
            {
                game.AddBall(Score.ToString());
                return true;
            }
        }
    }
}