namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Loaders.Models;

/// <summary>
///		Datos de una animación
/// </summary>
public class AnimationDto
{
	/// <summary>
	///		Nombre de la animación
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	///		Nombre de la textura
	/// </summary>
	public string Texture { get; set; } = default!;

	/// <summary>
	///		Frames
	/// </summary>
	public List<AnimationFrameDto> Frames { get; set; } = [];
}