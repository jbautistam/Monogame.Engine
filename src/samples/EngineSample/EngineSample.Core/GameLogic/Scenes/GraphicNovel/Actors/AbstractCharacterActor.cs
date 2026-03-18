using Bau.BauEngine.Managers;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

/// <summary>
///		Actor que representa un personaje
/// </summary>
public abstract class AbstractCharacterActor(Bau.BauEngine.Scenes.Layers.AbstractLayer layer, CharacterDefinition definition, 
											 int logicalLayer, int logicalZOrder) 
					: Bau.BauEngine.Actors.AbstractActorDrawable(layer, definition.ZOrder)
{
	/// <summary>
	///		Arranca el actor
	/// </summary>
	protected override void StartActor()
	{
		// Arranca el personaje: tiene que estar primero porque se cambia el renderer cuando el actor es un fondo
		StartCharacter();
		// Actualiza la expresión
		UpdateExpression(CharacterExpressionDefinition.DefaultType);
	}

	/// <summary>
	///		Arranca el carácter
	/// </summary>
	protected abstract void StartCharacter();

	/// <summary>
	///		Actualiza la expresión
	/// </summary>
	public void UpdateExpression(string expression)
	{
		CharacterExpressionDefinition? definition = Definition.GetExpression(expression);

			// Cambia la textura
			if (definition is not null)
				Renderer.Sprite = new Bau.BauEngine.Entities.Sprites.SpriteDefinition(definition.Sprite.Asset, definition.Sprite.Region);
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Obtiene el ZOrder del actor
	/// </summary>
	public int GetZOrder() => 1_000 * LogicalLayer + LogicalZOrder;


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