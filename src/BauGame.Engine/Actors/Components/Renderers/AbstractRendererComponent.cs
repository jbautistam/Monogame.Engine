using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Entities.Common;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Actors.Components.Renderers;

/// <summary>
///		Componente base para representación de un actor
/// </summary>
public abstract class AbstractRendererComponent(AbstractActorDrawable actor) : AbstractComponent(actor), Interfaces.IActorDrawable
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		StartSelf();
	}

	/// <summary>
	///		Inicializa el componente
	/// </summary>
	protected abstract void StartSelf();

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
	public override void UpdatePhysics(GameContext gameContext)
	{
		// ... no hace nada, sólo implementa la interface
	}

	/// <summary>
	///		Actualiza el componente
	/// </summary>
	public override void Update(GameContext gameContext)
	{
		// Carga los datos del sprite
		Sprite?.LoadAsset(Actor.Layer.Scene);
		// Actualiza el componente
		UpdateSelf(gameContext);
		// Actualiza los efectos
		foreach (Effects.AbstractRendererEffect effect in Effects)
			effect.Update(gameContext);
	}

	/// <summary>
	///		Actualiza los datos
	/// </summary>
	protected abstract void UpdateSelf(GameContext gameContext);

	/// <summary>
	///		Dibuja el actor
	/// </summary>
    public void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
    {
		if (Sprite is not null)
		{
			// Cambia la posición
			Sprite.SpriteEffect = SpriteEffects;
			// Dibuja el actor
			DrawSelf(renderingManager, gameContext);
		}
    }

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected abstract void DrawSelf(Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext);

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
		EndSelf();
	}

	/// <summary>
	///		Detiene el componente
	/// </summary>
	protected abstract void EndSelf();

	/// <summary>
	///		Actor
	/// </summary>
	public AbstractActorDrawable Actor { get; } = actor;

	/// <summary>
	///		Sprite
	/// </summary>
	public AbstractSpriteDefinition? Sprite { get; set; }

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
}