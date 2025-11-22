using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///		Layer de fondo
/// </summary>
public class BackgroundLayer(AbstractScene scene, string name, int sortOrder) : AbstractLayer(scene, name, LayerType.Background, sortOrder)
{
	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartLayer()
	{
		foreach (Backgrounds.AbstractBackgroundLayer background in BackgroundLayers)
			background.Start();
	}

	/// <summary>
	///		Actualiza las capas de la física
	/// </summary>
	protected override void UpdatePhysicsLayer(GameContext gameContext)
	{
		// ... en este caso no hace nada
	}

	/// <summary>
	///		Actualiza la capa
	/// </summary>
	protected override void UpdateLayer(GameContext gameContext)
	{
		foreach (Backgrounds.AbstractBackgroundLayer background in BackgroundLayers)
			if (background.Visible)
				background.UpdateLayer(gameContext);
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected override void DrawLayer(Cameras.Camera2D camera, GameContext gameContext)
	{
		foreach (Backgrounds.AbstractBackgroundLayer background in BackgroundLayers)
			if (background.Visible)
				background.DrawLayer(camera, gameContext);
	}

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndLayer()
	{
		foreach (Backgrounds.AbstractBackgroundLayer background in BackgroundLayers)
			background.End();
	}

	/// <summary>
	///		Lista de fondos
	/// </summary>
	public List<Backgrounds.AbstractBackgroundLayer> BackgroundLayers { get; } = [];
}
