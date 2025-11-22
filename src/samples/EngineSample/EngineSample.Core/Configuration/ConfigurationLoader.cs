using Bau.Libraries.BauGame.Engine;

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
		string? xml = GameEngine.Instance.FilesManager.StorageManager.ReadTextFile("Content/Settings/textures.xml");
			
			if (!string.IsNullOrWhiteSpace(xml))
				new Repositories.TexturesRepository().Load(xml, GameEngine.Instance.ResourcesManager);
	}
}
