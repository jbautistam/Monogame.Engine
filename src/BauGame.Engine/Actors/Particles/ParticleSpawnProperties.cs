using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///		Propiedades para el lanzamiento de una partícula
/// </summary>
public class ParticleSpawnProperties
{
	// Registros públicos
	public record Range<TypeData>(TypeData Minimum, TypeData Maximum);

	/// <summary>
	///		Rango de tiempo de vida
	/// </summary>
	public required Range<float> LifeTime { get; init; }

	/// <summary>
	///		Rango de velocidad
	/// </summary>
	public required Range<float> Speed { get; init; }

	/// <summary>
	///		Rango de ángulo inicial
	/// </summary>
	public required Range<float> Angle { get; init; }

	/// <summary>
	///		Rango de escala
	/// </summary>
	public required Range<float> Scale { get; init; }

	/// <summary>
	///		Rango de color
	/// </summary>
	public required Range<Color> Color { get; init; }

	/// <summary>
	///		Rango de opacidad
	/// </summary>
	public required Range<float> Opacity { get; init; }

	/// <summary>
	///		Longitud de la cola
	/// </summary>
	public required Range<float> TailLength { get; init; }

	/// <summary>
	///		Densidad de la cola
	/// </summary>
	public required Range<float> TailDensity { get; init; }

	/// <summary>
	///		Rotación
	/// </summary>
	public required Range<float> Rotation { get; init; }

	/// <summary>
	///		Textura
	/// </summary>
	public string? Texture { get; set; }

	/// <summary>
	///		Región de la textura
	/// </summary>
	public string? Region { get; set; }
}
