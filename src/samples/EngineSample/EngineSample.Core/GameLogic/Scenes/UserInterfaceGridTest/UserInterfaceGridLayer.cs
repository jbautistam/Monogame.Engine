using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Scenes.UserInterfaceGridTest;

/// <summary>
///		Layer de la partida
/// </summary>
public class UserInterfaceGridLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		Configuration.ConfigurationLoader loader = new();

			// Carga los etilos
			Styles = loader.LoadStyles(this, "Settings/VisualNovel/Styles.xml");
			// Carga el archivo de elementos de la pantalla
			Items.AddRange(loader.LoadScreen(this, "Settings/VisualNovel/ScreenUserInterfaceGrid.xml"));
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.BauEngine.Managers.GameContext gameContext)
	{
	}
}
