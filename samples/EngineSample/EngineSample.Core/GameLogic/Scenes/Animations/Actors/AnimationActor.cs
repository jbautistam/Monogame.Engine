using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Actors.Components.Renderers;

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
		Renderer.Sprite = new Bau.BauEngine.Entities.Sprites.SpriteDefinition(Texture, null);
		if (Renderer is RendererAnimatorComponent animator)
			animator.StartAnimation(Texture, Animation, false);
	}

	/// <summary>
	///		Arranca la animación
	/// </summary>
	public void Play()
	{
		if (Renderer is RendererAnimatorComponent animator)
			animator.Reset(Texture, Animation);
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
	protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
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