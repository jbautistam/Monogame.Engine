using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Scenes.Layers.Services.Particles;

/// <summary>
///		Manager del motor de partículas
/// </summary>
public class ParticleEngineService(string name) : AbstractLayerService(name)
{
	/// <summary>
	///		Arranca el servicio
	/// </summary>
	protected override void StartService()
	{
	}

	/// <summary>
	///		Actualiza el servicio
	/// </summary>
	protected override void UpdateService(GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el servicio
	/// </summary>
	protected override void StopService()
	{
	}
}
