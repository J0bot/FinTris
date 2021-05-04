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
            if (text.Contains("\n"))
            {
                throw new NotSupportedException("Multi-line UI Components are unsupported for the time being");
            }
            _text = text;
            _width = text.Length;
            _height = 1;
            _foregroundColor = DEFAULT_FOREGROUND_COLOR;
            _backgroundColor = DEFAULT_BACKGROUND_COLOR;
        }

        public override void Render()
        {
            Console.ForegroundColor = _foregroundColor;
            Console.BackgroundColor = _backgroundColor;

            int x = (_parent != null ? _parent.Position.x : 0) + _position.x;
            int y = (_parent != null ? _parent.Position.y : 0) + _position.y;

            if (_hAlignment == HorizontalAlignment.Center)
            {
                x += (_width - _text.Length) / 2;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(_text);
            Console.ResetColor();
        }
    }
}
