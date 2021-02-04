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
            RotationState rot = RotationState.Rotation1;

            int startX = 0;
            int startY = 0;

            int endX = 0;
            int endY = 0;

            switch (rot)
            {
                case RotationState.Rotation0:
                    startX = 0;
                    startY = 0;
                    break;
                case RotationState.Rotation1:
                    startX = 3;
                    startY = 0;
                    break;
                case RotationState.Rotation2:
                    startX = 0;
                    startY = 3;
                    break;
                case RotationState.Rotation3:
                    startX = 3;
                    startY = 3;
                    break;
                default:
                    break;
            }

            for (int i = startY; i < endY; i++)
            {
                for (int f = startX; f < endX; f++)
                {

                }
            }
        }
    }
}
