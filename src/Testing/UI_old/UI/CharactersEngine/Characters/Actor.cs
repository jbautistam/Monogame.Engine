using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Characters;

public class Actor
{
    public string Id { get; }
    public ActorTransform Transform { get; set; } = new();
    public bool IsVisible { get; set; }
    
    // Valores base para efectos que oscilan (shake, pulse)
    public Vector2 BasePosition { get; set; }
    public float BaseScale { get; set; } = 1f;
    public float BaseRotation { get; set; }
    
    public Actor(string id) => Id = id;
    
    public void UpdateBaseValues()
    {
        BasePosition = Transform.Position;
        BaseScale = Transform.Scale;
        BaseRotation = Transform.Rotation;
    }
    
    // Calcula el origen en píxeles para MonoGame
    public Vector2 GetPixelOrigin()
    {
        if (Transform.Texture == null) return Vector2.Zero;
        return new Vector2(
            Transform.Origin.X * Transform.Texture.Width,
            Transform.Origin.Y * Transform.Texture.Height
        );
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible || Transform?.Texture == null) return;
        
        var origin = GetPixelOrigin();
        var color = Transform.Color * Transform.Opacity;
        
        spriteBatch.Draw(
            Transform.Texture,
            Transform.Position,
            null,
            color,
            Transform.Rotation,
            origin,
            Transform.Scale,
            SpriteEffects.None,
            0f
        );
    }
}
