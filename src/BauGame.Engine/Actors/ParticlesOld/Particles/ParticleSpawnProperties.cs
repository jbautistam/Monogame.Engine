using Bau.BauEngine.Entities.Common;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.Particles;

/// <summary>
///		Propiedades para el lanzamiento de una partícula
/// </summary>
public class ParticleSpawnProperties
{
	/// <summary>
	///		Rango de tiempo de vida
	/// </summary>
	public required RangeStruct<float> LifeTime { get; init; }

	/// <summary>
	///		Rango de velocidad
	/// </summary>
	public required RangeStruct<float> Speed { get; init; }

	/// <summary>
	///		Rango de ángulo inicial
	/// </summary>
	public required RangeStruct<float> Angle { get; init; }

	/// <summary>
	///		Rango de escala
	/// </summary>
	public required RangeStruct<float> Scale { get; init; }

	/// <summary>
	///		Rango de color
	/// </summary>
	public required RangeStruct<Color> Color { get; init; }

	/// <summary>
	///		Rango de opacidad
	/// </summary>
	public required RangeStruct<float> Opacity { get; init; }

	/// <summary>
	///		Longitud de la cola
	/// </summary>
	public required RangeStruct<float> TailLength { get; init; }

	/// <summary>
	///		Densidad de la cola
	/// </summary>
	public required RangeStruct<float> TailDensity { get; init; }

	/// <summary>
	///		Rotación
	/// </summary>
	public required RangeStruct<float> Rotation { get; init; }

	/// <summary>
	///		Textura
	/// </summary>
	public required string Texture { get; init; }

	/// <summary>
	///		Región de la textura
	/// </summary>
	public string? Region { get; set; }
}