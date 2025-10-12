namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

public struct UiSize
{
    public float Width { get; set; }
    public float Height { get; set; }
    public bool IsRelative { get; set; } // true = porcentaje, false = píxeles

    public UiSize(float width, float height, bool isRelative = true)
    {
        Width = width;
        Height = height;
        IsRelative = isRelative;
    }

    public static UiSize Relative(float width, float height) => new(width, height, true);

    public static UiSize Absolute(float width, float height) => new(width, height, false);
}