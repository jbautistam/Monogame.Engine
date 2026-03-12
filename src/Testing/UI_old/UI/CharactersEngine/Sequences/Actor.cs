using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Sequences;

/// <summary>
///		Clase con los datos de un actor
/// </summary>
public class Actor
{
	public string Id { get; set; } = Guid.NewGuid().ToString();

	public int Layer { get; set; }

	public float Opacity { get; set; }

	public Color Color { get; set; }

	public Vector2 Position { get; set; }

	public Vector2 Scale { get; set; }

	public float Rotation { get; set; }

	public int ZOrder { get; set; }

	public Texture2D? Texture { get; set; }

	public int DrawOrder => 10_000 * Layer + ZOrder;
}