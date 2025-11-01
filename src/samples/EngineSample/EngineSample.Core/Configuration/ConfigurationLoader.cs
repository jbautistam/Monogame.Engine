using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

namespace EngineSample.Core.Configuration;

/// <summary>
///		Clases de carga de configuración
/// </summary>
public class ConfigurationLoader
{
	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	public void LoadTexturesSettings()
	{
		Loaders.Models.SettingsLoaderDto? dto = GameEngine.Instance.FilesManager.StorageManager.LoadJsonData<Loaders.Models.SettingsLoaderDto>("Content/Settings/textures.json");

			if (dto is not null)
			{
				// Crea las texturas
				foreach (Loaders.Models.TextureDto textureDto in dto.Textures)
					if (textureDto.Rows is null || textureDto.Columns is null)
						GameEngine.Instance.ResourcesManager.TextureManager.Create(textureDto.Name, textureDto.Asset);
					else
						GameEngine.Instance.ResourcesManager.TextureManager.Create(textureDto.Name, textureDto.Asset, textureDto.Rows ?? 0, textureDto.Columns ?? 0);
				// Crea las animaciones
				foreach (Loaders.Models.AnimationDto animationDto in dto.Animations)
				{
					Animation animation = GameEngine.Instance.ResourcesManager.AnimationManager.Add(animationDto.Name, animationDto.Texture, []);

						// Añade los frames
						foreach (Loaders.Models.AnimationFrameDto frameDto in animationDto.Frames)
							animation.Frames.Add(new Animation.AnimationFrame(frameDto.Name, frameDto.Time));
				}
			}
	}
}
