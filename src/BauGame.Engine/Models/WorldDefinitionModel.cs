using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Models;

/// <summary>
///		Clase con los datos del mundo
/// </summary>
public struct WorldDefinitionModel(int worldWidth, int worldHeight, int cellWidth, int cellHeight)
{
	/// <summary>
	///		Coordenadas del mundo
	/// </summary>
	public Rectangle WorldBounds { get; } = new(0, 0, worldWidth, worldHeight);

	/// <summary>
	///		Zona muerta para el movimiento de cámara
	/// </summary>
    public Rectangle DeadZone { get; set; } = new(0, 0, 0, 0);

	/// <summary>
	///		Ancho de las celdas del mundo
	/// </summary>
	public int CellWidth { get; } = cellWidth;

	/// <summary>
	///		Alto de las celdas del mundo
	/// </summary>
	public int CellHeight { get; } = cellHeight;
}
