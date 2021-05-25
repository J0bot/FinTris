using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleEngine
{
    public class ConsoleBackground : IDrawable
    {
        private readonly Bitmap _bmp;
        private static readonly Dictionary<Color, ConsoleColor> _colorMap = new Dictionary<Color, ConsoleColor>()
        {
            { Color.FromArgb(0, 0, 0), ConsoleColor.Black },
            { Color.FromArgb(0, 0, 128), ConsoleColor.DarkBlue },
            { Color.FromArgb(0, 128, 0), ConsoleColor.DarkGreen },
            { Color.FromArgb(0, 128, 128), ConsoleColor.DarkCyan },
            { Color.FromArgb(128, 0, 0), ConsoleColor.DarkRed },
            { Color.FromArgb(128, 0, 128), ConsoleColor.DarkMagenta },
            { Color.FromArgb(128, 128, 0), ConsoleColor.DarkYellow },
            { Color.FromArgb(192, 192, 192), ConsoleColor.Gray },
            { Color.FromArgb(128, 128, 128), ConsoleColor.DarkGray },
            { Color.FromArgb(0, 0, 255), ConsoleColor.Blue },
            { Color.FromArgb(0, 255, 0), ConsoleColor.Green },
            { Color.FromArgb(0, 255, 255), ConsoleColor.Cyan },
            { Color.FromArgb(255, 0, 0), ConsoleColor.Red },
            { Color.FromArgb(255, 0, 255), ConsoleColor.Magenta },
            { Color.FromArgb(255, 255, 0), ConsoleColor.Yellow },
            { Color.FromArgb(255, 255, 255), ConsoleColor.White },
        };

        public int Width
        {
            get { return _bmp.Width; }
        }

        public int Height
        {
            get { return _bmp.Height; }
        }

        public ConsoleBackground(Bitmap bmp)
        {
            _bmp = bmp;
        }

        public void Render()
        {
            for (int x = 0; x < _bmp.Width; x++)
            {
                for (int y = 0; y < _bmp.Height; y++)
                {
                    if (x == _bmp.Width - 1 && y == _bmp.Height - 1)
                    {
                        //break;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.BackgroundColor = _colorMap[_bmp.GetPixel(x, y)];
                    Console.Write(" ");
                }
            }

            Console.ResetColor();
        }
    }
}
