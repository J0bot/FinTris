using System;
namespace FinTris
{
    public static class Config
    {
        static string _playerName = "defaultPlayer";

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>The name of the player.</value>
        public static string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }
    }
}
