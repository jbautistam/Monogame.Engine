using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Scenes.Animations.Actors;

/// <summary>
///		Actor para animaciones
/// </summary>
public class AnimationActor : AbstractActorDrawable
{
	// Constantes públicas
	public const string PlayerName = nameof(PlayerName);
	// Enumerados públicos
	public enum MoveMode
	{
		LeftToRight,
		RightToLeft
	}

	public AnimationActor(AbstractLayer layer, string texture, string animation) : base(layer, 0)
	{
		Texture = texture;
		Animation = animation;
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
	{
		Renderer.Sprite = new Bau.Libraries.BauGame.Engine.Entities.Common.Sprites.SpriteDefinition(Texture, null);
		Renderer.StartAnimation(Texture, Animation, false);
	}

	/// <summary>
	///		Arranca la animación
	/// </summary>
	public void Play()
	{
		Renderer.Reset(Texture, Animation);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.Libraries.BauGame.Engine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Textura
	/// </summary>
	public string Texture { get; }

	/// <summary>
	///		Textura
	/// </summary>
	public string Animation { get; }
}