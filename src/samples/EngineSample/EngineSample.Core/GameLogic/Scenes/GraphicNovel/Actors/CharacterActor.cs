using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

/// <summary>
///		Actor que representa un personaje
/// </summary>
public class CharacterActor(Bau.Libraries.BauGame.Engine.Scenes.Layers.AbstractLayer layer, int logicalLayer, int logicalZOrder, CharacterDefinition definition) 
					: Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable(layer, logicalLayer * 1_000 + logicalZOrder)
{
	/// <summary>
	///		Arranca el actor
	/// </summary>
	protected override void StartActor()
	{
		UpdateExpression(CharacterExpressionDefinition.DefaultType);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Actualiza la expresión
	/// </summary>
	public void UpdateExpression(string expression)
	{
		CharacterExpressionDefinition? definition = Definition.GetExpression(expression);

			// Cambia la textura
			if (definition is not null)
			{
				Renderer.Texture = definition.Sprite.Asset;
				Renderer.Region = definition.Sprite.Region;
			}
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.Libraries.BauGame.Engine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Definición del actor
	/// </summary>
	public CharacterDefinition Definition { get; } = definition;

	/// <summary>
	///		Indice de capa donde se dibuja el actor
	/// </summary>
	public int LogicalLayer { get; } = logicalLayer;

	/// <summary>
	///		Zorder lógico
	/// </summary>
	public int LogicalZOrder { get; } = logicalZOrder;
}
