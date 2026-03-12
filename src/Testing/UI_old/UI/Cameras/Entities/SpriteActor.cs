using GameEngine.Math;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Entities.Actors;

public class SpriteActor : Actor
{
    public Texture2D Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }
    public Color Tint { get; set; } = Color.White;
    public Vector2 Origin { get; set; }

    public SpriteActor(Vector2 position, Texture2D texture) 
        : base(position)
    {
        Texture = texture;
        Origin = texture.Bounds.Center.ToVector2();
    }

    public override void Update(float deltaTime)
    {
    }

    protected override IEnumerable<RenderCommand> GenerateWorldCommands()
    {
        if (Texture == null) 
        {
            yield break;
        }

        yield return new TextureRenderCommand
        {
            Texture = Texture,
            Position = Position,
            SourceRectangle = SourceRectangle,
            Color = Tint,
            Rotation = Rotation,
            Origin = Origin,
            Scale = Scale,
            Bounds = GetRenderBounds()
        };
    }

    protected override IEnumerable<RenderCommand> GenerateScreenCommands()
    {
        yield break;
    }

    public override RectangleF GetRenderBounds()
    {
        if (Texture == null) 
        {
            return new RectangleF(Position, Vector2.Zero);
        }

        Vector2 sourceSize;
            
        if (SourceRectangle.HasValue)
        {
            sourceSize = new Vector2(
                SourceRectangle.Value.Width,
                SourceRectangle.Value.Height
            );
        }
        else
        {
            sourceSize = Texture.Bounds.Size.ToVector2();
        }
            
        Vector2 size = sourceSize * Scale;
        Vector2 halfSize = size * 0.5f;
        Vector2 min = Position - halfSize;
            
        return new RectangleF(min, size);
    }
}