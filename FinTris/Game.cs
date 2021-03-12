using System;
using System.Timers;

namespace FinTris
{
    public class Game
    {
        private int _MS =500;
        private Tetromino _tetromino;
        private Timer _gameTimer;
        private int _rows;
        private int _cols;
        private Random random;
        private int _score;
        private int _level;

        public int Level 
        {
            get { return _level; }
            set { _level = value; }
        }
        public int MS
        {
            get { return _MS; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }


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

            _gameTimer = new Timer(_MS);
            _gameTimer.Elapsed += timerHandler;
            _rows = 22;
            _cols = 11;

            _level = 1;

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

                //Si on accèlère la chute on gagne plus de point
                _score += 10; //Si on presse 1 seconde on a 10 points en plus


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
                _tetromino.Y++;

                UpdateBoard();
            }            
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

            //On change le score

            ScoreManager();

            // On informe le renderer qu'il y a eu un changement et on lui dit que faire une mise à jour
            BoardChanged.Invoke(this, board);
        }

        public void Rotate()
        {
        }


        /// <summary>
        /// Fonction qui gère le score et //Changement de la vitesse des pièces qui tombent suivant le niveau
        /// </summary>
        /// <param name="nbrKill">nombre de lignes déruites</param>
        public void ScoreManager(int nbrKill=0)
        {

            //Calcul des points :
            //Une ligne complète = 40pts
            //Deux = 100 pts
            //Trois = 300pts
            //Quatre = 1200pts
            if (nbrKill == 1)
            {
                _score += 40;
            }
            else if (nbrKill == 2)
            {
                _score += 100;
            }

            else if (nbrKill == 3)
            {
                _score += 300;
            }

            else if (nbrKill == 4)
            {
                _score += 1200;
            }

             
            // Changement de niveau tout les 5000 points, chute accélérée selon le niveau
            _level = (_score / 1000) + 1;

            _gameTimer.Interval = _MS / (_level * 0.5);

        }
    }
}
