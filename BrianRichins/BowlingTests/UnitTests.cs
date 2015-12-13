using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Bowling;

namespace BowlingTests
{
    [TestClass]
    public class UnitTests
    {

        Game TestGame { get; set; }
        Frame TestFrame { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            TestGame = new Game();
            TestFrame = new Frame();
        }


        #region Basic Setup
        [TestMethod]
        public void GameSetupTest()
        {
            Assert.IsNotNull(TestGame.Frames, "Game has no Frames list ");
            Assert.IsTrue(TestGame.Frames.Count == 1, "Game initialize did not create 1 starting frame");
            Assert.IsNotNull(TestGame.CurrentFrame, "CurrentFrame not initialized");

            Assert.IsNotNull(TestGame.CurrentFrame.Balls, "Frame initialized without list of Balls");
            Assert.IsTrue(TestGame.CurrentFrame.Balls.Count == 0, "Frame initialized with non-empty list of Balls");
        }

        [TestMethod]
        public void CanCreateFrame()
        {
            Assert.IsNotNull(TestFrame.Balls, "Frame created with no ball list");
            Assert.AreEqual(0, TestFrame.Balls.Count, "Frame created with wrong non-zero ball list");
        }

        [TestMethod]
        public void CanCreateBall()
        {
            int Score = 1;
            Ball NewBall = new Ball(Score);

            Assert.IsNotNull(NewBall);
            Assert.AreEqual(Score, NewBall.Score, "Ball created with wrong score");
        }


        [TestMethod]
        public void CanAddBallToGame()
        {
            int Score = 1;
            TestGame.AddBall(Score);

            Assert.IsNotNull(TestGame.CurrentFrame.Balls);
            Assert.AreEqual(1, TestGame.CurrentFrame.Balls.Count, "Frame has <> 1 ball after adding 1 ball");
            Assert.AreEqual(Score, TestGame.CurrentFrame.Balls.First().Score, "Ball score wrong after adding to game");
        }

        // Ball-Frame interaction

        [TestMethod]
        public void AddBallsCreatesNewFrame()
        {
            int Score = 1;
            TestGame.AddBall(Score);
            TestGame.AddBall(Score);
            TestGame.AddBall(Score);

            Assert.AreEqual(2, TestGame.Frames.Count, "Game doesn't have 2 frames after first 3 balls");
            Assert.AreEqual(TestGame.Frames.Last(), TestGame.CurrentFrame, "CurrentFrame not updated to new Frame");
        }

        
        [TestMethod]
        public void FrameClosed_Gutter()
        {
            Frame TestFrame = new Frame();
            Assert.IsFalse(TestFrame.IsComplete, "Frame closed with no balls added");

            TestFrame.Balls.Add(new Ball(0));
            Assert.IsFalse(TestFrame.IsComplete, "Frame closed with non-strike ball");

            TestFrame.Balls.Add(new Ball(0));
            Assert.IsTrue(TestFrame.IsComplete, "Frame not closed after 2 balls");
        }
        [TestMethod]
        public void FrameClosed_NonFull()
        {
            TestFrame.Balls.Add(new Ball(4));
            Assert.IsFalse(TestFrame.IsComplete, "Frame closed with non-strike ball");

            TestFrame.Balls.Add(new Ball(4));
            Assert.IsTrue(TestFrame.IsComplete, "Frame not closed after 2 balls");
        }
        [TestMethod]
        public void FrameClosed_Spare()
        {
            TestFrame.Balls.Add(new Ball(4));
            TestFrame.Balls.Add(new Ball(6));
            Assert.IsTrue(TestFrame.IsComplete, "Frame not closed after spare");
        }
        [TestMethod]
        public void FrameClosed_Strike()
        {
            TestFrame.Balls.Add(new Ball(10));
            Assert.IsTrue(TestFrame.IsComplete, "Frame not closed with non-strike ball");
        }

