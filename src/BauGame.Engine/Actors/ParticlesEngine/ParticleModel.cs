using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine;

/// <summary>
///		Clase con los datos de una partícula
/// </summary>
public class ParticleModel
{
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
	///		Escala
	/// </summary>
	public float Scale { get; set; }

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; }

	/// <summary>
	///		Indica si la partícula está activa
	/// </summary>
	public bool Enabled => LifeTime < TotalLifeTime;
}