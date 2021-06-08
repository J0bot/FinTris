using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FinTris
{
    /// <summary>
    /// Classe de config, permet de garder des informations dans un fichier
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The game score. (used in the Game class.)
        /// </summary>
        static private int _gameScore;

        /// <summary>
        /// The config location.
        /// </summary>
        static readonly string _configLocation = "./config.cfg";

        /// <summary>
        /// string to temporarily store all the content of the configuration file.
        /// </summary>
        static readonly string  _configStream;

        /// <summary>
        /// The list that will store the content of the configuration file, line by line
        /// </summary>
        static readonly List<string> _configFile = new List<string>();

        /// <summary>
        /// Initializes the Config class.
        /// </summary>
        static Config()
        {
            CheckIfFileExists();
            //add to the _configFile list the content of the configStream string,
            //splitted at every line ending.
            _configStream = File.ReadAllText(_configLocation);
            _configFile.AddRange(_configStream.Split('\n'));

            Debug.WriteLine("config loaded");
        }

        /// <summary>
        /// The error string that is used if no result was found from parsing the config file.
        /// </summary>
        static readonly string _errorString = "NOTFOUND";

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>The name of the player.</value>
        public static string PlayerName
        {
            get { return ParseConfig("PlayerName"); }
            set { UpdateConfig("PlayerName", value, false); }
        }

        /// <summary>
        /// Gets or sets the difficulty level.
        /// </summary>
        /// <value>The difficulty level.</value>
        public static string DifficultyLevel
        {
            get { return ParseConfig("Difficulty"); }
            set { UpdateConfig("Difficulty", value, false); }
        }

        /// <summary>
        /// Gets or sets the game score. This is used to compare if
        /// the current score is higher than the one found in the config file.
        /// </summary>
        /// <value>The game score.</value>
        public static int GameScore
        {
            get { return _gameScore; }
            set { _gameScore = value; }
        }

        /// <summary>
        /// Parses the config for a pattern that was given to it.
        /// </summary>
        /// <returns>The value associated with the pattern.</returns>
        /// <param name="pattern">The pattern we want to search.</param>
        public static string ParseConfig(string pattern)
        {
            CheckIfFileExists();
            string line;

            //regex to find the pattern and the value after it
            Regex patternFinder = new Regex("^" + pattern + "=.*");

            for (int i = 0; i < _configFile.Count; i++)
            {
                line = _configFile[i];
                Match result = patternFinder.Match(line);
                if (result.Success)
                {
                    Debug.WriteLine(result);
                    string[] splittedResult = result.ToString().Split('=');
                    return splittedResult[1];
                }
            }
            return _errorString;
        }

        /// <summary>
        /// Parse the config file for a pattern and replace the value associated to it. Maybe there's a way to
        /// use the ParseConfig method instead of parsing it another time here?
        /// </summary>
        /// <param name="pattern">The pattern we want to search</param>
        /// <param name="newValue">The new value to the pattern.</param>
        /// <param name="append">If set to <c>true</c>, append to the config file instead of searching for
        /// the pattern.</param>
        public static void UpdateConfig(string pattern, string newValue, bool append)
        {
            CheckIfFileExists();

            //if we know the parameter doesn't exist, we can simply append it to the end here
            //and not go any further
            if (append)
            {
                _configFile.Add(pattern + "=" + newValue);
                File.WriteAllText(_configLocation, String.Join("\n", _configFile));

                return;
            }

            string line;
            //regex to find the pattern and the value after it
            Regex patternFinder = new Regex("^" + pattern + "=.*");

            for (int i = 0; i < _configFile.Count; i++)
            {
                line = _configFile[i];
                Match result = patternFinder.Match(line);
                if (result.Success)
                {
                    //replace the current line with the new parameter,
                    //then write everything to the config file.
                    Debug.WriteLine("MATCH = " + result);
                    _configFile[i] = pattern + "=" + newValue;
                    Debug.WriteLine("NEW VALUE = " + pattern + "=" + newValue);
                    Debug.WriteLine("\n" + line);
                }
            }
            //write everything back to the config file.
            File.WriteAllText(_configLocation, String.Join("\n", _configFile));
        }

        /// <summary>
        /// Checks if the config file exists.
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
        /// Saves the score if the current player if it's higher than their previous best. Add it anyway
        /// if the player doesn't already have a best score (new player).
        /// </summary>
        public static void SaveScore()
        {
            Debug.WriteLine("SCORE: " + PlayerName + " " + ParseConfig(PlayerName + "_MaxScore"));
            if (ParseConfig(PlayerName + "_MaxScore") == _errorString)//but what if the player has the same name as the error string? :(
            {
                UpdateConfig(PlayerName + "_MaxScore", GameScore.ToString(), true);
            }
            else if (Convert.ToInt32(ParseConfig(PlayerName + "_MaxScore")) < GameScore)
            {
                UpdateConfig(PlayerName + "_MaxScore", GameScore.ToString(), false);
            }
        }


        /// <summary>
        /// returns a list of 5 arrays containing the name of the best players and their best score, sorted
        /// from the best to the lowest score.
        /// </summary>
        /// <returns>A list of string arrays.</returns>
        public static List<string[]> GetBestScores()
        {
            List<string[]> maxScores = new List<string[]>();
            //regex to search every entry referencing a best score (example: Yannick_MaxScore=1432)
            Regex scorePattern = new Regex(".*_MaxScore=.*");
            for (int i = 0; i < _configFile.Count; i++)
            {
                string line = _configFile[i];
                //try to match the regex to the current line
                Match result = scorePattern.Match(line);

                //if it succeeds...
                if (result.Success)
                {
                    //add the name of the player in the first case, its score in the second
                    string[] splittedResult = result.Groups[0].ToString().Split('=');
                    string[] entry = new string[2];
                    entry[0] = splittedResult[0].Remove(splittedResult[0].LastIndexOf('_'));
                    entry[1] = splittedResult[1];
                    maxScores.Add(entry);
                }
            }
            //orders the list of arrays from the higher score to the lowest
            //what did I just do, how, why does it work, I have SO many questions
            maxScores = maxScores.OrderBy(arr => Convert.ToInt32(arr[1])).ToList();
            maxScores.Reverse();
            //only keep the 5 best players
            if (maxScores.Count > 5)
            {
                maxScores.RemoveRange(5, maxScores.Count - 5);
            }
            return maxScores;
        }
    }
}