        [TestMethod]
        public void GameHoldsTenFrames()
        {
            for (int i = 0; i < 9; i++) // initialized with 1 frame already
            {
                TestGame.AddFrame();
            }

            Assert.AreEqual(10, TestGame.Frames.Count, "Game doesn't have 10 frames");
            Assert.AreEqual(0, TestGame.Frames.Last().Balls.Count, "10th frame contains balls without being added");
            Assert.IsFalse(TestGame.Frames.Last().IsComplete, "10th frame complete with no balls");
            Assert.IsFalse(TestGame.IsGameOver, "Game over before 10th frame closed");

            TestGame.AddFrame();
            Assert.AreEqual(11, TestGame.Frames.Count, "Game doesn't have 11 frames after adding 11th frame");
            Assert.IsTrue(TestGame.IsGameOver, "Game not over after 11th frame added");
        }
        #endregion


        #region Input & Frame Score Validation

        [TestMethod]
        public void ValidateScoreInputInvalidTest()
        {
            int Score = 0;
            Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput("asdf", ref Score), "Accepted string input as score");
            Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput("\n\t\r", ref Score), "Accepted special characters input");
            Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput("-1", ref Score), "Accepted negative number input");
            Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput("11", ref Score), "Accepted impossible score input");
            Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput("999999999999999999999999", ref Score), "Accepted overflowed integer input");
        }
        [TestMethod]
        public void EmtpyFrame_ValidateScoreInputValidTest()
        {
            int Score = 0;
            for (int i = 0; i <= 10; i++)
            {
                Assert.IsTrue(TestGame.CurrentFrame.ValidateScoreInput(i.ToString(), ref Score), "Empty frame failed to accept valid score of " + i);
            }

        }
        [TestMethod]
        public void GutterFrame_ValidateScoreInputValidTest()
        {
            int Score = 0;
            TestGame.AddBall(Score);
            for (int i = 0; i <= 10; i++)
            {
                Assert.IsTrue(TestGame.CurrentFrame.ValidateScoreInput(i.ToString(), ref Score), "Partial frame with 0 score rejected valid score of " + i);
            }
        }
        [TestMethod]
        public void PartialFrame_ValidateScoreInputValidTest()
        {
            int Score = 5;
            int Dummy = 0;
            TestGame.AddBall(Score);

            for (int i = 0; i <= 10; i++)
            {
                if (i <= Score)
                    Assert.IsTrue(TestGame.CurrentFrame.ValidateScoreInput(i.ToString(), ref Dummy), "Partial frame with 5 score rejected valid score of " + i);
                else
                    Assert.IsFalse(TestGame.CurrentFrame.ValidateScoreInput(i.ToString(), ref Dummy), "Partial frame with 5 score accepted invalid score of " + i);
            }
        }

        #endregion


        #region Score

        private static void AddStrikes(Game game, int frameCount)
        {
            for (int i = 1; i <= frameCount; i++)
            {
                game.AddBall(10);
            }

        }

        [TestMethod]
        public void TenthFrame_GuttersTest()
        {
            AddStrikes(TestGame, 9);

            TestGame.AddBall(0);
            TestGame.AddBall(0);

            Assert.AreEqual(0, TestGame.Frames[9].Score, "Gutterball 10th frame has non-zero score");
            Assert.IsTrue(TestGame.Frames[9].IsComplete, "10th frame not complete after not getting strike/spare");
            Assert.AreEqual(11, TestGame.Frames.Count, "10th frame failed to complete game after 2 balls < 10 score");

            TestGame.AddBall(0);
            Assert.AreEqual(2, TestGame.Frames[9].Balls.Count, "10th frame failed to reject 3rd ball");

        }
        [TestMethod]
        public void TenthFrame_SubCompleteTest()
        {
            AddStrikes(TestGame, 9);

            TestGame.AddBall(3);
            TestGame.AddBall(5);

            Assert.AreEqual(8, TestGame.Frames[9].Score, "Tenth frame score incorrect");
            Assert.IsTrue(TestGame.Frames[9].IsComplete, "10th frame not complete after not getting strike/spare");
            Assert.AreEqual(11, TestGame.Frames.Count, "10th frame failed to complete game after 2 balls < 10 score");

            TestGame.AddBall(3);
            Assert.AreEqual(2, TestGame.Frames[9].Balls.Count, "10th frame failed to reject 3rd ball");
        }
        [TestMethod]
        public void TenthFrame_SpareTest()
        {
            AddStrikes(TestGame, 9);

            TestGame.AddBall(3);
            TestGame.AddBall(7);

            Assert.AreEqual(10, TestGame.Frames[9].Score, "Tenth frame score incorrect");
            Assert.IsFalse(TestGame.Frames[9].IsComplete, "10th frame flagged complete after getting a spare");
            Assert.AreEqual(10, TestGame.Frames.Count, "10th frame completed game prematurely after getting a spare");
            Assert.AreEqual(263, TestGame.Score, "10th frame spare score incorrect");

            TestGame.AddBall(3);
            Assert.AreEqual(3, TestGame.Frames[9].Balls.Count, "10th frame rejected 3rd ball");
            Assert.AreEqual(13, TestGame.Frames[9].Score, "10th frame scored incorrectly after 3rd ball");
            Assert.AreEqual(266, TestGame.Score, "10th frame spare score incorrect");
        }
        [TestMethod]
        public void TenthFrame_Strike1Test()
        {
            AddStrikes(TestGame, 10);
            TestGame.AddBall(3);
            
            Assert.IsFalse(TestGame.Frames[9].IsComplete, "10th frame flagged complete after getting a strike");
            Assert.AreEqual(10, TestGame.Frames.Count, "10th frame completed game prematurely after getting a spare");

            TestGame.AddBall(3);
            Assert.AreEqual(3, TestGame.Frames[9].Balls.Count, "10th frame rejected 3rd ball");
            Assert.AreEqual(16, TestGame.Frames[9].Score, "10th frame scored incorrectly after 3rd ball");
            Assert.AreEqual(279, TestGame.Score, "10th frame strike non-spare score incorrect");
        }
        [TestMethod]
        public void TenthFrame_StrikeSpareTest()
        {
            AddStrikes(TestGame, 10);
            TestGame.AddBall(3);

            Assert.IsFalse(TestGame.Frames[9].IsComplete, "10th frame flagged complete after getting a strike");
            Assert.AreEqual(10, TestGame.Frames.Count, "10th frame completed game prematurely after getting a spare");

            TestGame.AddBall(7);
            Assert.AreEqual(3, TestGame.Frames[9].Balls.Count, "10th frame rejected 3rd ball");
            Assert.AreEqual(20, TestGame.Frames[9].Score, "10th frame scored incorrectly after 3rd ball");
            Assert.AreEqual(283, TestGame.Score, "10th frame strike-spare score incorrect");
        }


        [TestMethod]
        public void ScorePerfectTest()
        {
            int BallCount = 0;
            for (int i = 1; i <= 12; i++)    //initialized with 1 frame already
            {
                TestGame.AddBall(10);
                BallCount++;
            }

            Assert.AreEqual(12, TestGame.Frames.Sum(f => f.OwnBalls.Count()), "Perfect game does not have 12 balls");
            Assert.AreEqual(11,TestGame.Frames.Count, "12 strikes failed to create 11th frame");

            var GameFrames = TestGame.Frames.Take(10).ToList();
            Assert.IsTrue(GameFrames.TrueForAll(f => f.Balls.Count == 3), "Not all frames have 3 balls");
            Assert.IsTrue(GameFrames.TrueForAll(f => f.Score == 30), "Not all frames scored 30");

            Assert.IsTrue(TestGame.IsGameOver, "Full game not complete");
            Assert.AreEqual(300, TestGame.Score, "Perfect game didn't have score of 300");
        }

        [TestMethod]
        public void ScoreGutterballsTest()
        {
            int BallCount = 0;
            for (int i = 1; i <= 20; i++)    //initialized with 1 frame already
            {
                TestGame.AddBall(0);
                BallCount++;
            }

            Assert.AreEqual(20, TestGame.Frames.Sum(f => f.OwnBalls.Count()), "Gutter game does not have 20 balls");
            Assert.AreEqual(11, TestGame.Frames.Count, "Full gutter game failed to create 11th frame");

            var GameFrames = TestGame.Frames.Take(10).ToList();
            Assert.IsTrue(GameFrames.TrueForAll(f => f.Balls.Count == 2), "Not all frames have 2 balls");
            Assert.IsTrue(GameFrames.TrueForAll(f => f.Score == 0), "Not all frames scored 0");

            Assert.IsTrue(TestGame.IsGameOver, "Full gutter game not complete");
            Assert.AreEqual(0, TestGame.Score, "Worst game didn't have score of 0");
        }



        #endregion
    }
}
