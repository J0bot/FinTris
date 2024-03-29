/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using System.Timers;

namespace FinTris
{
    /// <summary>
    /// Classe Game qui est toute la logique du jeu.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Cette variable définit la vitesse de chute de notre Tetromino.
        /// </summary>
        private int _speed = 600;

        /// <summary>
        /// Attribut _tetromino.
        /// </summary>
        private Tetromino _tetromino;

        /// <summary>
        /// Attribut de next Tetromino.
        /// </summary>
        private Tetromino _nextTetromino;

        /// <summary>
        /// Attribut de timer pour gerer le temps.
        /// </summary>
        private readonly Timer _gameTimer;

        /// <summary>
        /// Attribut de nombre de lignes.
        /// </summary>
        private readonly int _rows;

        /// <summary>
        /// Attribut de nombre de colones.
        /// </summary>
        private readonly int _columns;

        /// <summary>
        /// Variable random pour executer toutes les fonctions avec random.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Le nombre total de toutes les formes.
        /// </summary>
        private readonly int _shapesCount;

        /// <summary>
        /// Attribut de score.
        /// </summary>
        private int _score;

        /// <summary>
        /// Attribut de lignes supprimées.
        /// </summary>
        private int _rowsCleared;

        /// <summary>
        /// Attribut de niveau.
        /// </summary>
        private int _level;

        /// <summary>
        /// Attribut de l'état du jeu.
        /// </summary>
        private GameState _state;

        /// <summary>
        /// C'est le tableau qui contient les états de tous les blocs du jeu.
        /// </summary>
        private readonly SquareState[,] _board;

        /// <summary>
        /// Événement qui permet de discuter avec GameRenderer pour assuser la synchronisation en l'affichage et la logique du jeu.
        /// </summary>
        public event EventHandler TetrominoMoved;

        /// <summary>
        /// Événement qui se déclenche quand un nouveau tetromino se génère.
        /// </summary>
        public event EventHandler NextTetroSpawned;

        /// <summary>
        /// Événement qui se déclenche quand une ligne est supprimée.
        /// </summary>
        public event EventHandler<int> RowCleared;

        /// <summary>
        /// Événement qui va nous permettre de lancer tout ce qui concerne les animations de fin de partie.
        /// </summary>
        public event EventHandler<GameState> StateChanged;

        /// <summary>
        /// Sert dans la fonction Rotate
        /// </summary>
        private Tetromino _rotatedTetro;

        /// <summary>
        /// Propriété read-only qui retourne le nombre de colones dans notre plateau de jeu.
        /// </summary>
        public int Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Nous retourne le niveau dans lequel on est actuellement.
        /// </summary>
        public int Level
        {
            get { return _level; }
        }

        /// <summary>
        /// Nous retourne notre score.
        /// </summary>
        public int Score
        {
            get { return _score; }
        }

        /// <summary>
        /// Nous retourne notre score.
        /// </summary>
        public int RowsCleared
        {
            get { return _rowsCleared; }
        }

        /// <summary>
        /// Propriété read-only qui retourne la quantité de Lignes dans notre plateau de jeu.
        /// </summary>
        public int Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Référence de l'instance du Tetromino actuellement en jeu.
        /// </summary>
        public Tetromino CurrentTetromino
        {
            get { return _tetromino; }
            set { _tetromino = value; }
        }

        /// <summary>
        /// Référence de l'instance du prochain Tetromino.
        /// </summary>
        public Tetromino NextTetromino
        {
            get { return _nextTetromino; }
            set { _nextTetromino = value; }
        }

        /// <summary>
        /// L'état du jeu.
        /// </summary>
        public GameState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Plateau de jeu, d'un point de vue d'état des blocs
        /// </summary>
        public SquareState[,] Board
        {
            get { return _board; }
        }


