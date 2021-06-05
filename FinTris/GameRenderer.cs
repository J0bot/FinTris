/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using System.Media;
using System.Threading;

namespace FinTris
{
    /// <summary>
    /// Classe qui s'occupe d'afficher le jeu.
    /// </summary>
    public class GameRenderer
    {        
        // Attributs.
        private readonly Game _game;
        private readonly Vector2 _position;
        private readonly Vector2 _innerPos;
        private readonly Vector2 _nextTetroPos;
        private readonly Vector2 _pausedPos;

        /// <summary>
        /// La matrice scale du console.
        /// </summary>
        private static readonly Vector2 MAT_SCALE = new Vector2(2, 1);

        /// <summary>
        /// Constructor renseigné de la classe GameRenderer.
        /// </summary>
        /// <param name="game">La référence de l'instance de Game. Sert à récupérer certaines informations importantes par rapport à l'affichage.</param>
        public GameRenderer(Game game)
        {
            _game = game;
            _nextTetroPos = new Vector2(60, 4);
            _pausedPos = new Vector2(60, 12);
            _position = new Vector2(30, 2);
            _innerPos = _position + MAT_SCALE;

            _game.TetrominoMoved += OnTetrominoMoved;
            _game.StateChanged += OnGameStateChanged;
            _game.NextTetroSpawned += OnNextTetroSpawned;
            _game.RowCleared += OnRowCleared;

            Console.Clear();

            RenderGameBorder();
            RenderScore();
        }

        private void OnNextTetroSpawned(object sender, EventArgs e)
        {
            lock (this)
            {
                RenderTetromino();
                RenderNextTetro();
            }
        }

        /// <summary>
        /// Fonction qui se déclenche quand il y a un changememt dans le plateau du jeu.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="board">Le plateau du jeu contenant les état de chaque case.</param>
        private void OnTetrominoMoved(object sender, EventArgs e)
        {
            lock (this)
            {
                // Mettre à jour l'affichage du tetromino.
                UpdateTetromino();
                RenderScore();
            }
        }

        /// <summary>
        /// Cette méthode s'occupe d'afficher le plateau du jeu en passant le tableau des états des cases en paramètre.
        /// </summary>
        /// <param name="board">Le tableau contenant les informations des cases.</param>
        private void UpdateTetromino()
        {
            for (int dy = -1; dy < 4 + 1; dy++) // 4 c'est la longueur maximal d'un tetromino.
            {
                for (int dx = -1; dx < 4 + 1; dx++)
                {
                    Vector2 posRelative = new Vector2(dx, dy);
                    Vector2 pos = _game.CurrentTetromino.PreviousPosition + posRelative;

                    if (!_game.WithinRange(pos))
                    {
                        continue;
                    }

                    UpdateTile(pos);
                }
            }

            Console.ResetColor();
        }

        private void OnRowCleared(object sender, int rowY)
        {
            lock (this)
            {
                Vector2 from = _innerPos;
                Vector2 size = new Vector2(_game.Columns * MAT_SCALE.x, rowY);
                Vector2 to = from + Vector2.Up;
                Console.MoveBufferArea(
                    from.x, from.y,
                    size.x, size.y,
                    to.x, to.y
                );
            }
            
        }

        /// <summary>
        /// Permet de mettre à jour une case.
        /// </summary>
        /// <param name="position">position de la case à dessiner en Vector2</param>
        private void UpdateTile(Vector2 position)
        {
            SquareState state = _game.Board[position.x, position.y];
            if (state == SquareState.MovingBlock)
            {
                DrawTile(position, _innerPos, _game.CurrentTetromino.Color);
            }
            else if (state == SquareState.Empty)
            {
                DrawTile(position, _innerPos, Console.BackgroundColor);
            }
        }

        /// <summary>
        /// Permet de dessiner une case du jeu 
        /// </summary>
        /// <param name="position">position de la case à dessiner en Vector2</param>
        /// <param name="shift">permet d'ajouter un décalement x et y en Vector2</param>
        /// <param name="color">permet d'implémenter la couleur à notre Tile en ConsoleColor</param>
        private void DrawTile(Vector2 position, Vector2 shift , ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(shift.x + (position.x * MAT_SCALE.x), shift.y + position.y);
            Console.Write("██");
        }

        /// <summary>
        /// Permet de dessiner un rectangle
        /// </summary>
        /// <param name="position">position en Vector2 de notre objet</param>
        /// <param name="width">largeur du border</param>
        /// <param name="height">hauteur du border </param>
        /// <param name="color">couleur en ConsoleColor du border</param>
        private void DrawBorder(Vector2 position, int width, int height, ConsoleColor color)
        {
            for (int j = 0; j < height; j++)
            {
                bool middle = j > 0 && j < height - 1;
                for (int i = 0; i < (middle ? 2 : width); i++)
                {
                    i = middle && i > 0 ? width - 1 : i;
                    Vector2 pos = new Vector2(i, j);
                    DrawTile(pos, position, color);
                }
            }
        }

