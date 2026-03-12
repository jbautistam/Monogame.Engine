using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

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
		LoadTexturesSettings("Settings/textures-common.xml");
		LoadTexturesSettings("Settings/textures-graphics-novel.xml");
		LoadTexturesSettings("Settings/textures-sample.xml");
		LoadTexturesSettings("Settings/textures-space.xml");
		LoadTexturesSettings("Settings/textures-tiles.xml");
	}

	/// <summary>
	///		Carga los estilos
	/// </summary>
	public UiStylesCollection LoadStyles(AbstractUserInterfaceLayer layer, string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				return new Repositories.StylesRepository().Load(layer, xml);
			else
				return new UiStylesCollection(layer);
	}

	/// <summary>
	///		Carga los datos de una pantalla
	/// </summary>
	public List<UiElement> LoadScreen(AbstractUserInterfaceLayer layer, string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				return new Repositories.UserInterfaceRepository().Load(layer, xml);
			else
				return new List<UiElement>();
	}

	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	private void LoadTexturesSettings(string fileName)
	{
		string? xml = ReadFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				new Repositories.TexturesRepository().Load(xml, GameEngine.Instance.ResourcesManager);
	}

	/// <summary>
	///		Carga la configuración de texturas y animación de un archivo
	/// </summary>
	private string? ReadFile(string fileName) => GameEngine.Instance.FilesManager.StorageManager.ReadTextFile($"Content/{fileName}");
}
