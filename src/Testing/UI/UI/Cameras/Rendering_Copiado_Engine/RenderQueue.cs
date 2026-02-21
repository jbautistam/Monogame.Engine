using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering;

public class RenderQueue
{
    private readonly List<RenderCommand> _commands = new List<RenderCommand>();

    public IReadOnlyList<RenderCommand> Commands => _commands;

    public void Clear()
    {
        _commands.Clear();
    }

    public void Enqueue(RenderCommand command)
    {
        _commands.Add(command);
    }

    public void EnqueueRange(IEnumerable<RenderCommand> commands)
    {
        foreach (var command in commands)
        {
            _commands.Add(command);
        }
    }

    public void Execute(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        foreach (var command in _commands)
        {
            command.Draw(spriteBatch, defaultFont);
        }
    }
}
