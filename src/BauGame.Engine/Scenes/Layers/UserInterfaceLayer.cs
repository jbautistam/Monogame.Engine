namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas de interface de usuario
/// </summary>
public class UserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
    /// <summary>
    ///     Arranca el proceso de la capa
    /// </summary>
	protected override void StartLayer()
	{
	}

	/// <summary>
	///		Actualiza las capas de la física
	/// </summary>
	protected override void UpdatePhysicsLayer(Managers.GameContext gameContext)
	{
		// ... en este caso no hace nada
	}

	/// <summary>
	///		Actualiza el interface de usuario de la capa
	/// </summary>
	protected override void UpdateUserInterface(Managers.GameContext gameContext)
	{
	}
}