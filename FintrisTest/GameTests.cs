using FinTris;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FintrisTest
{
    [TestClass]
    public class GameTests
    {

        //[TestMethod]
        //public void RotateTest()
        //{
        //    Game game = new Game();
        //    game.Start();

        //    //Arrange

        //    List<Vector2> initialAngle = game.CurrentTetromino.Blocks;
        //    TetrominoType currentTetromino = game.CurrentTetromino.Shape;
        //    List<Vector2> expected;

        //    //Configurations de Tetromino Possibles après rotation 

        //    byte[,] squarieRotated = new byte[,]
        //        {
        //            {1, 1},
        //            {1, 1},
        //        };

        //    List<Vector2> lstVectors = new List<Vector2>
        //    {
        //        new Vector2(0, 0),
        //        new Vector2(0, 1),
        //        new Vector2(1, 0),
        //        new Vector2(1, 1),
        //    };
        //    byte[,] snakeRotated = new byte[,]
        //    {

        //            {0 ,1 , 1},
        //            {1 ,1 , 0},

        //    };

        //    byte[,] isnakeRotated = new byte[,]
        //        {
        //            {1, 1. 0},
        //            {0, 1, 1},
        //        };

        //    byte[,] lawletRotated = new byte[,]
        //        {
        //            {1 ,0},
        //            {1, 0},
        //            {1, 1},
        //        };

        //    byte[,] ilawletRotated = new byte[,]
        //        {
        //            {0 ,1},
        //            {0, 1},
        //            {1, 1},
        //        };


        //    byte[,] pyramidRotated = new byte[,]
        //         {
        //            {1, 1, 1},
        //            {0 ,1 ,0},
        //         };

        //    byte[,] malongRotated = new byte[,]
        //         {
        //            {1},
        //            {1},
        //            {1},
        //            {1}
        //         };

        //    //Act
        //    game.Rotate();
        //    var newAngle = game.CurrentTetromino.Blocks;


        //    switch (currentTetromino)
        //    {
        //        case TetrominoType.Squarie:
        //            expected = squarieRotated;
        //            break;
        //        case TetrominoType.Snake:
        //            break;
        //        case TetrominoType.ISnake:
        //            break;
        //        case TetrominoType.Malong:
        //            break;
        //        case TetrominoType.Lawlet:
        //            break;
        //        case TetrominoType.ILawlet:
        //            break;
        //        case TetrominoType.Pyramid:
        //            break;
        //        default:
        //            break;
        //    }

        //    //Assert
        //    Assert.AreEqual(expected, newAngle);
        //}

        [TestMethod]
        public void MoveRightTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Vector2 oldPos = game.CurrentTetromino.Position;
            Vector2 expected;
            Vector2 newPos;


            //Act
            game.MoveRight();
            newPos = game.CurrentTetromino.Position;
            expected = oldPos + Vector2.Right;

            //Assert
            Assert.AreEqual(expected, newPos);
        }

        [TestMethod]
        public void MoveLeftTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Vector2 oldPos = game.CurrentTetromino.Position;
            Vector2 expected;
            Vector2 newPos;


            //Act
            game.MoveLeft();
            newPos = game.CurrentTetromino.Position;
            expected = oldPos + Vector2.Left;

            //Assert
            Assert.AreEqual(expected, newPos);
        }

        [TestMethod]
        public void MoveDownTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Vector2 oldPos = game.CurrentTetromino.Position;
            Vector2 expected;
            Vector2 newPos;


            //Act
            game.MoveDown();
            newPos = game.CurrentTetromino.Position;
            expected = oldPos - Vector2.Down;

            //Assert
            Assert.AreEqual(expected, newPos);
        }

        [TestMethod]
        public void DropDownTest()   //TODO test est buggé, corriger le test, actual position du tetromino a un y de 0 ?
        {
            //Arrange
            Game game = new Game();
            Tetromino tetromino = game.CurrentTetromino;
            Vector2 expectedPos = new Vector2(game.CurrentTetromino.Position.x, game.Rows - tetromino.Height);

            game.Start();

            //épaisseur bordure

            //Act 
            game.DropDown();
            //La position du Tetromino est comptée à partir de la position en haut à gauche de sa grille 

            Vector2 actualPos = tetromino.Position;

            //Assert
            Assert.AreEqual(expectedPos, actualPos);
        }

        //TODO implémenter UpdateBoardTest
        //[TestMethod]
        //public void UpdateBoardTest()
        //{

        //    Game game = new Game();
        //    game.Start();

        //    //Arrange


        //    //Act
        //    var updateBoardTest = new PrivateObject(game);
        //    var res = updateBoardTest.Invoke("UpdateBoard");


        //    //Assert
        //    //Assert.AreEqual();
        //}

        //[TestMethod]
        //public void CollideAtTest()
        //{
        //    Game game = new Game();
        //    game.Start();

        //    //Arrange
        //    Vector2 tetroPos;

        //    //Act
        //    var CollideAtTest = new PrivateObject(game);
        //    //var args = tetroPos;
        //    //var res = privateTestMethod.Invoke("CollideAt", args); 


        //    //Assert
        //    //Assert.AreEqual(expected, newPos);
        //}

        [TestMethod]
        public void CheckForDeathTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            GameState actualState;
            GameState expectedState;


            //Act
            var privateTestMethod = new PrivateObject(game);

            //teste l'état donné par CheckForDeath pour chaque position de tetromino
            //(est censé affiché finished lorsqu'il sort du tableau)
            for (int y = 0; y < game.Rows; y++)
            {
                for (int x = 0; x < game.Cols; x++)
                {
                    Vector2 tetroPosTest = new Vector2(x, y);

                    if ((bool)privateTestMethod.Invoke("WithinRange", tetroPosTest))
                    {
                        expectedState = GameState.Playing;

                        privateTestMethod.Invoke("CheckForDeath");
                        actualState = game.State;

                        //Assert
                        Assert.AreEqual(expectedState, actualState);

                    }
                    else
                    {
                        expectedState = GameState.Finished;

                        privateTestMethod.Invoke("CheckForDeath");
                        actualState = game.State;

                        //Assert
                        Assert.AreEqual(expectedState, actualState);
                    }

                }
            }

        }

        //TODO Test inutile?, revoir
        //[TestMethod]
        //public void ScoreManagerTest()
        //{
        //    Game game = new Game();
        //    game.Start();

        //    //Arrange


        //    //Act
        //    var privateTestMethod = new PrivateObject(game);
        //    int args = 1;
        //    privateTestMethod.Invoke("ScoreManager", args);


        //    //Assert
        //    Assert.AreEqual(expectedState, actualState);

        //}

        [TestMethod]
        public void PauseTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            GameState actualState;
            GameState expectedState = GameState.Paused;

            //Act
            game.Pause();
            actualState = game.State;

            //Assert
            Assert.AreEqual(expectedState, actualState);
        }

        [TestMethod]
        public void WithinRangeTest()
        {
            Game game = new Game();
            game.Start();

            //Arrange
            Vector2 pos;
            bool expectedRangeState;

            //Act
            PrivateObject privateTestMethod = new PrivateObject(game);

            //Assert
            for (int y = 0; y < game.Rows; y++)
            {
                for (int x = 0; x < game.Cols; x++)
                {
                    pos = new Vector2(x, y);
                    bool actualRangeState = (bool)privateTestMethod.Invoke("WithinRange", pos);

                    if ((pos.x >= 0 && pos.x < game.Cols) && (pos.y >= 0 && pos.y < game.Rows))
                    {
                        expectedRangeState = true;
                        Assert.AreEqual(expectedRangeState, actualRangeState);
                    }
                    else
                    {
                        expectedRangeState = false;
                        Assert.AreEqual(expectedRangeState, actualRangeState);

                    }
                }

            }
        }
    }
}
