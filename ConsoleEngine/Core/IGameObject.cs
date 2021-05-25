using System;

namespace ConsoleEngine
{
    public interface IGameObject
    {
        int Height { get; set; }
        string Id { get; }
        int Width { get; set; }
        GameObject Parent { get; set; }
        Scene Scene { get; }
        Vector2 Position { get; set; }
        HorizontalAlignment HorizontalAlignment { get; set; }
        SizingMode SizingMode { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }

        event EventHandler<ConsoleKey> KeyPressed;

        void OnKeyPressed(ConsoleKey input);
    }
}