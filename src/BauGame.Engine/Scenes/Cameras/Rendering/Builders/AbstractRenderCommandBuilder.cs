namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador abstracto
/// </summary>
public abstract class AbstractRenderCommandBuilder(RenderCommandsBuilder parent)
{
	/// <summary>
	///		Vuelve al generador anterior
	/// </summary>
	public RenderCommandsBuilder Back() => Parent;

	/// <summary>
	///		Genera una lista de comandos
	/// </summary>
	public abstract List<AbstractRenderCommand> Build();

	/// <summary>
	///		Generador padre
	/// </summary>
	public RenderCommandsBuilder Parent { get; } = parent;
}
