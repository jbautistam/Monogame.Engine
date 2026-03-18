using Bau.BauEngine.Actors.Components.Renderers;
using Bau.BauEngine.Entities.Common;
using Bau.BauEngine.Managers;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

/// <summary>
///		Actor que representa un fondo
/// </summary>
public class BackgroundActor(Bau.BauEngine.Scenes.Layers.AbstractLayer layer, CharacterDefinition definition, int logicalLayer, int logicalZOrder) 
					: AbstractCharacterActor(layer, definition, logicalLayer, logicalZOrder)
{
	/// <summary>
	///		Arranca el actor
	/// </summary>
	protected override void StartCharacter()
	{
		AbstractRendererComponent? component = Components.GetComponent<AbstractRendererComponent>();

			// Elimina el renderer predeterminado
			if (component is not null)
				Components.Remove(component);
			// Añade el renderer de fondo
			Renderer = new Components.RendererBackgroundComponent(this);
			Components.Add(Renderer);
			// Asigna los límites del rectángulo al mundo
			Transform.Bounds = new RectangleF(0, 0, Layer.Scene.WorldDefinition.WorldBounds.Width, Layer.Scene.WorldDefinition.WorldBounds.Height);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

    /// <summary>
	///		Región relativa de la textura a mostrar
	/// </summary>
    public RectangleF RelativeRegion { get; set; } = new(0, 0, 1, 1);
}