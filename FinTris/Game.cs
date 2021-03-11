///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;
using System.Timers;

namespace FinTris
{
    public class Game
    {
        /// <summary>
        /// Cette constante définit la vitesse de chute de notre Tetromino
        /// </summary>
        public const int MS = 500;

        /// <summary>
        /// Attribut _tetromino
        /// </summary>
        private Tetromino _tetromino;

        /// <summary>
        /// Attribut de timer pour gerer le temps
        /// </summary>
        private Timer _gameTimer;

        /// <summary>
        /// Attribut de nombre de lignes
        /// </summary>
        private int _rows;

        /// <summary>
        /// Attribut de nombre de collones
        /// </summary>
        private int _cols;

        /// <summary>
        /// Variable random pour executer toutes les fonctions avec random
        /// </summary>
        private Random random;

        /// <summary>
        /// C'est le tableau qui contient les états de tous les blocs du jeu
        /// </summary>
        private readonly SquareState[,] _board;

        /// <summary>
        /// C'est un événement qui permet de discuter avec GameRenderer pour assuser la synchronisation en l'affichage et la logique du jeu
        /// </summary>
        public event EventHandler<SquareState[,]> BoardChanged;

        /// <summary>
        /// Propriété qui retourne la quantité de colones dans notre plateau de jeu.
        /// 
        /// Est uniquement définie dans le constructeur de la classe Game
        /// </summary>
        public int Cols
        {
            get { return _cols; }
        }

        /// <summary>
        /// Propriété qui retourne la quantité de Lignes dans notre plateau de jeu.
        /// 
        /// Est uniquement définie dans le constructeur de la classe Game
        /// </summary>
        public int Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Propriété du Tetromino actuellement en jeu
        /// </summary>
        public Tetromino CurrentTetromino
        {
            get { return _tetromino; }
            set { _tetromino = value; }
        }

        /// <summary>
        /// Protriété du Timer du jeu
        /// </summary>
        public Timer GameTimer
        {
            get { return _gameTimer; }
            set { _gameTimer = value; }
        }

        /// <summary>
        /// Constructor renseigné de la classe Game
        /// </summary>
        /// <param name="rows">nombre de lignes (optionnel)</param>
        /// <param name="cols">nombre de collones (optionnel)</param>
        public Game(int rows = 22, int cols = 11)
        {
            //On va spawn une pièce random
            random = new Random();

            _tetromino = new Tetromino((TetrominoShape)random.Next(7), 3, 0);

            _gameTimer = new Timer(MS);
            _gameTimer.Elapsed += timerHandler;
            _rows = rows;
            _cols = cols;

            _board = new SquareState[cols, rows];
        }

        

        /// <summary>
        /// Fonction qui permet de lancer la rotation du Tetromino actuel, puis de update le plateau de jeu
        /// </summary>
        public void Rotate()
        {
            _tetromino.Rotate();
            UpdateBoard();
        }

        /// <summary>
        /// Fonction qui permet de faire bouger le Tetromino actuel vers la droite, puis de update le plateau de jeu
        /// 
        /// Le Tetromino ne va aller vers la droite que s'il ne dépasse pas les limites du terrain et qu'il n'y a pas de collisions avec tout autre bloc
        /// </summary>
        public void MoveRight()
        {
            Vector2 nextPos = _tetromino.Position + Vector2.Right;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position += Vector2.Right;
                UpdateBoard();
            }                        

        }

