using System;
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
            Vector2 nextPos = _tetromino.Position + Vector2.Right;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position += Vector2.Right;
                UpdateBoard();
            }                        

        }
        public void MoveLeft()
        {
            Vector2 nextPos = _tetromino.Position + Vector2.Left;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position += Vector2.Left;
                UpdateBoard();
            }
        }

        public void MoveDown()
        {
            Vector2 nextPos = _tetromino.Position - Vector2.Down;

            if (!CollideAt(nextPos))
            {
                _gameTimer.Stop();
                _tetromino.Position -= Vector2.Down;
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
            #region ancient colisions wey
            //// Si on touche le bas du tableau
            //if (_tetromino.Height + _tetromino.Position.y >= _rows)
            //{
            //    _tetromino.State = TetrominoState.Stopped;

            //    //On va spawn une nouvelle pièce random

            //     _tetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);

            //    for (int i = 0; i < board.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < board.GetLength(1); j++)
            //        {
            //            if (board[i, j] == SquareState.MovingBlock)
            //            {
            //                board[i, j] = SquareState.SolidBlock;
            //            }
            //        }
            //    }
            //}
            //// Si on ne touche rien
            //else
            //{
            #endregion
            Vector2 nextPos = _tetromino.Position - Vector2.Down;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position = nextPos;
            }
            else
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
                //_tetromino.Position -= Vector2.down;


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


            // On implémente notre tetromino dans notre board
            foreach (Vector2 block in _tetromino.Blocks)
            {
                Vector2 pos = block + _tetromino.Position;
                board[pos.x, pos.y] = SquareState.MovingBlock;
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

        /// <summary>
        /// Fonction qui va detecter les collisions des tetrominos
        /// </summary>
        private bool CollideAt(Vector2 tetroPos)
        {
            foreach (Vector2 bloc in _tetromino.Blocks)
            {
                //Conversion des coordonées de blocs à des coordonées relatives au plateau
                Vector2 pos = tetroPos + bloc;
                if (!WithinRange(pos) || board[pos.x,pos.y] ==SquareState.SolidBlock)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Detecter si la position données est dans le terrain de jeu
        /// </summary>
        /// <param name="pos">La position à vérifier</param>
        /// <returns>Vrai si la position donnée est dans le monde </returns>
        private bool WithinRange(Vector2 pos)
        {
            return pos.x >= 0 && pos.x < _cols && pos.y >= 0 && pos.y < _rows ;
        }

    }
}
