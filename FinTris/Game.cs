﻿using System;
using System.Timers;

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

            int randomTetromino = random.Next(7);
            int randomPosX = random.Next(0, 5);

            _tetromino = new Tetromino((TetrominoType)randomTetromino, 2 * randomPosX, 0);

            _gameTimer = new Timer(MS);
            _gameTimer.Elapsed += timerHandler;
            _rows = 22;
            _cols = 11;

            board = new SquareState[_cols, _rows];
        }

        public void MoveRight()
        {
            if (_tetromino.X + 1 <= _rows)
            {
                _tetromino.X++;
            }                        
        }
        public void MoveLeft()
        {
            if (_tetromino.X - 1 >=0)
            {
                _tetromino.X--;
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
                //On va spawn une nouvelle pièce random
                int randomTetromino = random.Next(0,7);
                int randomPosX = random.Next(0,4);

                _tetromino = new Tetromino((TetrominoType)randomTetromino, 2*randomPosX, 0);

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
                _tetromino.Y++;

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

                Tetromino tetromino = CurrentTetromino;
                byte[,] dataBoard = tetromino.Blocks;

                // On implémente notre tetromino dans notre board
                for (int y = 0; y < dataBoard.GetLength(1); y++)
                {
                    for (int x = 0; x < dataBoard.GetLength(0); x++)
                    {
                        board[x+tetromino.X, y + tetromino.Y] = dataBoard[x, y] == 0 ? SquareState.Empty : SquareState.MovingBlock;
                    }
                }

                // On informe le renderer qu'il y a eu un changement et on lui dit que faire une mise à jour
                BoardChanged.Invoke(this, board);
            }            
        }

        public void Rotate()
        {
        }
    }
}
