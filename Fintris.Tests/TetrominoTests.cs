using FinTris;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Fintris.Tests
{
    [TestClass]
    public class TetrominoTests
    {
        [TestMethod]
        public void MoveTest()
        {
            // Arrange
            Random rand = new Random();
            Tetromino tetro = new Tetromino(TetrominoType.Squarie, Vector2.Zero);
            Vector2 oldPos = tetro.Position;
            Vector2 expected = new Vector2(rand.Next(10), rand.Next(10));
            Vector2 actual;

            // Act
            tetro.Move(expected);
            actual = tetro.Position;

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(oldPos, tetro.PreviousPosition);
        }
    }
}