        /// <summary>
        /// Fonction qui permet de faire bouger le Tetromino actuel vers la gauche, puis de update le plateau de jeu.
        /// 
        /// Le Tetromino ne va aller vers la gauche que s'il ne dépasse pas les limites du terrain et qu'il n'y a pas de collisions avec tout autre bloc.
        /// </summary>
        public void MoveLeft()
        {
            Vector2 nextPos = _tetromino.Position + Vector2.Left;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position += Vector2.Left;
                UpdateBoard();
            }
        }

        /// <summary>
        /// Fonction qui permet de faire descendre manuelement le Tetromino vers le bas, puis de update le jeu.
        /// 
        /// Si on laisse la touche vers le bas enfoncée, le Tetromino va descendre encore plus vite
        /// 
        /// S'il y a un risque de sortir du terrain ou qu'il entre en collision avec un bloc, il ne va plus descendre.
        /// </summary>
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

        /// <summary>
        /// Fonction qui permet de faire tomber le Tetromino d'un coup. 
        /// 
        /// Dès qu'on presse sur la touche Enter, le Tetromino va descendre sans s'arreter jusqu'à ce qu'il trouve un obstacle, ou qu'il risque de sortir du terrain,
        /// 
        /// S'il touche un obstacle, ça va immédiatement faire spawn un nouveau Tetromino avec la méthode NewTetromino()
        /// </summary>
        public void DropDown()
        {
            _gameTimer.Stop();

            Vector2 nextPos = _tetromino.Position - Vector2.Down;

            while (!CollideAt(nextPos))
            {
                _tetromino.Position = nextPos;
                nextPos -= Vector2.Down;
                UpdateBoard();
            }
            _gameTimer.Start();
            NewTetromino();

        }

        /// <summary>
        /// Sert à start le timer
        /// </summary>
        public void Start()
        {
            _gameTimer.Start();
        }

        /// <summary>
        /// Cette fonction se déclenche chaque 500MS et va check les collisions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerHandler(object sender, ElapsedEventArgs e)
        {

            Vector2 nextPos = _tetromino.Position - Vector2.Down;

            if (!CollideAt(nextPos))
            {
                _tetromino.Position = nextPos;
            }
            else
            {

                NewTetromino();  
            }

            UpdateBoard();
        }

        /// <summary>
        /// Cette fonction va update le board.
        /// 
        /// On va commencer par reset le tableau de base pour effacer les données obsolètes.
        /// Par la suite on va implémenter le Tetromino dans le terrain.
        /// 
        /// Tout à la fin on va informer GameRenderer qu'il y a eu un changement dans le tableau.
        /// </summary>
        private void UpdateBoard()
        {         

            // Reset du tableau
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    if (_board[i, j] != SquareState.SolidBlock) // On va laisser les Tetrominos qui sont déjà tombés et on va reset le reste
                    {
                        _board[i, j] = SquareState.Empty;
                    }
                }
            }

            // On implémente notre tetromino dans notre board

            foreach (Vector2 block in _tetromino.Blocks)
            {
                Vector2 pos = block + _tetromino.Position;
                _board[pos.x, pos.y] = SquareState.MovingBlock;
            }


            // On informe le renderer qu'il y a eu un changement et on lui dit que faire une mise à jour
            BoardChanged.Invoke(this, _board);
        }



        /// <summary>
        /// Fonction qui va detecter les collisions des tetrominos
        /// </summary>
        private bool CollideAt(Vector2 tetroPos)
        {
            foreach (Vector2 bloc in _tetromino.Blocks)
            {
                // Conversion des coordonées de blocs à des coordonées relatives au plateau
                Vector2 pos = tetroPos + bloc;
                if (!WithinRange(pos) || _board[pos.x,pos.y] == SquareState.SolidBlock)
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

        /// <summary>
        /// Fonction qui permet d'instancier un nouveau Tetromino
        /// 
        /// On va commencer par stopper le Tetromino actuel, pui on va instancier le nouveau Tetromino
        /// Il va falloir ensuite transformer l'ancien Tetromino en bloc solide (SquareState.SolidBlock)
        /// </summary>
        private void NewTetromino()
        {
            _tetromino.State = TetrominoState.Stopped;

            //On va spawn une nouvelle pièce random

            _tetromino = new Tetromino((TetrominoShape)random.Next(7), 3, 0);
            //_tetromino = new Tetromino(TetrominoType.Pyramid, 3, 0);

            for (int a = 0; a < _board.GetLength(0); a++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    if (_board[a, j] == SquareState.MovingBlock)
                    {
                        _board[a, j] = SquareState.SolidBlock;
                    }
                }
            }

        }

        /// <summary>
        /// Cette fonction permet d'instancier en avance un Tetromino pour pouvoir le prévisualiser
        /// </summary>
        private void NextTetromino()
        {

        }

    }
}
