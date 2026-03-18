using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.Common;

/// <summary>
///		Clase con los datos del mundo
/// </summary>
public struct WorldDefinitionModel(int worldWidth, int worldHeight, int cellWidth, int cellHeight)
{
	/// <summary>
	///		Transforma coordenadas de mundo en coordenadas relativas (0 .. 1)
	/// </summary>
	public Vector2 WorldToRelative(Vector2 worldPosition) => new Vector2(worldPosition.X / WorldBounds.Width, worldPosition.Y / WorldBounds.Height);

	/// <summary>
	///		Transforma coordenadas relativas (0 .. 1) a coordenadas de mundo en coordenadas
	/// </summary>
	public Vector2 RelativeToWorld(Vector2 relativePosition) => new Vector2(relativePosition.X * WorldBounds.Width, relativePosition.Y * WorldBounds.Height);

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
