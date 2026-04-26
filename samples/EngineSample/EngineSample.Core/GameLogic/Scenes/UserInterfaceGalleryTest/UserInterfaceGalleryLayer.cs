using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Galleries;
using Bau.BauEngine.Entities.UserInterface.Styles;

namespace EngineSample.Core.GameLogic.Scenes.UserInterfaceGalleryTest;

/// <summary>
///		Layer de una galería
/// </summary>
public class UserInterfaceGalleryLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Variables privadas
	private UiGallery? _gallery;

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		Configuration.ResourcesLoader loader = new(Scene.SceneManager.EngineManager);
		(UiStylesCollection styles, List<UiElement> components) = loader.LoadScreen(this, "Settings/VisualNovel/ScreenUserInterfaceGallery.xml");

			// Carga los etilos
			Styles = loader.LoadStyles(this, "Settings/VisualNovel/Styles.xml");
			// Carga el archivo de elementos de la pantalla
			Items.AddRange(components);
			// Obtiene la galería
			_gallery = GetItem<UiGallery>("gallery");
			// Asigna los manejadores de eventos
			Click += (sender, args) => TreatClickButton(args.Component);
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.BauEngine.Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Trata los eventos de pulsación sobre los botones
	/// </summary>
	private void TreatClickButton(UiElement component)
	{
		if (_gallery is not null)
			switch (component.Id)
			{
				case "cmdUp":
						_gallery.MoveUp(1);
					break;
				case "cmdLeft":
						_gallery.MoveLeft(1);
					break;
				case "cmdRight":
						_gallery.MoveRight(1);
					break;
				case "cmdDown":
						_gallery.MoveDown(1);
					break;
			}
	}
}