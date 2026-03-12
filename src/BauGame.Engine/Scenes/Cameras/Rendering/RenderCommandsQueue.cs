using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Cola de comandos
/// </summary>
public class RenderCommandsQueue
{
    // Variables privadas
    private List<AbstractRenderCommand> _commands = [];

    /// <summary>
    ///     Añade un comando
    /// </summary>
    public void Add(AbstractRenderCommand command)
    {
        _commands.Add(command);
    }

    /// <summary>
    ///     Añade una lista de comandos
    /// </summary>
    public void AddRange(List<AbstractRenderCommand> commands)
    {
        foreach (AbstractRenderCommand command in commands)
            _commands.Add(command);
    }

    /// <summary>
    ///     Limpia los comandos
    /// </summary>
    public void Clear()
    {
        _commands.Clear();
    }

    /// <summary>
    ///     Ejecuta los comandos
    /// </summary>
    public void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
        // Ejecuta los comandos
        foreach (AbstractRenderCommand command in _commands)
            command.Execute(director, spriteBatch);
        // y limpia la cola
        Clear();
    }
}
