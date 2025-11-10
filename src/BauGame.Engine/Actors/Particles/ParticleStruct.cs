namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///		Estructura con los datos de una partícula
/// </summary>
internal struct ParticleStruct : Pool.IPoolable
{
	/// <summary>
	///		Modifica el estado de la partícula
	/// </summary>
	public void SetEnabled(bool enabled)
	{
		Enabled = enabled;
	}

	/// <summary>
	///		Indica si la partícula está activa en el pool
	/// </summary>
	public bool Enabled { get; private set; }
}
