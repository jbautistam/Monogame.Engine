using Bau.Libraries.LibHelper.Extensors;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.Particles;

/// <summary>
///		Layer del interface de usuario de la escena
/// </summary>
public class ParticlesUserInterfaceLayer(ParticlesScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string PromptControlName = nameof(PromptControlName);
	// Enumerados privadas
	private enum MenuOption
	{
		Unknown,
		Explossion,
		Books
	}

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		Configuration.ResourcesLoader loader = new(Scene.SceneManager.EngineManager);
		(UiStylesCollection styles, List<UiElement> components) = loader.LoadScreen(this, "Settings/VisualNovel/ParticlesSceneScreen.xml");

			// Carga los estilos
			Styles = styles;
			// Carga el archivo de elementos de la pantalla
			Items.Clear();
			Items.AddRange(components);
			// Asigna los manejadores de eventos
			Click += (sender, args) => TreatClick(args.Component, args.Tag);
	}

	/// <summary>
	///		Trata las pulsaciones
	/// </summary>
	private void TreatClick(UiElement component, string? tag)
	{
		switch (component)
		{
			case UiMenu menu:
					if (!string.IsNullOrWhiteSpace(tag))
						switch ((MenuOption) tag.GetInt(0))
						{
							case MenuOption.Explossion:
									ShowParticles("Explossion", new Vector2(300, 300));
								break;
							case MenuOption.Books:
									ShowParticles("Books-01", new Vector2(50, 50));
								break;
						}
				break;
		}
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.BauEngine.Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Muestra las partículas
	/// </summary>
	private void ShowParticles(string name, Vector2 position)
	{
		if (Scene is ParticlesScene particlesScene)
			particlesScene.ShowParticles(name, position);
	}
}
