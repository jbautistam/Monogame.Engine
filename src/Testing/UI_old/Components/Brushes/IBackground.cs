using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Interfaz base para todos los tipos de fondo
/// </summary>
public interface IBackground
{
    /// <summary>
    /// Dibuja el fondo dentro del área especificada
    /// </summary>
    /// <param name="spriteBatch">SpriteBatch para dibujar</param>
    /// <param name="bounds">Área absoluta en píxeles donde dibujar</param>
    /// <param name="contentBounds">Área de contenido (después de padding)</param>
    void Draw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle contentBounds);
        
    /// <summary>
    /// Indica si el fondo tiene contenido para dibujar
    /// </summary>
    bool HasContent { get; }
}