        /// <summary>
        /// Permet de créer la bordure du jeu.
        /// </summary>
        public void RenderGameBorder()
        {
            int width = (_game.Columns + 2) * MAT_SCALE.x;
            string line = new string('█', width);
            string middle = "██" + new string(' ', 22) + "██";

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(_position.x, _position.y);
            Console.Write(line);

            for (int y = 0; y < _game.Rows; y++)
            {
                Console.SetCursorPosition(_position.x, _position.y + y + 1);
                Console.Write(middle);
            }

            Console.SetCursorPosition(_position.x, _position.y + _game.Rows + 1);
            Console.Write(line);
            Console.ResetColor();            

            // Prochain Tetromino.
            Console.SetCursorPosition(_nextTetroPos.x, _nextTetroPos.y - 2);
            Console.Write("NEXT TETROMINO");
            Console.ResetColor();
        }

        /// <summary>
        /// Fonction qui s'occupe de dessiner le score.
        /// </summary>
        private void RenderScore()
        {
            // Affichage du score.
            Console.SetCursorPosition(60, 15);
            Console.WriteLine($"Score : {_game.Score} pts");

            // Affichage des lignes supprimées.
            Console.SetCursorPosition(60, 17);
            Console.WriteLine($"Lignes : {_game.RowsCleared}");

            // Affichage du niveau.
            Console.SetCursorPosition(60, 19);
            Console.WriteLine($"Niveau : {_game.Level}");
        }

        /// <summary>
        /// La fonction callback de l'événement IsDead. Déclenché par la Game quand le jeu est terminé.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="e">If set to <c>true</c> e.</param>
        private void OnGameStateChanged(object sender, GameState newState)
        {
            if (newState == GameState.Finished)
            {
                lock (this)
                {
                    DeathAnim();
                }
                GameManager.Play();
            }
            else if (newState == GameState.Paused)
            {
                WriteAt(Resources.pause_text, _pausedPos.x, _pausedPos.y);
            }
            else if (newState == GameState.Playing)
            {
                // Effacer le texte de pause lorsque l'on reprend le jeu.
                WriteAt(new string('\0', Resources.pause_text.Length), _pausedPos.x, _pausedPos.y);
            }
        }

        /// <summary>
        /// Animation quand le jeu finit qui permet de remplir l'écran avec des blocs.
        /// </summary>
        public void DeathAnim()
        {
            Config.SaveScore();
            _game.Stop();

            SoundPlayer koSound = new SoundPlayer(Resources.TetrisSoundKo);
            koSound.Play();

            Console.ForegroundColor = ConsoleColor.Blue;
            for (int y = _game.Rows - 1; y >= 0; y--)
            {
                for (int x = _game.Columns - 1; x >= 0; x--)
                {
                    WriteAt("██", x * 2 + _position.x + 2, y + _position.y + 1);
                }
                Thread.Sleep(100);
            }
                
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int y = _game.Rows; y > 0; y--)
            {
                for (int x = 0; x < _game.Columns; x++)
                {
                    WriteAt("██", x * 2 + _position.x + 2, y + _position.y);
                }
                Thread.Sleep(10);
            }

            Thread.Sleep(1200);

            Console.ResetColor();

            RenderGameBorder();

            int cursorX = _position.x + _game.Columns / 2;
            int cursorY = _position.y + _game.Rows / 4;

            foreach (string line in Resources.game_over.Split('\n'))
            {
                WriteAt(line, cursorX, ++cursorY);
            }

            cursorY += 5;
            cursorX += 2;

            foreach (string line in Resources.try_again.Split('\n'))
            {
                WriteAt(line, cursorX, cursorY);
                cursorX += 2;
                cursorY += 1;
            }

            Thread.Sleep(1500);
        }

        /// <summary>
        /// Fonction qui va s'occuper d'afficher le prochain Tetromino.
        /// </summary>
        private void RenderNextTetro()
        {
            int max = 10; // 10 parceque le tetromino le plus large est de 6 caractères + 4 de bordures.
            int width = _game.NextTetromino.Width + 4;
            int height = _game.NextTetromino.Height + 4;

            // Effacer l'ancienne représentation du next tetromino.
            for (int x = 0; x < max; x++)
            {
                for (int y = 0; y < max; y++)
                {
                    Console.SetCursorPosition(_nextTetroPos.x + x * 2, _nextTetroPos.y + y);
                    Console.Write("  ");
                }
            }

            // Dessiner la bordure.
            DrawBorder(_nextTetroPos, width, height, ConsoleColor.Red);

            // Dessiner la forme du next tetromino.
            foreach (Vector2 posBlock in _game.NextTetromino.Blocks)
            {
                Vector2 pos = posBlock + (Vector2.One * 2);
                DrawTile(pos, _nextTetroPos, _game.NextTetromino.Color);
            }

            Console.ResetColor();
        }

