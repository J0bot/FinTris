using FinTris;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Fintris.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void PauseTest()
        {
            //Arrange
            Game game = new Game();
            GameState actualState;
            GameState expectedState = GameState.Paused;

            //Act
            game.Start();
            game.Pause();
            actualState = game.State;

            //Assert
            Assert.AreEqual(expectedState, actualState);
        }

        [TestMethod]
        public void ResumeTest()
        {
            //Arrange
            Game game = new Game();
            GameState actualState;
            GameState expectedState = GameState.Playing;

            //Act
            game.Start();
            game.Pause();
            game.Resume();
            actualState = game.State;

            //Assert
            Assert.AreEqual(expectedState, actualState);
        }

        [TestMethod]
        public void MoveDownTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Vector2 pos = game.CurrentTetromino.Position;
            Vector2 expected = pos + Vector2.Up;
            Vector2 actual;

            //Act
            game.MoveDown();
            actual = game.CurrentTetromino.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DropDownTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Tetromino tetro = game.CurrentTetromino;
            Vector2 pos = tetro.Position;
            Vector2 expected = pos + (Vector2.Up * (game.Rows - tetro.Height));
            Vector2 actual;

            //Act
            game.DropDown();
            actual = tetro.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WithinRangeTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            PrivateObject pv = new PrivateObject(game);
            int m = 5; // Margin

            //Act
            for (int y = -m; y < game.Rows + m; y++)
            {
                for (int x = -m; x < game.Columns + m; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    bool actual = (bool)pv.Invoke("WithinRange", pos);
                    bool expected;

                    if ((pos.x >= 0 && pos.x < game.Columns) && (pos.y >= 0 && pos.y < game.Rows))
                    {
                        expected = true;
                    }
                    else
                    {
                        expected = false;
                    }
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod]
        public void DeleteRowTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            PrivateObject pv = new PrivateObject(game);
            int rowY = game.Rows - 2;

            //Act
            game.DropDown();
            pv.Invoke("DeleteRow", rowY);

            //Assert
            for (int x = 0; x < game.Columns; x++)
            {
                SquareState expected = SquareState.Empty;
                SquareState actual = game.Board[x, rowY];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void UpdateScoreTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            PrivateObject pv = new PrivateObject(game);
            int[] expected = new int[] { 40, 100, 300, 1200 };
            int[] actual = new int[expected.Length];

            //Act
            for (int i = 0; i < expected.Length; i++)
            {
                int oldScore = game.Score;
                pv.Invoke("UpdateScore", i + 1);
                actual[i] = game.Score - oldScore;
            }

            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangeStateTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            PrivateObject pv = new PrivateObject(game);
            Array values = Enum.GetValues(typeof(GameState));
            GameState[] expected = (GameState[])values;
            GameState[] actual = new GameState[expected.Length];

            //Act
            int i = 0;
            foreach (int val in values)
            {
                pv.Invoke("ChangeState", (GameState)val);
                actual[i] = game.State;
                i++;
            }

            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MoveRightTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Tetromino tetro = game.CurrentTetromino;
            Vector2 expected = tetro.Position + Vector2.Right;
            Vector2 actual;

            //Act
            game.MoveRight();
            actual = game.CurrentTetromino.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }
            
    }
}
