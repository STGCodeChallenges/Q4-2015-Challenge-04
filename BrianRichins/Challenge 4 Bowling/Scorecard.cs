using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Round
{
    public Round(int players)
    {
        this.Games = new List<Game>(players);

        int Result = Math.Min(Math.Max(1, players), 10);  // force 1-10 players
        for (int i = 0; i < Result; i++)
        {
            this.Games.Add(new Game());
        }
    }

    public List<Game> Games { get; set; }
    public bool QuitNow = false;
    public bool IsRoundOver { get { return Games.All(g => g.IsGameOver); } }


    public int GetWinnerScore()
    {
        return this.Games.Max(g => g.Score);
    }

    public void PrintScores()
    {
        if (this.Games == null)
        {
            Console.WriteLine("No games to print scores for");
            return;
        }
        else
        {
            for (int i = 1; i <= Games.Count; i++)
            {
                Console.WriteLine("Player {0}:", i);
                Games[i-1].PrintScore();
            }
        }
    }
}

public class Game
{
    public Game()
    {
        Frames = new List<Frame>(11);
        AddFrame();
    }

    public List<Frame> Frames { get; private set; }
    public Frame CurrentFrame;
    public int Score { get { return this.Frames.Take(10).Sum(f => f.Score); } }

    public Frame AddFrame()
    {
        var NewFrame = this.CurrentFrame = new Frame();
        Frames.Add(NewFrame);
        NewFrame.Index = Frames.Count;
        return NewFrame;
    }

    public bool AddBall(int Score)  //mostly for testing
    {
        return AddBall(Score.ToString());
    }
    public bool AddBall(string scoreInput)
    {
        int Score = 0;
        bool InputValidForNewBall = this.CurrentFrame.ValidateScoreInput(scoreInput, ref Score);
        
        if (InputValidForNewBall)
        {
            var NewBall = new Ball(Score);
            this.CurrentFrame.Balls.Add(NewBall);

            //add to previous frame if it was a strike or spare    
            if (Frames.Count > 1)
            {
                var LastFrame = Frames[Frames.Count - 2];
                if ((LastFrame.IsSpare || LastFrame.IsStrike) && LastFrame.Balls.Count < 3)    // prevent frame 10 from adding ball 3 to frame 9
                    LastFrame.Balls.Add(NewBall);

                if (Frames.Count > 2 && LastFrame.IsStrike)
                {
                    var FrameMinus2 = Frames[Frames.Count - 3];
                    if (FrameMinus2.IsStrike && FrameMinus2.Balls.Count < 3)    // prevent frame 10 from adding balls 2 & 3 to frame 8
                        FrameMinus2.Balls.Add(NewBall);
                }
            }

            //see if frame is completed; add new frame before rolling next ball
            if (CurrentFrame.IsComplete)
                AddFrame();
        }

        return InputValidForNewBall;
    }

    
    public bool IsGameOver { get { return this.Frames.Count > 10; } }

    public List<StringBuilder> GetCard()
    {
        StringBuilder Divider = new StringBuilder();
        StringBuilder Header = new StringBuilder(10);
        StringBuilder FrameScore = new StringBuilder(10);
        StringBuilder RunningScore = new StringBuilder(10);
        /*
         * --------|--------|--------|
         * 01     X|02   9,1|03 X,6,0|
         *    20   |   16   |    6   | 
         *  [ 20]  | [ 36]  |  [42]  |
         * --------|--------|--------|
         * 
         * 3 balls 3 balls 2 balls
         * 10,9,1 / 9,1,6 / 6,0
         */
        int RunningTotal = 0;
        foreach (var frame in Frames.Where(f => f.Index <= 10)) //skip frame 11, which indicates the game is over
        {
            Divider.Append("-------|");
            Header.Append(frame.Index.ToString().PadLeft(2, '0') + frame.DisplayOwnBalls.PadLeft(5, ' ') + '|');
            FrameScore.Append(frame.Score.ToString().PadLeft(5, ' ') + "  |");

            RunningTotal += frame.Score;
            RunningScore.Append(" [" + RunningTotal.ToString().PadLeft(3, ' ') + "] |");
        }

        var CardStrings = new List<StringBuilder>(4);
        CardStrings.Add(Divider);
        CardStrings.Add(Header);
        CardStrings.Add(FrameScore);
        CardStrings.Add(RunningScore);

        return CardStrings;
    }
    public void PrintScore()
    {
        GetCard().ForEach(sb => Console.WriteLine(sb));
    }
}

public class Frame
{
    public Frame() { Balls = new List<Ball>(3); }

    public List<Ball> Balls { get; set; }
    public int Index { get; set; }
    public int Score { get { return Balls.Sum(b => b.Score); } }
    public int MaxPossible()
    {
        if (IsComplete)
            return 0;
        else if (Balls.Count == 0)
            return 10;
        else if (Index < 10)
            return 10 - Score;
        else
        {
            if (Balls.Count == 1)   //evaluating ball 2
            {
                if (this.IsStrike)
                    return 10;
                else
                    return 10 - Score;
            }
            // evaluating ball 3;  ball 1 was a strike or frame would be complete
            else if (this.IsSpare || Balls[1].Score == 10)  //spare or 2nd strike
                return 10;
            else
                return 10 - Balls[1].Score;

        }
    }
    public bool ValidateScoreInput(string input, ref int number)
    {
        var IsInteger = Int32.TryParse(input, out number);

        if (IsInteger && number >= 0 && number <= this.MaxPossible())
            return true;
        else
            return false;
    }
    public bool IsStrike
    {
        get
        {
            return Balls.Count >= 1 && Balls.First().Score == 10;
        }
    }
    public bool IsSpare
    {
        get { return (Balls.Count >= 2) && (Balls[0].Score < 10) && (Balls.Take(2).Sum(b => b.Score) == 10); }
    }
    public bool IsComplete
    {
        get
        {
            if (this.Index < 10)
            {
                if (this.IsSpare || this.IsStrike)
                    return true;
                else
                    return this.Balls.Count == 2;
            }
            else    //last frame
            {
                switch (this.Balls.Count)
                {
                    default:
                    case 0:
                    case 1:
                        return false;
                    case 2:
                        return this.Score < 10;
                    case 3:
                        return true;
                }
            }
        }
    }

    public IEnumerable<Ball> OwnBalls
    {
        get
        {
            if (this.Index < 10)
            {
                if (this.IsStrike)
                    return this.Balls.Take(1);
                else if (this.IsSpare)
                    return this.Balls.Take(2);
            }
            // else last frame or non-filled frame
            return this.Balls;
        }
    }
    public string DisplayOwnBalls
    {
        get
        {
            List<string> ScoreString = this.OwnBalls.Select(b => b.DisplayScore).ToList();
            if (this.IsSpare)  //gauranteed to have 2 balls
                ScoreString[1] = "/";

            if (this.Index == 10 && this.IsStrike && this.IsComplete)  //gauranteed to have 3 balls; if 3rd is strike it is already an 'X'
            {
                if (this.Balls[1].Score < 10 && this.Balls.Skip(1).Sum(b => b.Score) == 10) // see if last ball is a spare
                    ScoreString[2] = "/";
            }

            return String.Join(",", ScoreString);
        }
    }
}

public class Ball
{
    public Ball(int score) 
    {
        if (score >= 0 && score <= 10)
            Score = score; 
    }

    public int Score { get; private set; }
    public string DisplayScore
    {
        get
        {
            if (Score == 10)
                return "X";
            else
                return Score.ToString();
        }
    }
}

public enum Command
{
    Score = -1,
    Quit = -2,
    Card = -3
}