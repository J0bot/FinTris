﻿using System;
using System.Timers;
using System.Diagnostics;

namespace FinTris
{
    public class Game
    {
        public const int MS = 500;

        private Tetromino _tetromino;
        private Timer _gameTimer;
        private int _rows;
        private int _cols;
        private Random random;

        private SquareState[,] board;

        public event EventHandler<SquareState[,]> BoardChanged;

        //public event EventHandler TetrominoStopped;

        public int Cols
        {
            get { return _cols; }
        }
        public int Rows
        {
            get { return _rows; }
        }

        public Tetromino CurrentTetromino
        {
            get { return _tetromino; }
            set { _tetromino = value; }
        }
        public Timer GameTimer
        {
            get { return _gameTimer; }
            set { _gameTimer = value; }
        }

        public Game()
        {
            //On va spawn une pièce random
            random = new Random();

            _tetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);

            _gameTimer = new Timer(MS);
            _gameTimer.Elapsed += timerHandler;
            _rows = 22;
            _cols = 11;

            board = new SquareState[_cols, _rows];
        }

        public void MoveRight()
        {
            if (_tetromino.X + _tetromino.Blocks.GetLength(0) < _cols)
            {
                _tetromino.X++;
                UpdateBoard();
            }                        

        }
        public void MoveLeft()
        {
            if (_tetromino.X - 1 >= 0)
            {
                _tetromino.X--;
                UpdateBoard();
            }
        }

        public void MoveDown()
        {
            if (_tetromino.Blocks.GetLength(1) + _tetromino.Y + 1 <= _rows)
            {
                _gameTimer.Stop();
                _tetromino.Y++;
                UpdateBoard();
                _gameTimer.Start();
            }
        }

        public void Start()
        {
            _gameTimer.Start();
        }

        /// <summary>
        /// Cette fonction se déclenche chaque 500MS et va check les collisions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerHandler(object sender, ElapsedEventArgs e)
        {
            // Si on touche le bas du tableau
            if (_tetromino.Blocks.GetLength(1) + _tetromino.Y >= _rows)
            {
                _tetromino.State = TetrominoState.Stopped;
                
                //On va spawn une nouvelle pièce random

                 _tetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);

                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == SquareState.MovingBlock)
                        {
                            board[i, j] = SquareState.SolidBlock;
                        }
                    }
                }
            }
            // Si on ne touche rien
            else
            {
                for (int i = 0; i < _tetromino.Blocks.GetLength(0); i++)
                {
                    //Debug.WriteLine($"{_tetromino.X + i} {_tetromino.Y + _tetromino.Blocks.GetLength(1) + 1}");
                    byte data = _tetromino.Blocks[i, _tetromino.Blocks.GetLength(1) - 1];
                    if ((data == 1 && board[_tetromino.X + i, _tetromino.Y + _tetromino.Blocks.GetLength(1)] == SquareState.SolidBlock))
                    {
                        _tetromino.State = TetrominoState.Stopped;

                        //On va spawn une nouvelle pièce random

                        _tetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);

                        for (int a = 0; a < board.GetLength(0); a++)
                        {
                            for (int j = 0; j < board.GetLength(1); j++)
                            {
                                if (board[a, j] == SquareState.MovingBlock)
                                {
                                    board[a, j] = SquareState.SolidBlock;
                                }
                            }
                        }
                    }
                }
                #region way
                //for (int y = 0; y < _tetromino.Blocks.GetLength(1); y++)
                //{
                //    for (int x = 0; x < _tetromino.Blocks.GetLength(0); x++)
                //    {
                //        int x2 = _tetromino.X + x;
                //        int y2 = _tetromino.Y + y;

                //        if (board[x2, y2 + 1] == SquareState.SolidBlock && _tetromino.Blocks[x, y] != 1)
                //        {

                //        }
                //    }
                //}
                #endregion
                _tetromino.Y++;

                
            }
            UpdateBoard();
        }

        private void UpdateBoard()
        {         

            // Reset du tableau
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != SquareState.SolidBlock) // On va laisser les Tetrominos qui sont déjà tombés et on va reset le reste
                    {
                        board[i, j] = SquareState.Empty;
                    }
                }
            }

            byte[,] dataBoard = CurrentTetromino.Blocks;

            // On implémente notre tetromino dans notre board
            for (int y = 0; y < dataBoard.GetLength(1); y++)
            {
                for (int x = 0; x < dataBoard.GetLength(0); x++)
                {
                    board[x + CurrentTetromino.X, y + CurrentTetromino.Y] = dataBoard[x, y] == 0 ? SquareState.Empty : SquareState.MovingBlock;
                }
            }

            // On informe le renderer qu'il y a eu un changement et on lui dit que faire une mise à jour
            BoardChanged.Invoke(this, board);
        }

        public void Rotate()
        {
        }

        private void NextTetromino()
        {

        }



    }
}
