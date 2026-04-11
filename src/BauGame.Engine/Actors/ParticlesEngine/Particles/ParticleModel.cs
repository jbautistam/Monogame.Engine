using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Particles;

/// <summary>
///		Clase con los datos de una partícula
/// </summary>
public class ParticleModel
{
	/// <summary>
	///		Actualiza los datos de la partícula
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		Position += Velocity;
		LifeTime += gameContext.DeltaTime;
	}

	/// <summary>
	///		Tiempo de vida actual
	/// </summary>
	public float LifeTime { get; set; }

	/// <summary>
	///		Tiempo de vida total (no varía a lo largo del tiempo)
	/// </summary>
	public float TotalLifeTime { get; set; }

	/// <summary>
	///		Posición actuali
	/// </summary>
	public Vector2 Position { get; set; }

	/// <summary>
	///		Velocidad
	/// </summary>
	public Vector2 Velocity { get; set; }

	/// <summary>
	///		Rotación
	/// </summary>
	public float Rotation { get; set; }

	/// <summary>
	///		Definición del sprite
	/// </summary>
	public Entities.Sprites.SpriteDefinition? Sprite { get; set; }

	/// <summary>
	///		Escala
	/// </summary>
	public float Scale { get; set; } = 1;

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;

	/// <summary>
	///		Indica si la partícula está activa
	/// </summary>
	public bool Enabled => LifeTime < TotalLifeTime;
}