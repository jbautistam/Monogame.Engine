using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

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
		Renderer.AnimatorBlenderProperties = new Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.AnimatorBlenderProperties(groupAnimation);
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
		if (Renderer.AnimatorBlenderProperties is not null)
		{
			if (speed.X != 0 || speed.Y != 0)
				Renderer.AnimatorBlenderProperties.Add("speed", 1);
			else
				Renderer.AnimatorBlenderProperties.Add("speed", 0);
			Renderer.AnimatorBlenderProperties.Add("died", isDead);
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
	protected override void DrawSelf(Camera2D camera, GameContext gameContext)
	{
	}

	/// <summary>
	///		Prepara los comandos de presentación
	/// </summary>
	protected override void PrepareRenderCommandsSelf(RenderCommandsBuilder builder, GameContext gameContext)
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