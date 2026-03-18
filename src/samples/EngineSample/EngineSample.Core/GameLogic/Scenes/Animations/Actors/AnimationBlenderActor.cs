using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Actors.Components.Renderers;

namespace EngineSample.Core.GameLogic.Scenes.Animations.Actors;

/// <summary>
///		Actor para animaciones
/// </summary>
public class AnimationBlenderActor : AbstractActorDrawable
{
	// Constantes públicas
	public const string PlayerName = nameof(PlayerName);
	// Enumerados públicos
	public enum MoveMode
	{
		LeftToRight,
		RightToLeft
	}

	public AnimationBlenderActor(AbstractLayer layer, string groupAnimation) : base(layer, 0)
	{
		if (Renderer is RendererAnimatorComponent animator)
			animator.AnimatorBlenderProperties = new AnimatorBlenderProperties(groupAnimation);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
	{
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		// Cambia el movimiento
		if (Speed.X > 0)
			Moving = MoveMode.LeftToRight;
		else if (Speed.X < 0)
			Moving = MoveMode.RightToLeft;
		// Actualiza las propiedades de animación
		UpdateAnimation(Speed, IsDead);
	}

	/// <summary>
	///		Actualiza las propiedades de animación
	/// </summary>
	private void UpdateAnimation(Vector2 speed, bool isDead)
	{
		// Asigna las propiedades
		if (Renderer is RendererAnimatorComponent animator && animator.AnimatorBlenderProperties is not null)
		{
			if (speed.X != 0 || speed.Y != 0)
				animator.AnimatorBlenderProperties.Add("speed", 1);
			else
				animator.AnimatorBlenderProperties.Add("speed", 0);
			animator.AnimatorBlenderProperties.Add("died", isDead);
		}
		// Cambia la orientación del sprite
		if (Moving == MoveMode.LeftToRight)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Forma de movimiento
	/// </summary>
	public MoveMode Moving { get; private set; } = MoveMode.LeftToRight;

	/// <summary>
	///		Velocidad
	/// </summary>
	public Vector2 Speed { get; set; } = Vector2.Zero;

	/// <summary>
	///		Indica si está eliminado
	/// </summary>
	public bool IsDead { get; set; }
}