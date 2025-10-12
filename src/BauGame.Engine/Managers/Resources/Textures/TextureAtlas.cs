using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

/// <summary>
///     Definición de un sprite sheet sobre una textura
/// </summary>
public class TextureAtlas(TextureManager textureManager, string id, string asset) : AbstractTexture(textureManager, id, asset)
{
    // Variables privadas
    private bool _initialized = false;

    /// <summary>
    ///     Obtiene una región
    /// </summary>
	public override TextureRegion? GetRegion(string name)
	{
        // Crea las regiones
        if (!_initialized)
        {
            // Crea las regiones
            CreateRegions();
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Devuelve la región
        return Regions.Get(name);
	}

    /// <summary>
    ///     Obtiene el rectángulo de la región a partir de la fila y columna
    /// </summary>
    private void CreateRegions()
    {
        Texture2D? texture = GetTexture();

            if (texture is not null)
            {
                int columnWidth = texture.Width / Columns;
                int rowHeight = texture.Height / Rows;

                    // Añade los rectángulos
                    for (int row = 0; row < Rows; row++)
                        for (int column = 0; column < Columns; column++)
                        {
                            string name = $"{row.ToString()},{column.ToString()}";

                                // Añade las regiones al diccionario
                                Regions.Add(name, new TextureRegion(name)
                                                            {
                                                                Texture = texture,
                                                                Region = new Rectangle(columnWidth * column, rowHeight * row, columnWidth, rowHeight)
                                                            }
                                           );
                        }
            }
    }

    /// <summary>
    ///     Filas
    /// </summary>
    public required int Rows { get; init; }

    /// <summary>
    ///     Columnas
    /// </summary>
    public required int Columns { get; init; }

    /// <summary>
    ///     Regiones
    /// </summary>
    public Base.DictionaryModel<TextureRegion> Regions { get; } = new();
}