using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Models;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Renderers;

/// <summary>
///		Componente para representación de un actor
/// </summary>
public class RendererComponent(AbstractActor actor) : AbstractComponent(actor, true)
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
		// ... no hace nada
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
					StartAnimation(rule.Texture, rule.Animation, rule.Loop);
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
			// Asgina la textura
			Texture = texture;
			Region = Animator.GetDefaultRegion();
			// Indica que está animando
			Animator.IsPlaying = true;
		}
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
    public override void Draw(Camera2D camera, GameContext gameContext)
    {
		TextureRegion? region = GetRegion(Region);

			if (region is not null)
			{
				Vector2 position;

					// Calcula la posición dependiendo de si las coordenadas son relativas
					if (ScaleToViewPort)
						position = camera.WorldToScreenRelative(Actor.Transform.Bounds.TopLeft);
					else
						position = camera.WorldToScreen(Actor.Transform.BoundsCentered.TopLeft);
					// Dibuja el actor
					region.Draw(camera, position, 
								Actor.Transform.Center, CalculateScale(camera.ScreenViewport, region.Region),
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
	///		Calcula la escala
	/// </summary>
	private Vector2 CalculateScale(Viewport screenViewport, Rectangle region)
	{
		if (!ScaleToViewPort)
			return Scale;
		else
		{
			float scaleX = (float) screenViewport.Width / region.Width;
			float scaleY = (float) screenViewport.Height / region.Height;
			float scale = Math.Min(scaleX, scaleY);
    
				// Redondear hacia abajo para mantener píxeles enteros
				if (ScaleToViewPortPerfect)
					scale = (float) Math.Floor(scale);
				// Devuelve la escala
				return new Vector2(scale, scale);
		}
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	internal Size GetSize()
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
	public AbstractActor Actor { get; } = actor;

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
	public Vector2 Scale { get; set; } = new(1, 1);

	/// <summary>
	///		Indica si se tiene que escalar a las coordenadas del ViewPort
	/// </summary>
	public bool ScaleToViewPort { get; set; }

	/// <summary>
	///		Indica si se tiene que escalar a las coordenadas del ViewPort de forma perfecta
	/// </summary>
	public bool ScaleToViewPortPerfect { get; set; }

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
	public AnimatorBlenderProperties? AnimatorBlenderProperties { get; }
}