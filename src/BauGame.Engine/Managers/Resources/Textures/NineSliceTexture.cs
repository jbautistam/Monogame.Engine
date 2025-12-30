using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///     Textura de nueve partes para botones
/// </summary>
public class NineSliceTexture(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
	/// <summary>
	///		Obtiene la región de la textura
	/// </summary>
	public override TextureRegion? GetRegion(string? name)
	{
		Texture2D? texture = GetTexture();

			// Obtiene la textura
			if (texture is not null)
				return new TextureRegion(NormalizeName(name))
								{
									Texture = texture,
									Region = new Rectangle(0, 0, texture.Width, texture.Height)
								};
			else
				return null;
	}

/*
    // Método personalizado para dibujar
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        var width = Bounds.Width;
        var height = Bounds.Height;

        // Calcular dimensiones de las 9 regiones
        var centerWidth = width - LeftSlice - RightSlice;
        var centerHeight = height - TopSlice - BottomSlice;

        // Si el tamaño es demasiado pequeño, no dibujar o ajustar
        if (centerWidth < 0 || centerHeight < 0) return;

        // Definir las 9 regiones de origen (source) y destino (destination)

        // Esquinas (no se escalan)
        spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y, LeftSlice, TopSlice),
                         new Rectangle(0, 0, LeftSlice, TopSlice), color);

        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice + centerWidth, (int)position.Y, RightSlice, TopSlice),
                         new Rectangle(_texture.Width - RightSlice, 0, RightSlice, TopSlice), color);

        spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y + TopSlice + centerHeight, LeftSlice, BottomSlice),
                         new Rectangle(0, _texture.Height - BottomSlice, LeftSlice, BottomSlice), color);

        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice + centerWidth, (int)position.Y + TopSlice + centerHeight, RightSlice, BottomSlice),
                         new Rectangle(_texture.Width - RightSlice, _texture.Height - BottomSlice, RightSlice, BottomSlice), color);

        // Bordes horizontales (solo escalan en X)
        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice, (int)position.Y, centerWidth, TopSlice),
                         new Rectangle(LeftSlice, 0, _texture.Width - LeftSlice - RightSlice, TopSlice), color);

        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice, (int)position.Y + TopSlice + centerHeight, centerWidth, BottomSlice),
                         new Rectangle(LeftSlice, _texture.Height - BottomSlice, _texture.Width - LeftSlice - RightSlice, BottomSlice), color);

        // Bordes verticales (solo escalan en Y)
        spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y + TopSlice, LeftSlice, centerHeight),
                         new Rectangle(0, TopSlice, LeftSlice, _texture.Height - TopSlice - BottomSlice), color);

        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice + centerWidth, (int)position.Y + TopSlice, RightSlice, centerHeight),
                         new Rectangle(_texture.Width - RightSlice, TopSlice, RightSlice, _texture.Height - TopSlice - BottomSlice), color);

        // Centro (escala en ambas direcciones)
        spriteBatch.Draw(_texture, new Rectangle((int)position.X + LeftSlice, (int)position.Y + TopSlice, centerWidth, centerHeight),
                         new Rectangle(LeftSlice, TopSlice, _texture.Width - LeftSlice - RightSlice, _texture.Height - TopSlice - BottomSlice), color);
    }
*/
    /// <summary>
    ///     Tamaño de sección a la izquierda
    /// </summary>
    public int LeftSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección a la derecha
    /// </summary>
    public int RightSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde arriba
    /// </summary>
    public int TopSlice { get; set; }

    /// <summary>
    ///     Tamaño de sección desde abajo
    /// </summary>
    public int BottomSlice { get; set; }
}