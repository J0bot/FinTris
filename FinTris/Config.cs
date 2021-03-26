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
        //is this valid in .NET framework 1.6?
        static private string _playerName;
        static private string _difficulty;
        static private int _gameScore;

        /// <summary>
        /// The config location.
        /// </summary>
        static string _configLocation = "./config.cfg";

        //reads the config file and stores in in an array
        static string _configSteam = File.ReadAllText(_configLocation);

        static List<string> _configFile = new List<string>();

        /// <summary>
        /// Initializes the Config class.
        /// </summary>
        static Config()
        {
            Debug.WriteLine("config loaded");
            _configFile.AddRange(_configSteam.Split('\n'));
        }

        static Regex _optionFinder = new Regex(".*=.*");

        static readonly string _errorString = "NOTFOUND";

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>The name of the player.</value>
        public static string PlayerName
        {
            get { return ParseConfig("PlayerName"); }
            set
            {
                _playerName = value;
                UpdateConfig("PlayerName", _playerName, false);
            }
        }

        public static int GameScore
        {
            get { return _gameScore; }
            set { _gameScore = value; }
        }

        /// <summary>
        /// Gets or sets the difficulty level.
        /// </summary>
        /// <value>The difficulty level.</value>
        public static string DifficultyLevel
        {
            get { return ParseConfig("Difficulty"); }
            set
            {
                _difficulty = value;
                UpdateConfig("Difficulty", _difficulty, false);
            }
        }

        /// <summary>
        /// Parses the config for a pattern that was given to it.
        /// </summary>
        /// <returns>The config.</returns>
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
        /// Updates the config if there is a need to
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="append">If set to <c>true</c> append instead of searching for the pattern.</param>
        public static void UpdateConfig(string pattern, string newValue, bool append)
        {
            CheckIfFileExists();

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
            File.WriteAllText(_configLocation, String.Join("\n", _configFile));
        }

        private static void CheckIfFileExists()
        {
            if (!File.Exists(_configLocation))
            {
                File.Create(_configLocation);
            }
        }

        /// <summary>
        /// Saves the score if the current player if it's higher than their previous best. Add it anyway
        /// if the player doesn't already have a best score.
        /// </summary>
        public static void SaveScore()
        {
            Debug.WriteLine("SCORE: " + PlayerName + " " + ParseConfig(PlayerName + "_MaxScore"));
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
        /// returns a list of 5 arrays containing the name of the best players and its best score, sorted
        /// from the best to the lowest score.
        /// </summary>
        /// <returns>The scores.</returns>
        public static List<string[]> GetScores()
        {
            List<string[]> maxScores = new List<string[]>();
            Regex scorePattern = new Regex(".*_MaxScore=.*");
            for (int i = 0; i < _configFile.Count; i++)
            {
                string line = _configFile[i];
                Match result = scorePattern.Match(line);

                if (result.Success)
                {
                    string[] splittedResult = result.Groups[0].ToString().Split('=');
                    string[] entry = new string[2];
                    entry[0] = splittedResult[0].Remove(splittedResult[0].LastIndexOf('_'));
                    entry[1] = splittedResult[1];
                    maxScores.Add(entry);
                }
            }
            //what did I just do, how, why does it work, I have SO many questions
            maxScores = maxScores.OrderBy(arr => Convert.ToInt32(arr[1])).ToList();
            maxScores.Reverse();
            maxScores.RemoveRange(5, maxScores.Count - 5);

            return maxScores;
        }
    }
}
