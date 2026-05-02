using Bau.Libraries.LibHelper.Extensors;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Styles;

namespace EngineSample.Core.GameLogic.Scenes.Effects;

/// <summary>
///		Layer de la partida
/// </summary>
public class EffectsUserInterfaceLayer(EffectsScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string PromptControlName = nameof(PromptControlName);
	// Enumerados privadas
	private enum MenuOption
	{
		Unknown,
		Wipe
	}

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		Configuration.FilesManager loader = new(Scene.SceneManager.EngineManager);
		(UiStylesCollection styles, List<UiElement> components) = loader.LoadScreen(this, "Settings/Screens/EffectsScreen.xml");

			// Carga los estilos
			Styles = styles;
			// Carga el archivo de elementos de la pantalla
			Items.Clear();
			Items.AddRange(components);
			// Asigna los manejadores de eventos
			Click += (sender, args) => TreatClick(args.Component, args.Tag);
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.BauEngine.Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Trata las pulsaciones sobre la interface de usuario
	/// </summary>
	private void TreatClick(UiElement component, string? tag)
	{
		switch (component)
		{
			case UiMenu menu:
					if (!string.IsNullOrWhiteSpace(tag))
						TreatMenuOption((MenuOption) tag.GetInt(0));
				break;
		}
	}

	/// <summary>
	///		Trata las opciones del menú
	/// </summary>
	private void TreatMenuOption(MenuOption option)
	{
		switch (option)
		{
			case MenuOption.Wipe:
					MainScene.EffectsLayer?.CreateEffectWipe();
				break;
		}
	}

	/// <summary>
	///		Escena principal
	/// </summary>
	private EffectsScene MainScene { get; } = scene;
}