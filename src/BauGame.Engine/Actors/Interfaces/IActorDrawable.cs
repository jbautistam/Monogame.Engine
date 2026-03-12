namespace Bau.Libraries.BauGame.Engine.Actors.Interfaces;

/// <summary>
///		Interface que deben cumplir los actores que se pueden dibujar
/// </summary>
public interface IActorDrawable
{
	/// <summary>
	///		Dibuja el actor
	/// </summary>
	void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext);

	/// <summary>
	///		Prepara los comandos de dibujo
	/// </summary>
	void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Managers.GameContext gameContext);
}
