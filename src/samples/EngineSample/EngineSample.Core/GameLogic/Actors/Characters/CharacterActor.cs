using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Actor que representa un personaje
/// </summary>
public class CharacterActor(Bau.Libraries.BauGame.Engine.Scenes.Layers.AbstractLayer layer, CharacterDefinition definition) : Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable(layer, null)
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
		// Manda un mensaje con la posición del actor
		Layer.Scene.MessagesManager.SendMessage(this, "Hud", Constants.PlayerPosition, $"{Transform.Bounds.Top:#,##0}, {Transform.Bounds.Left:#,##0}");
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
	protected override void DrawSelf(Camera2D camera, GameContext gameContext)
	{
	}

	/// <summary>
	///		Prepara los comandos de dibujado
	/// </summary>
	protected override void PrepareRenderCommandsSelf(RenderCommandsBuilder builder, GameContext gameContext)
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
}
