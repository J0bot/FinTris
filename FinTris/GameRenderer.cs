using System.Timers;

namespace FinTris
{
    public class GameRenderer
    {
        public const int MS = 500;
        private Timer _gameTimer;

        public Timer GameTimer
        {
            get { return _gameTimer; }
            set { _gameTimer = value; }
        }

        public GameRenderer()
        {
            _gameTimer = new Timer(MS);
            _gameTimer.Elapsed += timerHandler;
        }

        private void timerHandler(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {

        }
    }
}
