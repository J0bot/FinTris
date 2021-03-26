///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;
using System.Timers;

namespace FinTris
{
    /// <summary>
    /// Classe Game qui est responsable de toute la logique du jeu
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Cette constante définit la vitesse de chute de notre Tetromino
        /// </summary>
        private int _MS = 500;

        /// <summary>
        /// Attribut _tetromino
        /// </summary>
        private Tetromino _tetromino;

        /// <summary>
        /// Attribut de next Tetromino
        /// </summary>
        private Tetromino _nextTetromino;

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
        /// Attribut de score
        /// </summary>
        private int _score;

        /// <summary>
        /// Attribut de niveau
        /// </summary>
        private int _level;

        /// <summary>
        /// Attribut de l'état du jeu
        /// </summary>
        private GameState _state;
        /// <summary>
        /// Variable random pour executer toutes les fonctions avec random
        /// </summary>
        private Random random;

        /// <summary>
        /// C'est le tableau qui contient les états de tous les blocs du jeu
        /// </summary>
        private readonly Case[,] _board;

        /// <summary>
        /// C'est un événement qui permet de discuter avec GameRenderer pour assuser la synchronisation en l'affichage et la logique du jeu
        /// </summary>
        public event EventHandler<Case[,]> BoardChanged;

        /// <summary>
        /// Evenement qui va nous permettre de lancer tout ce qui concerne les animations de fin de partie
        /// </summary>
        public event EventHandler<bool> IsDed;

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
        /// Nous retourne le niveau dans lequel on est actuellement
        /// </summary>
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Nous retourne notre score
        /// </summary>
        public int Score
        {
            get { return _score; }
            set { _score = value; }
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
        /// Propriété du prochain Tetromino
        /// </summary>
        public Tetromino NextTetromino
        {
            get { return _nextTetromino; }
            set { _nextTetromino = value; }
        }

        /// <summary>
        /// Etat du jeu en GameState
        /// </summary>
        public GameState State
        {
            get { return _state; }
            set { _state = value; }
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
            
            _tetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);
            _nextTetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0, TetrominoState.NextTetromino);

            _gameTimer = new Timer(_MS);
            _gameTimer.Elapsed += timerHandler;
            _rows = rows;
            _cols = cols;

            _board = new Case[cols, rows];

            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = new Case();
                }
            }
        }
        

        /// <summary>
        /// Fonction qui permet de lancer la rotation du Tetromino actuel, puis de update le plateau de jeu
        /// </summary>
        public void Rotate()
        {
            if (_state != GameState.Playing)
            {
                return;
            }

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
            if (_state != GameState.Playing)
            {
                return;
            }

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
            if (_state != GameState.Playing)
            {
                return;
            }

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
            if (_state != GameState.Playing)
            {
                return;
            }

            Vector2 nextPos = _tetromino.Position - Vector2.Down;

            if (!CollideAt(nextPos))
            {
                _gameTimer.Stop();
                _tetromino.Position -= Vector2.Down;

                //Si on accèlère la chute on gagne plus de point
                _score += 10; //Si on presse 1 seconde on a 10 points en plus   

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
            if (_state != GameState.Playing)
            {
                return;
            }


            _gameTimer.Stop();

            Vector2 nextPos = _tetromino.Position - Vector2.Down;
            _score += 20;
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
            //Reset du tableau
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    if (_board[i, j].State != SquareState.SolidBlock) // On va laisser les Tetrominos qui sont déjà tombés et on va reset le reste
                    {
                        _board[i, j].State = SquareState.Empty;
                    }
                }
            }

            // On implémente notre tetromino dans notre board

            foreach (Vector2 block in _tetromino.Blocks)
            {
                Vector2 pos = block + _tetromino.Position; 
                _board[pos.x, pos.y].State = SquareState.MovingBlock;
                _board[pos.x, pos.y].Color = _tetromino.TetrominoColor;
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
                if (!WithinRange(pos) || _board[pos.x,pos.y].State == SquareState.SolidBlock)
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
            CheckForFullRows();
            ScoreManager();
            //On va spawn une nouvelle pièce random

            _tetromino = _nextTetromino;
            _nextTetromino = new Tetromino((TetrominoType)random.Next(7), 3, 0);
            //_tetromino = new Tetromino(TetrominoType.Malong, 3, 0, (ConsoleColor)random.Next(9, 15));

            for (int a = 0; a < _board.GetLength(0); a++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    if (_board[a, j].State == SquareState.MovingBlock)
                    {
                        _board[a, j].State = SquareState.SolidBlock;
                    }
                }
            }
            CheckForDeath();

        }
        

        /// <summary>
        /// Fonction qui va check si une ligne est pleine ou pas
        /// </summary>
        private void CheckForFullRows()
        {
            for (int y = _tetromino.Position.y; y < _tetromino.Position.y + _tetromino.Height; y++)
            {
                int killedRows = 0;
                bool isfull = true;
                for (byte x = 0; x < _cols; x++)
                {
                    if (_board[x, y].State == SquareState.Empty)
                    {
                        isfull = false;
                    }
                }
                if (isfull)
                {
                    DeleteRow(y);
                    killedRows++;
                    //BoardChanged.Invoke(this, _board);
                    //System.Threading.Thread.Sleep(1000);
                }
                ScoreManager(killedRows);
            }
        }

        /// <summary>
        /// Cette fonction va delete la ligne spécifiée
        /// </summary>
        /// <param name="fullY">Ligne à supprimer</param>
        private void DeleteRow(int fullY)
        {
            for (int x = 0; x < _cols; x++)
            {
                _board[x, fullY].State = SquareState.Empty;
                _board[x, fullY].Color = ConsoleColor.White;
            }

            for (int x = 0; x < _cols; x++)
            {
                for (int y = fullY; y > 0; y--)
                {
                    _board[x, y] = _board[x, y - 1];
                }
            }

        }
        

        /// <summary>
        /// Cette fonction va vérifier si on a perdu ou pas
        /// </summary>
        private void CheckForDeath()
        {
            if (CollideAt(_tetromino.Position) == true)
            {
                IsDed.Invoke(this, true);
                UpdateBoard(); //je suis désolé Ahmad
                _state = GameState.Finished;
            }
        }


        /// <summary>
        /// Fonction qui gère le score et //Changement de la vitesse des pièces qui tombent suivant le niveau
        /// </summary>
        /// <param name="nbrKill">nombre de lignes déruites</param>
        private void ScoreManager(int nbrKill = 0)
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

        /// <summary>
        /// Sert à start le timer
        /// </summary>
        public void Start()
        {
            _state = GameState.Playing;
            _gameTimer.Start();
            
        }

        /// <summary>
        /// Sert à Stop le timer
        /// </summary>
        public void Stop()
        {
            _gameTimer.Stop();
            
        }

        public void Pause()
        {
            _gameTimer.Enabled = !_gameTimer.Enabled;
            _state = _gameTimer.Enabled ? GameState.Playing : GameState.Paused;
        }
        
    }
}
