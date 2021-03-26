using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
namespace FinTris
{
    public static class Config
    {
        static string _playerName = "defaultPlayer";

        /// <summary>
        /// The config location.
        /// </summary>
        static string _configLocation = "./config.cfg";

        //reads the config file and stores in in an array
        static string _configSteam = File.ReadAllText(_configLocation);
        static string[] _configFile = _configSteam.Split('\n');

        static Regex _optionFinder = new Regex(".*=.*");

        static readonly string errorString = "NOTFOUND";

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
                UpdateConfig("PlayerName", _playerName);
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

            for (int i = 0; i < _configFile.Length; i++)
            {
                line = _configFile[i];
                var result = patternFinder.Match(line);
                if (result.Success)
                {
                    Debug.WriteLine(result);
                    string[] splittedResult = result.ToString().Split('=');
                    return splittedResult[1];
                }
            }
            return errorString;
        }

        /// <summary>
        /// Checks if the configuration file contains old values and updates
        /// them if there is the need to
        /// </summary>
        public static void UpdateConfig(string pattern, string newValue)
        {
            CheckIfFileExists();
            string line;

            //regex to find the pattern and the value after it
            Regex patternFinder = new Regex("^" + pattern + "=.*");

            for (int i = 0; i < _configFile.Length; i++)
            {
                line = _configFile[i];
                var result = patternFinder.Match(line);
                if (result.Success)
                {
                    //IT DOESN'T WORK!!!! WHAT GIVES YOU FUCKING PIECE OF SHIT
                    //it's supposed to replace the current line with the new parameter,
                    //the write everything to the config file. But, it's not updating whyyyyyy
                    Debug.WriteLine("MATCH = " + result);
                    line = result + pattern + "=" + newValue;
                    Debug.WriteLine("NEW VALUE = " + pattern + "=" + newValue);
                    Debug.WriteLine("\n" + line);
                }
            }
            File.WriteAllText(_configLocation, String.Join("", _configFile));
        }

        private static void CheckIfFileExists()
        {
            if (!File.Exists(_configLocation))
            {
                File.Create(_configLocation);
            }
        }
    }
}