        /// <summary>
        /// Constructor renseigné de la classe Game.
        /// </summary>
        /// <param name="rows">Nombre de lignes (optionnel).</param>
        /// <param name="cols">Nombre de collones (optionnel).</param>
        public Game(int rows = 22, int cols = 11)
        {
            // On va spawn une pièce random.
            _random = new Random();
            
            if (Config.DifficultyLevel == "Normal")
            {
                _speed -= 100;
            }
            else if (Config.DifficultyLevel == "Hard")
            {
                _speed -= 300;
            }

            _gameTimer = new Timer(_speed);
            _gameTimer.Elapsed += TimerHandler;
            _rows = rows;
            _columns = cols;
            _shapesCount = Enum.GetNames(typeof(TetrominoType)).Length;

            _board = new SquareState[cols, rows];
        }


        /// <summary>
        /// Sert à commencer le jeu.
        /// </summary>
        public void Start()
        {
            if (_state == GameState.Waiting)
            {
                _tetromino = CreateRandomTetro();
                _nextTetromino = CreateRandomTetro();
                _rotatedTetro = new Tetromino(_tetromino.Shape, _tetromino.Position);
                _rotatedTetro.Rotate();

                NextTetroSpawned?.Invoke(this, EventArgs.Empty);

                // Mettre à jour l'état du jeu.
                ChangeState(GameState.Playing);

                // Commencer le timer.
                _gameTimer.Start();
            }
        }


        /// <summary>
        /// Sert à arrêter le jeu.
        /// </summary>
        public void Stop()
        {
            _gameTimer.Stop();
            _state = GameState.Waiting;
        }


        /// <summary>
        /// Cette méthode sert à mettre pause au jeu.
        /// </summary>
        /// <param name="notify">permet de savoir si on veut un message comme quoi on est en pause ou pas</param>
        public void Pause(bool notify = true)
        {
            _gameTimer.Stop();
            if (notify)
            {
                ChangeState(GameState.Paused);
            }
        }


        /// <summary>
        /// Permet de continuer le jeu
        /// </summary>
        /// <param name="notify">permet de savoir si on veut un message comme quoi on est en pause ou pas</param>
        public void Resume(bool notify = true)
        {
            _gameTimer.Start();
            if (notify)
            {
                ChangeState(GameState.Playing);
            }
        }


        /// <summary>
        /// Si le jeu est en cours, on fait pause, et si le jeu est en pause, on le fait tourner
        /// </summary>
        public void PauseOrResume()
        {
            if (_state == GameState.Playing)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }


        /// <summary>
        /// Méthode qui permet de lancer la rotation du Tetromino actuel, puis de mettre à jour le plateau de jeu.
        /// </summary>
        public void Rotate()
        {
            if (_state != GameState.Playing)
            {
                return;
            }

            // Vérifier si on peut effectuer la rotation sans collision avec les murs.
            if (_tetromino.Position.x + _tetromino.Height <= _columns &&
                _tetromino.Position.y + _tetromino.Width <= _rows &&
                !CollideAt(_tetromino.Position, _rotatedTetro))
            {
                _tetromino.Rotate();
                _rotatedTetro.Rotate();
                UpdateBoard();
            }
        }


        /// <summary>
        /// Méthode qui permet de déplacer le Tetromino actuel vers la droite, puis de mettre à jour le plateau de jeu.
        /// </summary>
        public void MoveRight()
        {
            //Si notre Tetromino peut bouger vers la droite, il va le faire
            if (MoveTetromino(Vector2.Right))
            {
                UpdateBoard();
            }
        }


        /// <summary>
        /// Méthode qui permet de déplacer le Tetromino actuel vers la gauche, puis de mettre à jour le plateau de jeu.
        /// </summary>
        public void MoveLeft()
        {
            if (MoveTetromino(Vector2.Left))
            {
                UpdateBoard();
            }
        }


        /// <summary>
        /// Déplacer le tetromino.
        /// </summary>
        /// <param name="direction">La direction de déplacement.</param>
        /// <returns>false si le Tetromino ne peut pas bouger et true si le Tetromino a fait un mouvement</returns>
        private bool MoveTetromino(Vector2 direction)
        {
            Vector2 nextPos = _tetromino.Position + direction;
            if (_state != GameState.Playing || CollideAt(nextPos))
            {
                return false;
            }

            _tetromino.Move(direction);

            return true;
        }


