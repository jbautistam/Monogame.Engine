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
	// Variables privadas
	private string? _texture, _region;
	private AbstractTexture? _textureSprite;
	private bool _updated;

	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		_updated = true;
		LoadTexture();
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
		// Actualiza la textura
		LoadTexture();
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
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	private void LoadTexture()
	{
		if (_updated)
		{
			// Carga la textura
			if (string.IsNullOrWhiteSpace(Texture))
				_textureSprite = null;
			else
				_textureSprite = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Texture);
			// Indica que se ha cargado con las últimas modificaciones
			_updated = false;
		}
	}

	/// <summary>
	///		Arranca una animación
	/// </summary>
	public void StartAnimation(string texture, string animation, bool loop)
	{
		if (Animator.SetAnimation(animation, loop))
		{
			// Asigna la textura
			Texture = texture;
			Region = Animator.GetDefaultRegion();
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
		TextureRegion? region = GetRegion(Region);

			if (region is not null)
			{
				Vector2 position;

					// Calcula la posición dependiendo de si las coordenadas son relativas
					position = Actor.Transform.BoundsCentered.Location;
					// Dibuja el actor
					region.Draw(renderingManager, position, Actor.Transform.Center, Scale,
								SpriteEffects, Opacity * Color, Actor.Transform.Rotation);
			}
    }

	/// <summary>
	///		Obtiene la región de la textura para dibujarla
	/// </summary>
	private TextureRegion? GetRegion(string? region)
	{
		if (_textureSprite is null)
			return null;
		else if (Animator.IsPlaying || Animator.HasEndLoop)
			return Animator.GetTexture(_textureSprite);
		else
			return _textureSprite.GetRegion(region);
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public Size GetSize()
	{
		TextureRegion? region = GetRegion(Region);

			if (region is null)
				return new Size(0, 0);
			else
				return new Size(region.Region.Width, region.Region.Height);
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
	///		Textura
	/// </summary>
	public string? Texture 
	{	
		get { return _texture; }
		set
		{
			if (string.IsNullOrWhiteSpace(_texture) || !_texture.Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				_texture = value;
				_updated = true;
			}
		}
	}

	/// <summary>
	///		Región
	/// </summary>
	public string? Region
	{	
		get { return _region; }
		set
		{
			if (string.IsNullOrWhiteSpace(_region) || !_region.Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				_region = value;
				_updated = true;
			}
		}
	}

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