using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="RectangleRenderCommand"/>
/// </summary>
public class RectangleRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public RectangleRenderCommandBuilder(RenderCommandsBuilder parent, int thickness, bool fill) : base(parent)
	{
		Create(thickness, fill);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(int thickness, bool fill)
	{
		Commands.Add(new RectangleRenderCommand()
								{
									Thickness = thickness,
									Fill = fill
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="RectangleRenderCommand"/>
	/// </summary>
	public RectangleRenderCommandBuilder WithCommand(int thickness, bool fill)
	{
		// Crea el comando
		Create(thickness, fill);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public RectangleRenderCommandBuilder WithTransform(Rectangle destination, Vector2 origin)
	{
		// Añade la transformación
		if (LastCommand is not null)
		{
			LastCommand.Transform.Destination = destination;
			LastCommand.Transform.Coordinates.Origin = origin;
		}
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la rotación
	/// </summary>
	public RectangleRenderCommandBuilder WithRotation(float rotation)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Rotation = rotation;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la escala
	/// </summary>
	public RectangleRenderCommandBuilder WithScale(float scale) => WithScale(new Vector2(scale, scale));

	/// <summary>
	///		Añade la escala
	/// </summary>
	public RectangleRenderCommandBuilder WithScale(Vector2 scale)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Scale = scale;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el color
	/// </summary>
	public RectangleRenderCommandBuilder WithColor(Color color)
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
	public RectangleRenderCommandBuilder WithZIndex(int zIndex)
	{
		// Añade el orden de dibujo
		if (LastCommand is not null)
			LastCommand.ZIndex = zIndex;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una sombra
	/// </summary>
	public RectangleRenderCommandBuilder WithShadow(Vector2 offset, Color color, int blurRadius)
	{
		// Añade los datos de la sombra
		if (LastCommand is not null)
		{
			LastCommand.ShadowOffset = offset;
			LastCommand.ShadowColor = color;
			LastCommand.ShadowBlurRadius = blurRadius;
		}
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un radio para las esquinas
	/// </summary>
	public RectangleRenderCommandBuilder WithCorner(int cornerRadius)
	{
		// Añade los datos de las esquinas
		if (LastCommand is not null)
			LastCommand.CornerRadius = cornerRadius;
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
			foreach (RectangleRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<RectangleRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private RectangleRenderCommand? LastCommand 
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
