namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Loaders.Models;

/// <summary>
///		Datos de un frame de una animación
/// </summary>
public class AnimationFrameDto
{
	/// <summary>
	///		Nombre del fram
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	///		Tiempo
	/// </summary>
	public float Time { get; set; }
}