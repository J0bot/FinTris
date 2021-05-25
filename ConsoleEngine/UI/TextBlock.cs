using System;

namespace ConsoleEngine
{
    public class TextBlock : GameObject, IDrawable
    {
        const ConsoleColor DEFAULT_FOREGROUND_COLOR = ConsoleColor.White;
        const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;

        protected string _text;
        protected string _format;
        protected int _actualWidth;
        protected int _actualHeight;
        protected ConsoleColor _foregroundColor;
        protected ConsoleColor _backgroundColor;

        private bool _isMultiLine;
        private string[] _lines;

        public string Text
        {
            get => _text;
            set => UpdateText(value);
        }

        public string Format
        {
            get => _format;
            set => _format = value;
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
            UpdateText(text);
        }

        private void UpdateText(string text)
        {
            // Support paragraphs.
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

        public virtual void Render()
        {
            Console.ForegroundColor = _foregroundColor;
            Console.BackgroundColor = _scene.BackgroundColor;

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

        public void SetText(string text)
        {
            UpdateText(_format.Replace("{0}", text));
        }
    }
}
