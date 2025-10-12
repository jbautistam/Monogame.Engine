namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

public struct UIMargin
{
    public UIMargin(float all)
    {
        Left = Top = Right = Bottom = all;
    }

    public UIMargin(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public float Left { get; set; }

    public float Top { get; set; }

    public float Right { get; set; }

    public float Bottom { get; set; }

    public float Horizontal => Left + Right;

    public float Vertical => Top + Bottom;
}
