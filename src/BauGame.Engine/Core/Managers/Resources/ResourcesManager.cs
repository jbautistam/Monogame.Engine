
namespace Bau.Monogame.Engine.Domain.Core.Managers.Resources;

/// <summary>
///		Manager para la definición de recursos
/// </summary>
public class ResourcesManager
{
	public ResourcesManager(EngineManager engineManager)
	{
		EngineManager = engineManager;
		TextureManager = new TextureManager(this);
		GlobalContentManager = new ContentDisposableManager(this);
		AnimationManager = new Animations.AnimationManager(this);
	}

	/// <summary>
	///		Carga la configuración de texturas de un archivo
	/// </summary>
	public void LoadSettings(string fileName)
	{
		Loaders.Models.SettingsLoaderDto? dto = EngineManager.FilesManager.StorageManager.LoadJsonData<Loaders.Models.SettingsLoaderDto>(fileName);

			if (dto is not null)
			{
				// Crea las texturas
				foreach (Loaders.Models.TextureDto textureDto in dto.Textures)
					if (textureDto.Rows is null || textureDto.Columns is null)
						TextureManager.Create(textureDto.Name, textureDto.Asset);
					else
						TextureManager.Create(textureDto.Name, textureDto.Asset, textureDto.Rows ?? 0, textureDto.Columns ?? 0);
				// Crea las animaciones
				foreach (Loaders.Models.AnimationDto animationDto in dto.Animations)
				{
					Animations.Animation animation = AnimationManager.Add(animationDto.Name, animationDto.Texture, []);

						// Añade los frames
						foreach (Loaders.Models.AnimationFrameDto frameDto in animationDto.Frames)
							animation.Frames.Add(new Animations.Animation.AnimationFrame(frameDto.Name, frameDto.Time));
				}
			}
	}

	/// <summary>
	///		Manager principal del motor
	/// </summary>
	public EngineManager EngineManager { get; }

	/// <summary>
	///		Manager de contenido global
	/// </summary>
	public ContentDisposableManager GlobalContentManager { get; }

	/// <summary>
	///		Manager de definiciones de texturas
	/// </summary>
	public TextureManager TextureManager { get; }

	/// <summary>
	///		Manager para las definiciones de animaciones
	/// </summary>
	public Animations.AnimationManager AnimationManager { get; }
}