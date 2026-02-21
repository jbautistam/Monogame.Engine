using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Interfaz base para todos los tipos de borde
/// </summary>
public interface IBorder
{
    /// <summary>
    /// Dibuja el borde alrededor del área especificada
    /// </summary>
    /// <param name="spriteBatch">SpriteBatch para dibujar</param>
    /// <param name="bounds">Área absoluta donde dibujar el borde</param>
    void Draw(SpriteBatch spriteBatch, Rectangle bounds);
        
    /// <summary>
    /// Espesor del borde en píxeles
    /// </summary>
    int Thickness { get; }
        
    /// <summary>
    /// Indica si el borde tiene contenido para dibujar
    /// </summary>
    bool HasContent { get; }
}
