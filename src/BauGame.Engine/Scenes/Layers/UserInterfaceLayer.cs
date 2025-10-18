using Microsoft.Xna.Framework;

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
	///		Actualiza el interface de usuario de la capa
	/// </summary>
	protected override void UpdateUserInterface(GameTime gameTime)
	{
	}
}