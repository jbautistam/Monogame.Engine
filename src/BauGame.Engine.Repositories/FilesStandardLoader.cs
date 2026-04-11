using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Layers;

namespace Bau.BauEngine.Repositories;

/// <summary>
///		Clases de carga de archivos de configuración como configuración de definición de texturas, sistemas de partículas,
///	datos de interface de usuario...
/// </summary>
public class FilesStandardLoader(EngineManager manager)
{
	/// <summary>
	///		Carga el texto de un archivo de contenido
	/// </summary>
	private string? LoadTextFile(string fileName) => Manager.FilesManager.StorageManager.ReadTextFile($"Content/{fileName}");

	/// <summary>
	///		Carga las configuraciones de texturas y animación
	/// </summary>
	public void LoadTexturesSettings(string fileName)
	{
		string? xml = LoadTextFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				new Xml.TexturesRepository().Load(xml, Manager.ResourcesManager);
	}

	/// <summary>
	///		Carga las definiciones de los sistemas de partículas
	/// </summary>
	public void LoadParticlesSystem(string fileName)
	{
		string? xml = LoadTextFile("Settings/Particles.xml");
			
			if (!string.IsNullOrWhiteSpace(xml))
				Manager.ResourcesManager.ParticlesResourcesManagers.AddRange(new Xml.ParticleSystemRepository().Load(xml));
	}

	/// <summary>
	///		Carga los estilos
	/// </summary>
	public UiStylesCollection LoadStyles(AbstractUserInterfaceLayer layer, string fileName)
	{
		string? xml = LoadTextFile(fileName);
			
			if (!string.IsNullOrWhiteSpace(xml))
				return new Xml.StylesRepository().Load(layer, xml);
			else
				return new UiStylesCollection(layer);
	}

	/// <summary>
	///		Carga los datos de una pantalla
	/// </summary>
	public (UiStylesCollection styles, List<UiElement> components) LoadScreen(AbstractUserInterfaceLayer layer, string fileName,
																			  Xml.AbstractUserInterfaceRepository? repository = null)
	{
		string? xml = LoadTextFile(fileName);
		UiStylesCollection styles = new(layer);
		List<UiElement> components = [];
			
			// Obtiene los datos
			if (!string.IsNullOrWhiteSpace(xml))
			{
				(string? style, components) = (repository ?? new Xml.UserInterfaceRepository()).Load(layer, xml);

					// Carga los estilos
					if (!string.IsNullOrWhiteSpace(style))
						styles = LoadStyles(layer, style);
			}
			// Devuelve los estilos y componentes de la pantalla
			return new (styles, components);
	}

	/// <summary>
	///		Manager principal del motor
	/// </summary>
	public EngineManager Manager { get; } = manager;
}
