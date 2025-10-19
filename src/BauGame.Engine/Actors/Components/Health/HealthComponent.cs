using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Health;

/// <summary>
///		Componente de salud
/// </summary>
public class HealthComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
	// Variables privadas
	private float _invulnerability = 0;

	/// <summary>
	///		Aplica el daño
	/// </summary>
	public void ApplyDamage(float damage)
	{
		if (_invulnerability <= 0)
		{
			// Aplica el daño
			ActualHealth -= damage;
			// Inicia el tiempo de vulnerabilidad
			_invulnerability = InvulnerabilityTime;
			// Aplica el efecto de invulnerabilidad
			if (InvulnerabilityEffect is not null)
				Owner.Renderer.Effects.Add(InvulnerabilityEffect);
		}
	}

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
	public override void UpdatePhysics(Managers.GameContext gameContext) {}

	/// <summary>
	///		Actualiza el componente
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
		if (_invulnerability > 0)
		{
			// Disminuye el tiempo de invulnerabilidad
			_invulnerability -= gameContext.DeltaTime;
			// Si ha terminado la invulnerabilidad se elimina el efecto
			if (_invulnerability <= 0 && InvulnerabilityEffect is not null)
				InvulnerabilityEffect.Stop();
		}
	}

	/// <summary>
	///		Dibuja el compoente (no hace nada, sólo implementa el interface)
	/// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el uso del compoente (no hace nada, sólo implementa el interface)
	/// </summary>
	public override void End()
	{
	}

	/// <summary>
	///		Número de vidas inicial
	/// </summary>
	public required int Lives { get; init; }

	/// <summary>
	///		Salud inicial
	/// </summary>
	public required float Health { get; init; }

	/// <summary>
	///		Tiempo de invulnerabilidad
	/// </summary>
	public required float InvulnerabilityTime { get; init; }

	/// <summary>
	///		Salud actual
	/// </summary>
	public float ActualHealth { get; private set; }

	/// <summary>
	///		Efecto de invulnerabilidad
	/// </summary>
	public Renderers.Effects.AbstractRendererEffect? InvulnerabilityEffect { get; set; }
}
