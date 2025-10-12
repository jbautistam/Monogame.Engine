namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Loaders.Models;

/// <summary>
///		Dto para carga de configuración
/// </summary>
public class SettingsLoaderDto
{
	/// <summary>
	///		Texturas
	/// </summary>
	public List<TextureDto> Textures { get; set; } = [];

	/// <summary>
	///		Animaciones
	/// </summary>
	public List<AnimationDto> Animations { get; set; } = [];
}
