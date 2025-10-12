namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources.Loaders.Models;

/// <summary>
///		Datos de una textura
/// </summary>
public class TextureDto
{
	/// <summary>
	///		Nombre de la textura
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	///		Archivo de la textura
	/// </summary>
	public string Asset { get; set; } = default!;

	/// <summary>
	///		Filas de la textura
	/// </summary>
	public int? Rows { get; set; }

	/// <summary>
	///		Columnas de la textura
	/// </summary>
	public int? Columns { get; set; }
}