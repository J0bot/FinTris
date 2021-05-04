using System;

namespace ConsoleEngine
{
    interface IClickable
    {
        event EventHandler<ConsoleKey> Clicked;
    }
}
