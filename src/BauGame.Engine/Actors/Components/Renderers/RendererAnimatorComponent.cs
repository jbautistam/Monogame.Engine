using Bau.BauEngine.Managers;
using Bau.BauEngine.Managers.Resources.Animations;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Actors.Components.Renderers;

/// <summary>
///		Componente para representación de un actor con animación
/// </summary>
public class RendererAnimatorComponent(AbstractActorDrawable actor) : AbstractRendererComponent(actor), Interfaces.IActorDrawable
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	protected override void StartSelf()
	{
	}

	/// <summary>
	///		Arranca la carga de los datos de la definición
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
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
			Sprite = new SpriteDefinition(texture, Animator.GetDefaultRegion());
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
    protected override void DrawSelf(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
    {
		if (Sprite is not null)
		{
			// Obtiene la región adecuada para la animación
			Sprite.Region = GetRegion();
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
	///		Detiene el componente
	/// </summary>
	protected override void EndSelf()
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Componente de animación
	/// </summary>
	internal AnimatorComponent Animator { get; } = new();

	/// <summary>
	///		Propiedades para el mezclador de animaciones
	/// </summary>
	public AnimatorBlenderProperties? AnimatorBlenderProperties { get; set; }
}