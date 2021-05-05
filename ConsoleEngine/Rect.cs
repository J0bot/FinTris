using System;

namespace ConsoleEngine
{
    public class Rect
    {
        const char CHAR = '█';
        const int THICKNESS = 2;

        private readonly int _width;
        private readonly int _height;
        private Vector2 _position;
        private ConsoleColor _borderColor;
        private ConsoleColor _fillColor;

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

        public ConsoleColor BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        public ConsoleColor FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        public Rect(int x, int y, int width, int height, ConsoleColor borderColor = ConsoleColor.White, ConsoleColor fillColor = ConsoleColor.Black)
        {
            _width = width / THICKNESS;
            _height = height;
            _position = new Vector2(x, y);
            _borderColor = borderColor;
            _fillColor = fillColor;
        }

        public void Draw()
        {

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x > 0 && y > 0 && x < _width - 1 && y < _height - 1)
                    {
                        Console.ForegroundColor = _fillColor;
                    }
                    else
                    {
                        Console.ForegroundColor = _borderColor;
                    }

                    Console.SetCursorPosition(_position.x + (x * THICKNESS), _position.y + y);
                    Console.Write(new string(CHAR, THICKNESS));
                }
            }

            Console.ResetColor();
        }
    }
}