        /// <summary>
        /// Méthode qui permet de faire descendre manuelement le Tetromino vers le bas, puis de mettre à jour le jeu.
        /// </summary>
        public void MoveDown()
        {
            _gameTimer.Stop();
            if (MoveTetromino(Vector2.Up)) // Up parce que +[0,-1] = -[0,1], l'axe Y du console est inversé.
            {
                _score++;
                UpdateBoard();
            }
            _gameTimer.Start();            
        }


        /// <summary>
        /// Méthode qui permet de faire tomber le Tetromino d'un coup. 
        /// 
        /// Dès qu'on presse sur la touche Enter, le Tetromino va descendre sans s'arreter jusqu'à ce qu'il trouve un obstacle, ou qu'il risque de sortir du terrain,
        /// 
        /// S'il touche un obstacle, ça va immédiatement faire spawn un nouveau Tetromino avec la méthode NewTetromino()
        /// </summary>
        public void DropDown()
        {
            if (_state != GameState.Playing)
            {
                return;
            }

            _gameTimer.Stop();

            while (MoveTetromino(Vector2.Up))
            {
                _score++;
                UpdateBoard();
            }

            _gameTimer.Start();

            SpawnNextTetromino();
        }


        /// <summary>
        /// La fonction callback du timer du jeu. Sert à faire descendre le Tetromino à chaque certain nombre de millisecondes.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="e">Les paramètres de l'événement.</param>
        private void TimerHandler(object sender, ElapsedEventArgs e)
        {
            if (MoveTetromino(Vector2.Up))
            {
                UpdateBoard();
            }
            else
            {
                SpawnNextTetromino();
            }
        }


