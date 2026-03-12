using GameEngine.Math;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Entities.Actors;

public class TextActor : Actor
{
    public string Text { get; set; }
    public SpriteFont Font { get; set; }
    public Color Color { get; set; } = Color.White;
    public Vector2 Origin { get; set; }

    public TextActor(Vector2 position, string text, SpriteFont font) 
        : base(position)
    {
        Text = text;
        Font = font;
    }

    public override void Update(float deltaTime)
    {
    }

    protected override IEnumerable<RenderCommand> GenerateWorldCommands()
    {
        yield break;
    }

    protected override IEnumerable<RenderCommand> GenerateScreenCommands()
    {
        if (string.IsNullOrEmpty(Text))
        {
            yield break;
        }

        yield return new TextRenderCommand
        {
            Text = Text,
            Font = Font,
            Position = Position,
            Color = Color,
            Rotation = Rotation,
            Origin = Origin,
            Scale = Scale,
            Bounds = GetRenderBounds()
        };
    }

    public override RectangleF GetRenderBounds()
    {
        if (Font == null || string.IsNullOrEmpty(Text))
        {
            return new RectangleF(Position, Vector2.Zero);
        }

        Vector2 size = Font.MeasureString(Text) * Scale;
        Vector2 halfSize = size * 0.5f;
        Vector2 min = Position - halfSize;
            
        return new RectangleF(min, size);
    }
}
