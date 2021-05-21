using System;

namespace FinTris
{
    public class Border
    {
        const char CHAR = '█';
        const int THICKNESS = 2;

        private readonly int _width;
        private readonly int _height;
        private Vector2 _position;
        private ConsoleColor _color;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public ConsoleColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Border(int x, int y, int width, int height)
        {
            _width = width;
            _height = height;
            _position = new Vector2(x, y);
            _color = ConsoleColor.White;
        }

        public void Draw()
        {
            Console.ForegroundColor = _color;

            for (int y = 0; y < Height; y++)
            {
                Console.SetCursorPosition(_position.x, _position.y + y);

                if (y > 0 && y < Height -1)
                {
                    Console.Write(new string(CHAR, THICKNESS));
                    Console.SetCursorPosition(_position.x + Width - THICKNESS, _position.y + y);
                    Console.Write(new string(CHAR, THICKNESS));
                }
                else
                {
                    Console.Write(new string(CHAR, Width));
                }
            }

            Console.ResetColor();
        }
    }
}
