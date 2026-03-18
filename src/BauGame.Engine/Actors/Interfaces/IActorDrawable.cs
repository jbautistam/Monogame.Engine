namespace Bau.BauEngine.Actors.Interfaces;

/// <summary>
///		Interface que deben cumplir los actores que se pueden dibujar
/// </summary>
public interface IActorDrawable
{
	/// <summary>
	///		Dibuja el actor
	/// </summary>
	void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext);
}
