using FinTris;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FintrisTest
{
    [TestClass]
    public class GameTests
    {

        [TestMethod]
        public void ScoreTest()
        {

        }

        [TestMethod]
        public void RotateTest()
        {
            Game game = new Game();
            Tetromino tetromino = new Tetromino();
            game.Start();

            //Arrange
            TetrominoType initialAngle = game.CurrentTetromino.Shape;
            TetrominoType expected;
            TetrominoType newAngle;

            //Configurations de Tetromino Possibles après rotation 

            byte[,] squarieRotated = new byte[,]
                {
                    {1, 1},
                    {1, 1},
                };

            byte[,] snake = new byte[,]
            {
                    {1 ,0},
                    {1, 1},
                    {0, 1},
            };

            byte[,] isnake = new byte[,]
                {
                    {0 ,1},
                    {1, 1},
                    {1, 0},
                };

            byte[,] lawlet = new byte[,]
                {
                    {1 ,0},
                    {1, 0},
                    {1, 1},
                };

            byte[,] ilawlet = new byte[,]
                {
                    {0 ,1},
                    {0, 1},
                    {1, 1},
                };


            byte[,] pyramid = new byte[,]
                 {
                    {1, 1, 1},
                    {0 ,1 ,0},
                 };

            byte[,] malong = new byte[,]
                 {
                    {1},
                    {1},
                    {1},
                    {1}
                 };



            //Act
            initialAngle = game.CurrentTetromino.Shape;
            game.Rotate();
            newAngle = game.CurrentTetromino.Shape;
            expected = initialAngle;

            //Assert
            Assert.AreEqual(expected, newAngle);
        }

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
            GameRenderer gameRenderer = new GameRenderer(game); //TODO implémenter la variable d'épaisseur de la bordure lorsqu'elle sera mise à jour par Ahmad

            game.Start();

            Vector2 expectedPos;
            int initScore = game.Score;
            int expectedScore = initScore + 20;
            int actualScore;

            //Act 
            game.DropDown();
            //La position du Tetromino est comptée à partir de la position en haut à gauche de sa grille 
            //2 = épaisseur bordure

            expectedPos = new Vector2(game.CurrentTetromino.Position.x, game.Rows - game.CurrentTetromino.Height - 2 ); //bas du tableau 
            Vector2 actualPos = game.CurrentTetromino.Position;
            actualScore = game.Score;


            //Assert
            Assert.AreEqual(expectedPos, actualPos);
            //Assert.AreEqual(expectedScore, actualScore);
        }


    }
}