        private void RenderTetromino()
        {
            foreach (Vector2 blockPos in _game.CurrentTetromino.Blocks)
            {
                Vector2 pos = _game.CurrentTetromino.Position + blockPos;
                DrawTile(pos, _innerPos, _game.CurrentTetromino.Color);
            }
        }

        /// <summary>
        /// Si le joueur appuie sur A, il entre dans une zone interdite.
        /// </summary>
        public void CheatCode()
        {
            // Lancement de la première voix.
            SoundPlayer bowserSound2 = new SoundPlayer(Resources.bowserSound2);

            if (!GameManager.Muted)
            {
                bowserSound2.Play();
            }
            _game.Pause();
            Console.Clear();
            Console.SetCursorPosition(50, 14);
            TypewriterEffect("??? : Tricheur !");
            Thread.Sleep(200);

            Console.SetCursorPosition(35, 16);
            TypewriterEffect("??? : Tu ne devais pas avoir accès à cette zone !");

            Thread.Sleep(200);

            Console.SetCursorPosition(39, 18);
            TypewriterEffect("??? : Maintenant il va falloir...");
            Thread.Sleep(100);
            TypewriterEffect(" payer !", 100);
            Thread.Sleep(1000);
            Console.Clear();

            // Lancement de la deuxième voix.
            SoundPlayer bowserSound = new SoundPlayer(Resources.bowserSound);

            if (!GameManager.Muted)
            {
                bowserSound.Play();
            }         

            string[] bowserString = new string[] {
                "                                   @                                  ",
                "                                 @@@@@                                ",
                "                          *@@@&@@@@@@@@##@@@@                         ",
                "                         @@@@@@@@@@@@@@@@@@@@@.                       ",
                "         @             @@@@@@@@@@@@@@@@@@@@@@@@@            @@        ",
                "       @@@@           @@@@@@@@@@@@@@@@@@@@@@@@@@@@          @@@@      ",
                "     /@@@@@%        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@       @@@@@@     ",
                "     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@      ",
                "          @@@@@@@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@@@@@@         ",
                "          @@@@@@@@       &@@@@@@@@@@@@@@@@@@@@.      &@@@@@@@         ",
                "       #@@@@@@@@@@,         @@@@@@@@@@@@@@@          @@@@@@@@@@,      ",
                "    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   ",
                "  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ",
                " @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                ".@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                " @@@@@@@@@@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@@@@@@@@@",
                " @@@@@@@@@@@@       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@       ,@@@@@@@@@@@",
                "  @@@@@@@@@@@*         @@@@@@,           ,@@@@@@          @@@@@@@@@@@ ",
                "   @@@@@@@@@@@           @@                 @@.          @@@@@@@@@@@  ",
                "     @@@@@@@@@@           @                 @           @@@@@@@@@@    ",
                "        @@@@@@@@                                       @@@@@@@@       ",
                "          @@@@@@@                                     @@@@@@@         ",
                "            @@@@@@                                   @@@@@@           ",
                "              @@@@@@     @@@              ,@@@      @@@@@.            ",
                "               @@@@@@  @@@@@@@@@@@@@@@@@@@@@@@@@  @@@@@@              ",
                "                 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                ",
                "                 (@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@(                ",
                "                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                 ",
                "                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                  ",
                "                    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   ",
                "                      &@@@@@@@@        #@@@@@@@@                      "
            };

            // Affichage du monstre.
            // 
            for (int i = 0; i < 5; i++)
            {

                Console.ForegroundColor = ConsoleColor.Red;

                //string[] bowser = File.ReadAllLines(Resources.Bowser);

                //for (int w = 0; w < bowser.Length; w++)
                //{
                //    Console.WriteLine(bowser[w]);
                //}
                for (int j = 0; j < bowserString.Length; j++)
                {
                    Console.WriteLine(bowserString[j]);
                }


                Thread.Sleep(100);
                Console.Clear();
                Thread.Sleep(100);

            }
            Console.ResetColor();
            
            //ResetRender();
            _game.Resume();
        }

        /// <summary>
        /// Ecris ue texte à une position donnée. (repris de la documentation de Microsoft)
        /// </summary>
        /// <param name="s">Le texte à afficher.</param>
        /// <param name="x">La position X.</param>
        /// <param name="y">La position Y.</param>
        public static void WriteAt(string s, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(s);
        }

        /// <summary>
        /// Effect de machine à ecrire.
        /// </summary>
        /// <param name="text">Le texte à afficher.</param>
        /// <param name="time">Le temps de l'affichage.</param>
        public void TypewriterEffect(string text, int time = 50)
        {
            // On va parcourir le string et écrire lettre par lettre.
            for (int i = 0; i < text.Length; i++) 
            {
                Console.Write(text.Substring(i, 1));
                Thread.Sleep(time);
            }
        }

    }
}
