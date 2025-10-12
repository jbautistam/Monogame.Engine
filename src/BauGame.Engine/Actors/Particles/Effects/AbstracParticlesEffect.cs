using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Effects;

/// <summary>
///		Clase base para los efectos de emisión de partículas
/// </summary>
public abstract class AbstracParticlesEffect(int numberOfParticles, Color? startColor = null)
{
	/// <summary>
	///		Emite las partículas
	/// </summary>
	public abstract void Emit(ParticlesSystemActor particlesSystem);

	/// <summary>
	///		Número de partículas inicial
	/// </summary>
	public int NumberOfParticles { get; } = numberOfParticles;
	
	/// <summary>
	///		Color inicial
	/// </summary>
	public Color? StartColor { get; } = startColor;
}
