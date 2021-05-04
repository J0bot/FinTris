using System;

namespace FinTris
{
    public class Button : TextBlock
    {
        const ConsoleColor DEFAULT_SELECTED_FCOLOR = ConsoleColor.Black;
        const ConsoleColor DEFAULT_SELECTED_BCOLOR = ConsoleColor.White;

        private bool _isSelected;
        private ConsoleColor _sForegroundColor;
        private ConsoleColor _sBackgroundColor;

        public event EventHandler Clicked;

        public ConsoleColor ForegroundColorSelected
        {
            get => _sForegroundColor;
            set => _sForegroundColor = value;
        }

        public ConsoleColor BackgroundColorSelected
        {
            get => _sBackgroundColor;
            set => _sBackgroundColor = value;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public Button(string text) : base(text)
        {
            _sForegroundColor = DEFAULT_SELECTED_FCOLOR;
            _sBackgroundColor = DEFAULT_SELECTED_BCOLOR;
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