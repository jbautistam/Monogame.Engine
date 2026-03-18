using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Managers.Resources.Textures.Configuration;

/// <summary>
///		Configuración de una región para una textura
/// </summary>
public class TextureRegionAtlasConfiguration(int row, int column, int rows, int columns) : TextureAbstractRegionConfiguration($"{row.ToString()},{column.ToString()}")
{
	/// <summary>
	///		Obtiene el rectángulo origen de la textura
	/// </summary>
	public override Rectangle GetBounds(Texture2D texture)
	{
        int columnWidth = texture.Width / Columns;
        int rowHeight = texture.Height / Rows;

            // Devuelve el rectángulo origen de la textura
			return new Rectangle(columnWidth * Column, rowHeight * Row, columnWidth, rowHeight);
	}

	/// <summary>
	///		Fila
	/// </summary>
	public int Row { get; } = row;

	/// <summary>
	///		Columna
	/// </summary>
	public int Column { get; } = column;

	/// <summary>
	///		Número de filas total
	/// </summary>
	public int Rows { get; } = rows;

	/// <summary>
	///		Número de columnas total
	/// </summary>
	public int Columns { get; } = columns;

	/// <summary>
	///		Configuración para una textura NineSlice
	/// </summary>
	public TextureRegionNineSliceConfiguration? NineSliceConfiguration { get; set; }
}