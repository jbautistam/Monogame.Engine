using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Scenes.Rendering.Renderers;

/// <summary>
///     Clase para dibujo de un <see cref="SpriteTextDefinition"/>
/// </summary>
public class SpriteTextRenderer(RenderingManager renderingManager)
{
	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteTextDefinition? sprite, string text, Point position, Color color)
    {
        DrawString(sprite, text, new Vector2(position.X, position.Y), color);
    }

	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteTextDefinition? sprite, string text, Vector2 position, Color color)
	{
		SpriteFont? spriteFont = sprite?.LoadAsset(RenderingManager.Scene);

			if (spriteFont is not null && RenderingManager.SpriteBatch is not null)
				RenderingManager.SpriteBatch.DrawString(spriteFont, text, position, color);
	}

	/// <summary>
	///		Escribe una cadena
	/// </summary>
	public void DrawString(SpriteTextDefinition? sprite, SpriteTextParameters parameters, Rectangle bounds)
	{
        // Cargamos el sprite font. Lo necesitamos porque vamos a medir el ancho de visualización de la cadena
        sprite?.LoadAsset(RenderingManager.Scene);
        // Recalcula los datos y dibuja la cadena
        if (sprite is not null)
        {
            List<string> lines = parameters.GetLines(sprite, bounds);
            float lineHeight = sprite.GetLineSpacing();
            float textHeight = lines.Count * lineHeight;
            float startY;

                // Quita el último espaciado de las líneas
                textHeight -= lineHeight - sprite.GetLineSpacing();
                // Calcula la posición inicial dependiendo de la alineación
                switch (parameters.VerticalAlignment)
                {
                    case Entities.UserInterface.UiLabel.VerticalAlignmentType.Center: 
                            startY = parameters.Bounds.Y + 0.5f * (parameters.Bounds.Height - textHeight); 
                        break;
                    case Entities.UserInterface.UiLabel.VerticalAlignmentType.Bottom: 
                            startY = parameters.Bounds.Y + parameters.Bounds.Height - textHeight; 
                        break;
                    default: 
                            startY = parameters.Bounds.Y; 
                        break;
                }
                // Dibuja las líneas
                for (int index = 0; index < lines.Count; index++)
                {
                    string line = lines[index];
                    Vector2 size = sprite.MeasureString(line);
                    float x;
                    float y = startY + (index * lineHeight);
                
                        // Posición X según alineación
                        switch (parameters.HorizontalAlignment)
                        {
                            case Entities.UserInterface.UiLabel.HorizontalAlignmentType.Center:
                                    x = parameters.Bounds.X + 0.5f * (parameters.Bounds.Width - size.X);
                                break;
                            case Entities.UserInterface.UiLabel.HorizontalAlignmentType.Right:
                                    x = parameters.Bounds.X + parameters.Bounds.Width - size.X;
                                break;
                            default:
                                    x = parameters.Bounds.X;
                                break;
                        }
                        // Calcula la coordenada Y
                        y = Math.Min(y, y + textHeight - lineHeight);
                        // Dibuja el texto
                        DrawString(sprite, line, new Vector2(x, y), parameters.Color);
                }
        }
	}

    /// <summary>
    ///     Manager de representación
    /// </summary>
    public RenderingManager RenderingManager { get; } = renderingManager;
}