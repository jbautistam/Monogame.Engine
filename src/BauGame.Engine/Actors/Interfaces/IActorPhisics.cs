namespace Bau.Libraries.BauGame.Engine.Actors.Interfaces;

/// <summary>
///		Interface que deben cumplir los actores que tratan con físicas
/// </summary>
public interface IActorPhisics
{
	/// <summary>
	///		Actualiza las físicas
	/// </summary>
    public void UpdatePhysics(Managers.GameContext gameContext);

	/// <summary>
	///		Obtiene los objetos con los que se tiene contacto hasta cierto punto
	/// </summary>
	public List<Scenes.Physics.KinematicCollisionModel> Raycast(Microsoft.Xna.Framework.Vector2 direction, float distance, bool stopAtFirst);
}
