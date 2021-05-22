using System;

namespace ConsoleEngine
{
    public class TextBlock : UIComponent
    {
        const ConsoleColor DEFAULT_FOREGROUND_COLOR = ConsoleColor.White;
        const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;

        protected string _text;
        protected ConsoleColor _foregroundColor;
        protected ConsoleColor _backgroundColor;

        private int _actualWidth;
        private bool _isMultiLine;
        private string[] _lines;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;
            set => _foregroundColor = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public TextBlock(string text)
        {
            //if (text.Contains("\n"))
            //{
            //    throw new NotSupportedException("Multi-line UI Components are unsupported for the time being");
            //}
            _lines = text.Split('\n');
            foreach (string line in _lines)
            {
                if (_actualWidth < line.Length)
                {
                    _actualWidth = line.Length;
                }
            }
            _text = text;
            _height = _lines.Length;
            _foregroundColor = DEFAULT_FOREGROUND_COLOR;
            _backgroundColor = DEFAULT_BACKGROUND_COLOR;
            _width = _actualWidth;
            _isMultiLine = _lines.Length > 0;
        }

        public override void Render()
        {
            Console.ForegroundColor = _foregroundColor;
            Console.BackgroundColor = _backgroundColor;

            int x = (_parent != null ? _parent.Position.x : 0) + _position.x;
            int y = (_parent != null ? _parent.Position.y : 0) + _position.y;

            if (_hAlignment == HorizontalAlignment.Center)
            {
                if (_isMultiLine)
                {
                    x += (_width - _actualWidth) / 2;
                }
                else
                {
                    x += (_width - _text.Length) / 2;
                }
            }

            int dy = 0;
            foreach (string line in _lines)
            {
                Console.SetCursorPosition(x, y + dy);
                Console.Write(line);
                dy++;
            }

            Console.ResetColor();
        }
    }
}