        /// <summary>
        /// Cette méthode va mettre à jour le plateau et informer GameRenderer.
        /// </summary>
        private void UpdateBoard()
        {
            // On va commencer par réinitialiser le tableau de base pour effacer les données obsolètes.
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (_board[i, j] != SquareState.SolidBlock) // On va laisser les Tetrominos qui sont déjà tombés et on va reset le reste.
                    {
                        _board[i, j] = SquareState.Empty;
                    }
                }
            }

            // Par la suite on va implémenter le Tetromino dans le terrain.

            foreach (Vector2 block in _tetromino.Blocks)
            {
                Vector2 pos = block + _tetromino.Position; 
                _board[pos.x, pos.y] = SquareState.MovingBlock;
                //_board[pos.x, pos.y].Color = _tetromino.Color;
            }


            // Tout à la fin on va informer GameRenderer qu'il y a eu un changement dans le tableau.

            TetrominoMoved?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Détecter une collision du tetromino à une position donnée.
        /// </summary>
        /// <param name="tetroPos">La position du tetromino.</param>
        /// <returns>Retourne true s'il y a une collision, sinon retourne false.</returns>
        private bool CollideAt(Vector2 tetroPos)
        {
            foreach (Vector2 bloc in _tetromino.Blocks)
            {   
                // Conversion des coordonées de blocs à des coordonées relatives au plateau
                Vector2 pos = tetroPos + bloc;
                if (!WithinRange(pos) || _board[pos.x, pos.y] == SquareState.SolidBlock)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Détecter une collision d'un tetromino à une position donnée.
        /// </summary>
        /// <param name="tetroPos">La position du tetromino.</param>
        /// <param name="tetro">Le tetromino en question.</param>
        /// <returns>Retourne true s'il y a une collision, sinon retourne false.</returns>
        private bool CollideAt(Vector2 tetroPos, Tetromino tetro)
        {
            foreach (Vector2 bloc in tetro.Blocks)
            {
                // Conversion des coordonées de blocs à des coordonées relatives au plateau
                Vector2 pos = tetroPos + bloc;
                if (!WithinRange(pos) || _board[pos.x, pos.y] == SquareState.SolidBlock)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Vérifier si la position données est dans le terrain de jeu.
        /// </summary>
        /// <param name="pos">La position à vérifier</param>
        /// <returns>Retourne true si la position donnée est dans le monde, retourne false sinon.</returns>
        public bool WithinRange(Vector2 pos)
        {
            return pos.x >= 0 && pos.x < _columns && pos.y >= 0 && pos.y < _rows ;
        }


        /// <summary>
        /// Accelerer le jeu quand on essaie de tricher.
        /// </summary>
        public void SpeedUp()
        {
            _speed = 20;
            UpdateScore();
        }


        /// <summary>
        /// Méthode qui permet d'instancier un nouveau Tetromino.
        /// 
        /// On va commencer par stopper le Tetromino actuel, puis on va instancier le nouveau Tetromino
        /// Il va falloir ensuite transformer l'ancien Tetromino en bloc solide (SquareState.SolidBlock)
        /// </summary>
        private void SpawnNextTetromino()
        {
            _tetromino.State = TetrominoState.Stopped;
            CheckForFullRows();
            UpdateScore();

            // On va spawn une nouvelle pièce random
            _tetromino = _nextTetromino;
            _nextTetromino = CreateRandomTetro();
            _rotatedTetro = new Tetromino(_tetromino.Shape, _tetromino.Position);
            _rotatedTetro.Rotate();

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if (_board[x, y] == SquareState.MovingBlock)
                    {
                        _board[x, y] = SquareState.SolidBlock;
                    }
                }
            }

            CheckForDeath();

            NextTetroSpawned?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Instancier un nouveau tetromino aléatoire.
        /// </summary>
        /// <returns>Retourne un nouveau tetromino.</returns>
        private Tetromino CreateRandomTetro()
        {
            return new Tetromino((TetrominoType)_random.Next(_shapesCount), new Vector2(_columns / 2 - 1, 0));
        }


        /// <summary>
        /// Méthode qui vérifie si une ligne est pleine ou pas.
        /// </summary>
        private void CheckForFullRows()
        {
            for (int y = _tetromino.Position.y; y < _tetromino.Position.y + _tetromino.Height; y++)
            {
                int rowsCount = 0;
                bool isfull = true;
                for (byte x = 0; x < _columns; x++)
                {
                    if (_board[x, y] == SquareState.Empty)
                    {
                        isfull = false;
                        break;
                    }
                }
                if (isfull)
                {
                    DeleteRow(y);
                    rowsCount++;
                }
                UpdateScore(rowsCount);
            }
        }


        /// <summary>
        /// Cette fonction va supprimer la ligne spécifiée.
        /// </summary>
        /// <param name="rowY">Les coordonnées Y de la ligne à supprimer.</param>
        private void DeleteRow(int rowY)
        {
            for (int x = 0; x < _columns; x++)
            {
                _board[x, rowY] = SquareState.Empty;
            }

            for (int y = rowY; y > 0; y--)
            {
                for (int x = 0; x < _columns; x++)
                {
                    _board[x, y] = _board[x, y - 1];
                }
            }
            _rowsCleared++;
            RowCleared?.Invoke(this, rowY);
        }


        /// <summary>
        /// Cette fonction va vérifier si on a perdu ou pas.
        /// </summary>
        private void CheckForDeath()
        {
            if (CollideAt(_tetromino.Position) == true)
            {
                Config.GameScore = Score;
                UpdateBoard();
                ChangeState(GameState.Finished);
            }
        }


        /// <summary>
        /// Changer l'état du jeu.
        /// </summary>
        /// <param name="newState">Le nouvel état.</param>
        private void ChangeState(GameState newState)
        {
            _state = newState;
            StateChanged?.Invoke(this, newState);
        }


        /// <summary>
        /// Méthode qui gère le score et change la vitesse des pièces qui tombent suivant le niveau.
        /// </summary>
        /// <param name="rowsCount">Le nombre de lignes déruites.</param>
        private void UpdateScore(int rowsCount = 0)
        {
            // Calcul des points :
            // Une ligne complète = 40pts
            // Deux = 100 pts
            // Trois = 300pts
            // Quatre = 1200pts

            if (rowsCount == 1)
            {
                _score += 40;
            }
            else if (rowsCount == 2)
            {
                _score += 100;
            }
            else if (rowsCount == 3)
            {
                _score += 300;
            }
            else if (rowsCount == 4)
            {
                _score += 1200;
            }

            // Changement de niveau tout les 1000 points, chute accélérée selon le niveau
            _level = (_score / 1000) + 1;

            _gameTimer.Interval = _speed / (_level * 0.5);

            //GameManager.PlaySound(GameManager.fallSound);                
        }
    }
}
