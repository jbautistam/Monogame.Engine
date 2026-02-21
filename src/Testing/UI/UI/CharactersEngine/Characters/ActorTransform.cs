using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Characters;

/// <summary>
///     Estado del personaje / actor
/// </summary>
public class ActorTransform
{
    public Vector2 Position { get; set; }
    public float Scale { get; set; } = 1f;
    public float Rotation { get; set; }
    public Color Color { get; set; } = Color.White;
    public float Opacity { get; set; } = 1f;
    public Texture2D Texture { get; set; }
    public int ZOrder { get; set; }
    // Origen de rotación/escala (0-1, 0-1 relativo a la textura)
    public Vector2 Origin { get; set; } = new Vector2(0.5f, 1f); // Default: bottom-center
}
