using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers;

/// <summary>
///		Componente para representación de un actor
/// </summary>
public class RendererComponent(AbstractActorDrawable actor) : AbstractComponent(actor, true), Interfaces.IActorDrawable
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
	}

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
	public override void UpdatePhysics(GameContext gameContext)
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Arranca la carga de los datos de la definición
	/// </summary>
	public override void Update(GameContext gameContext)
	{
		// Carga los datos del sprite
		Sprite?.LoadAsset(Actor.Layer.Scene);
		// Actualiza la animación
		UpdateAnimator(gameContext);
		// Actualiza los efectos
		foreach (Effects.AbstractRendererEffect effect in Effects)
			effect.Update(gameContext);
	}

	/// <summary>
	///		Actualiza el componente de animación
	/// </summary>
	private void UpdateAnimator(GameContext gameContext)
	{
		// Busca la animación correspondiente dependiendo de las propiedades de animación
		if (AnimatorBlenderProperties is not null)
		{
			AnimationBlenderGroupRuleModel? rule = GameEngine.Instance.ResourcesManager.AnimationManager.AnimationBlender.EvaluateRules(AnimatorBlenderProperties);

				if (rule is not null)
				{
					Animation? animation = GameEngine.Instance.ResourcesManager.AnimationManager.Animations.Get(rule.Animation);

						// Arranca la animación
						if (animation is not null)
							StartAnimation(animation.Texture, rule.Animation, rule.Loop);
				}
		}
		// Actualiza el animador
		Animator.Update(gameContext);
	}

	/// <summary>
	///		Arranca una animación
	/// </summary>
	public void StartAnimation(string texture, string animation, bool loop)
	{
		if (Animator.SetAnimation(animation, loop))
		{
			// Crea el sprite
			Sprite = new Entities.Common.Sprites.SpriteDefinition(texture, Animator.GetDefaultRegion());
			// Indica que está animando
			Animator.IsPlaying = true;
		}
	}

	/// <summary>
	///		Arranca de nuevo una animación
	/// </summary>
	public void Reset(string texture, string animation)
	{
		// Inicializa la animación
		Animator.Reset();
		// Asigna la textura
		Animator.SetAnimation(animation, false);
		// Indica que está animando
		Animator.IsPlaying = true;
	}

	/// <summary>
	///		Detiene la animación
	/// </summary>
	public void StopAnimation()
	{
		Animator.IsPlaying = false;
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
    {
		if (Sprite is not null)
		{
			// Obtiene la región adecuada para la animación
			Sprite.Region = GetRegion();
			// Cambia la posición
			Sprite.SpriteEffect = SpriteEffects;
			// Dibuja el sprite
			renderingManager.SpriteRenderer.Draw(Sprite, Actor.Transform.BoundsCentered.Location, Actor.Transform.Center, Scale, Actor.Transform.Rotation, Opacity * Color);
		}
    }

	/// <summary>
	///		Obtiene la región de la textura adecuada para la animación
	/// </summary>
	private string? GetRegion()
	{
		if (Sprite is not null && (Animator.IsPlaying || Animator.HasEndLoop))
			return Animator.GetActualRegion();
		else
			return Sprite?.Region;
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public Size GetSize()
	{
		if (Sprite is not null)
			return Sprite.GetSize();
		else
			return new Size(0, 0);
	}

	/// <summary>
	///		Detiene el componente
	/// </summary>
	public override void End()
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Actor
	/// </summary>
	public AbstractActorDrawable Actor { get; } = actor;

	/// <summary>
	///		Sprite
	/// </summary>
	public Entities.Common.Sprites.SpriteDefinition? Sprite { get; set; }

	/// <summary>
	///		Efectos de dibujo
	/// </summary>
	public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1.0f;

	/// <summary>
	///		Escalado
	/// </summary>
	public Vector2 Scale { get; set; } = Vector2.One;

	/// <summary>
	///		Efectos asociados a la representación
	/// </summary>
	public List<Effects.AbstractRendererEffect> Effects { get; } = [];

	/// <summary>
	///		Componente de animación
	/// </summary>
	internal AnimatorComponent Animator { get; } = new();

	/// <summary>
	///		Propiedades para el mezclador de animaciones
	/// </summary>
	public AnimatorBlenderProperties? AnimatorBlenderProperties { get; set; }
}