using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace FinTris
{
    public static class Config
    {
        /// <summary>
        /// Le score du jeu. Utilisé dans la classe "Game".
        /// </summary>
        static private int _gameScore;

        /// <summary>
        /// L'emplacement du fichier de configuration, où est gardé le nom du joueur actuel ainsi que les scores
        /// de tous les autres joueurs.
        /// </summary>
        static string _configLocation = "./config.cfg";
            
        /// <summary>
        /// La liste qui servira à stocker le fichier de configuration, une ligne à la fois
        /// </summary>
        static List<string> _configFile = new List<string>();

        /// <summary>
        /// Constructeur servant à initialiser la classe "Config".
        /// </summary>
        static Config()
        {
            CheckIfFileExists();

            //ajouter au string temporaire configStream le contenu du fichier de configuration.
            string configStream = File.ReadAllText(_configLocation);
            //ajouter à la liste "_configFile" le contenu de configSteam, coupé à chaque fin de ligne.
            _configFile.AddRange(configStream.Split('\n'));

            Debug.WriteLine("config loaded");
        }

        /// <summary>
        /// Le string d'erreur qui est retourné si aucun résultat n'a été trouvé dans le fichier de configuration.
        /// </summary>
        static readonly string _errorString = "-111111111";

        /// <summary>
        /// Retourne ou assigne le nom du joueur actuel.
        /// </summary>
        /// <value>Le nouveau nom du joueur.</value>
        public static string PlayerName
        {
            get { return ParseConfig("PlayerName"); }
            set { UpdateConfig("PlayerName", value, false); }
        }

        /// <summary>
        /// Retourne ou assigne le niveau de difficulté.
        /// </summary>
        /// <value>Le niveau de difficulté.</value>
        public static string DifficultyLevel
        {
            get { return ParseConfig("Difficulty"); }
            set { UpdateConfig("Difficulty", value, false); }
        }

        /// <summary>
        /// Retourne le score. Ceci est utilisé afin de savoir si le score actuel est
        /// plus élevé que celui stocké dans le fichier de configuration.
        /// </summary>
        /// <value>The nouveau score.</value>
        public static int GameScore
        {
            get { return _gameScore; }
            set { _gameScore = value; }
        }

        /// <summary>
        /// Méthode qui cherche dans le fichier de configuration le patterne (clé) qui lui a été passé.
        /// </summary>
        /// <returns>La valeur associée à la clé.</returns>
        /// <param name="pattern">La clé que nous voulons chercher.</param>
        public static string ParseConfig(string pattern)
        {
            CheckIfFileExists();
            string line;

            //regex servant à chercher la clé et la valeur qui la suit
            Regex patternFinder = new Regex("^" + pattern + "=.*");

            for (int i = 0; i < _configFile.Count; i++)
            {
                line = _configFile[i];
                //la variable "result" contiendra statut de la recherche.
                Match result = patternFinder.Match(line);
                if (result.Success)
                {
                    //si la clé a été trouvé, le match est coupé en 2 au niveau du signe "="
                    //et la valeur de la clé est retournée.
                    Debug.WriteLine(result);
                    string[] splittedResult = result.ToString().Split('=');
                    return splittedResult[1];
                }
            }
            //si on arrive là, cela veut dire que la clé n'existe pas.
            return _errorString;
        }

        /// <summary>
        /// Cherche dans le fichier de configuration une clé et remplace la valeur qui lui est associée.
        /// </summary>
        /// <param name="pattern">La clé que nous voulons chercher.</param>
        /// <param name="newValue">La nouvelle valeur à inscrire.</param>
        /// <param name="append">Si vrai, la clé passée ainsi que sa valeur seront ajoutés à la fin du
        /// fichier, utile si nous voulons ajouter une nouvelle clé au fichier de configuration.</param>
        public static void UpdateConfig(string pattern, string newValue, bool append)
        {
            CheckIfFileExists();

            //Si nous savons que la clé n'existe pas, nous pouvons simplement l'ajouter en fin de fichier ici et ne
            //pas aller plus loin.
            if (append)
            {
                _configFile.Add(pattern + "=" + newValue);
                File.WriteAllText(_configLocation, String.Join("\n", _configFile));

                return;
            }

            //Ici, nous effectuons presque le même code que dans la méthode "ParseConfig". Si le temps le permet, il serait
            //bien de créer une troisième méthode commune à ces deux-ci afin d'éviter de répéter le code.

            string line;
            //regex servant à chercher la clé et la valeur qui la suit
            Regex patternFinder = new Regex("^" + pattern + "=.*");

            for (int i = 0; i < _configFile.Count; i++)
            {
                line = _configFile[i];
                //la variable "result" contiendra statut de la recherche.
                Match result = patternFinder.Match(line);
                if (result.Success)
                {
                    //si la clé a été trouvé, le match est coupé en 2 au niveau du signe "="
                    //et la valeur de la clé est retournée.
                    Debug.WriteLine("MATCH = " + result);
                    _configFile[i] = pattern + "=" + newValue;
                    Debug.WriteLine("NEW VALUE = " + pattern + "=" + newValue);
                    Debug.WriteLine("\n" + line);
                }
            }
            //Écrasement du fichier de configuration avec le nouveau contenu.
            File.WriteAllText(_configLocation, String.Join("\n", _configFile));
        }

        /// <summary>
        /// Méthode qui vérifie si le fichier de configuration existe. Si ce n'est pas le cas, cette méthode
        /// le créera et insérera les paramètres de base à l'intérieur.
        /// </summary>
        private static void CheckIfFileExists()
        {
            if (!File.Exists(_configLocation))
            {
                File.WriteAllText(_configLocation,
                    "PlayerName=Default\n" +
                    "Difficulty=Normal\n");
            }
        }

        /// <summary>
        /// Cette méthode enregistre le score du joueur actuel en fin de partie si celui-ci est plus haut que
        /// son record précédent. Si le joueur est nouveau, le score sera ajouté à la fin du fichier de configuration.
        /// </summary>
        public static void SaveScore()
        {
            Debug.WriteLine("SCORE: " + PlayerName + " " + ParseConfig(PlayerName + "_MaxScore"));
            // TODO: Mais que ce passe-t'il si le joueur a le même nom que le string d'erreur? Il faudrait trouver un moyent
            //plus propre de vérifier si le joueur existe ou non dans le fichier de configuration!
            if (ParseConfig(PlayerName + "_MaxScore") == _errorString)
            {
                UpdateConfig(PlayerName + "_MaxScore", GameScore.ToString(), true);
            }
            else if (Convert.ToInt32(ParseConfig(PlayerName + "_MaxScore")) < GameScore)
            {
                UpdateConfig(PlayerName + "_MaxScore", GameScore.ToString(), false);
            }
        }


        /// <summary>
        /// Cette méthode retourne une liste de 5 tableaux contenant le nom des meilleurs joueurs ainsi que leur
        /// meilleur score, arrangé du meilleur au plus mauvais.
        /// </summary>
        /// <returns>Une liste de tableau de strings.</returns>
        public static List<string[]> GetBestScores()
        {
            List<string[]> maxScores = new List<string[]>();
            //regex pour cherche toutes les clefs contenant un meilleur score (example: Yannick_MaxScore=1432)
            Regex scorePattern = new Regex(".*_MaxScore=.*");
            for (int i = 0; i < _configFile.Count; i++)
            {
                string line = _configFile[i];
                //Essait de matcher le regex à la ligne actuelle
                Match result = scorePattern.Match(line);

                if (result.Success)
                {
                    //Ajoute le nom du joueur dans la première case du tableau et so score dans la seconde
                    string[] splittedResult = result.Groups[0].ToString().Split('=');
                    string[] entry = new string[2];
                    entry[0] = splittedResult[0].Remove(splittedResult[0].LastIndexOf('_'));
                    entry[1] = splittedResult[1];
                    maxScores.Add(entry);
                }
            }
            //Arrange la liste de tableau du meilleur score au plus mauvait
            //what did I just do, how, why does it work, I have SO many questions
            maxScores = maxScores.OrderBy(arr => Convert.ToInt32(arr[1])).ToList();
            maxScores.Reverse();
            //Ne garde que les 5 meilleurs joueurs
            if (maxScores.Count > 5)
            {
                maxScores.RemoveRange(5, maxScores.Count - 5);
            }

            return maxScores;
        }
    }
}
