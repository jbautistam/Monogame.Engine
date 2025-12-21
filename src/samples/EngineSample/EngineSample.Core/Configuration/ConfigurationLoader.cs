using Bau.Libraries.BauGame.Engine;

namespace EngineSample.Core.Configuration;

/// <summary>
///		Clases de carga de configuración
/// </summary>
public class ConfigurationLoader
{
	/// <summary>
	///		Carga las configuraciones de texturas y animación
	/// </summary>
	public void LoadTexturesSettings()
	{
		LoadTexturesSettings("textures-common.xml");
		LoadTexturesSettings("textures-graphics-novel.xml");
		LoadTexturesSettings("textures-sample.xml");
		LoadTexturesSettings("textures-space.xml");
		LoadTexturesSettings("textures-tiles.xml");
	}

	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	private void LoadTexturesSettings(string fileName)
	{
		string? xml = GameEngine.Instance.FilesManager.StorageManager.ReadTextFile($"Content/Settings/{fileName}");
			
			if (!string.IsNullOrWhiteSpace(xml))
				new Repositories.TexturesRepository().Load(xml, GameEngine.Instance.ResourcesManager);
	}
}
