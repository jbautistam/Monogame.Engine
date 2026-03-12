using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="TextureRenderCommand"/>
/// </summary>
public class TextureRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public TextureRenderCommandBuilder(RenderCommandsBuilder parent, Texture2D texture, Rectangle? source) : base(parent)
	{
		Create(texture, source);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(Texture2D texture, Rectangle? source)
	{
		Commands.Add(new TextureRenderCommand()
								{
									Texture = texture,
									Source = source
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="TextureRenderCommand"/>
	/// </summary>
	public TextureRenderCommandBuilder WithCommand(Texture2D texture, Rectangle? source)
	{
		// Crea el comando
		Create(texture, source);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public TextureRenderCommandBuilder WithTransform(Vector2 position, Vector2 origin)
	{
		// Añade la transformación
		if (LastCommand is not null)
		{
			LastCommand.Transform.Coordinates.Position = position;
			LastCommand.Transform.Coordinates.Origin = origin;
		}
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public TextureRenderCommandBuilder WithTransform(Rectangle destination, Vector2 origin)
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
	///		Añade la la región de la textura
	/// </summary>
	public TextureRenderCommandBuilder WithSource(Rectangle source)
	{
		// Añade la región de la textura
		if (LastCommand is not null)
			LastCommand.Source = source;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la rotación
	/// </summary>
	public TextureRenderCommandBuilder WithRotation(float rotation)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Rotation = rotation;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el orden de dibujo
	/// </summary>
	public TextureRenderCommandBuilder WithZIndex(int zIndex)
	{
		// Añade el orden de dibujo
		if (LastCommand is not null)
			LastCommand.ZIndex = zIndex;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la escala
	/// </summary>
	public TextureRenderCommandBuilder WithScale(float scale) => WithScale(new Vector2(scale, scale));

	/// <summary>
	///		Añade la escala
	/// </summary>
	public TextureRenderCommandBuilder WithScale(Vector2 scale)
	{
		// Añade la escala
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Scale = scale;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el tipo de dibujo
	/// </summary>
	public TextureRenderCommandBuilder WithDrawType(SpriteRenderCommand.DrawType type)
	{
		// Añade el tipo de dibujo
		if (LastCommand is not null)
			LastCommand.DrawMode = type;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el color
	/// </summary>
	public TextureRenderCommandBuilder WithColor(Color color)
	{
		// Añade el color
		if (LastCommand is not null)
			LastCommand.Presentation.Color = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el efecto de dibujo
	/// </summary>
	public TextureRenderCommandBuilder WithEffect(SpriteEffects effect)
	{
		// Añade el efecto
		if (LastCommand is not null)
			LastCommand.Presentation.Effect = effect;
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
			foreach (TextureRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<TextureRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private TextureRenderCommand? LastCommand 
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
