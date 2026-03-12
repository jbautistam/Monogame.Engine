using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="LineRenderCommand"/>
/// </summary>
public class LineRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public LineRenderCommandBuilder(RenderCommandsBuilder parent, Vector2 start, Vector2 end, int thickness) : base(parent)
	{
		Create(start, end, thickness);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(Vector2 start, Vector2 end, int thickness)
	{
		Commands.Add(new LineRenderCommand()
								{
									Start = start,
									End = end,
									Thickness = thickness
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="RectangleRenderCommand"/>
	/// </summary>
	public LineRenderCommandBuilder WithCommand(Vector2 start, Vector2 end, int thickness)
	{
		// Crea el comando
		Create(start, end, thickness);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el color
	/// </summary>
	public LineRenderCommandBuilder WithColor(Color color)
	{
		// Añade el color
		if (LastCommand is not null)
			LastCommand.Presentation.Color = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el orden de dibujo
	/// </summary>
	public LineRenderCommandBuilder WithZIndex(int zIndex)
	{
		// Añade el orden de dibujo
		if (LastCommand is not null)
			LastCommand.ZIndex = zIndex;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera una lista de comandos
	/// </summary>
	public override List<AbstractRenderCommand> Build()
	{
		List<AbstractRenderCommand> commands = [];

			// Convierte la lista
			foreach (LineRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<LineRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private LineRenderCommand? LastCommand 
	{ 
		get
		{
			if (Commands.Count > 0)
				return Commands[Commands.Count - 1];
			else
				return null;
		}
	}
}
