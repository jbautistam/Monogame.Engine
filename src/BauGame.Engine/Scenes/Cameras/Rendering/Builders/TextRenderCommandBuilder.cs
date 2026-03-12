using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="TextRenderCommand"/>
/// </summary>
public class TextRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public TextRenderCommandBuilder(RenderCommandsBuilder parent, SpriteFont font, string text) : base(parent)
	{
		Create(font, text);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(SpriteFont font, string text)
	{
		Commands.Add(new TextRenderCommand()
								{
									Font = font,
									Text = text
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="TextRenderCommand"/>
	/// </summary>
	public TextRenderCommandBuilder WithTextureCommand(SpriteFont font, string text)
	{
		// Crea el comando
		Create(font, text);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public TextRenderCommandBuilder WithTransform(Vector2 position, Vector2 origin)
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
	///		Añade la rotación
	/// </summary>
	public TextRenderCommandBuilder WithRotation(float rotation)
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
	public TextRenderCommandBuilder WithScale(float scale) => WithScale(new Vector2(scale, scale));

	/// <summary>
	///		Añade la escala
	/// </summary>
	public TextRenderCommandBuilder WithScale(Vector2 scale)
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
	public TextRenderCommandBuilder WithColor(Color color)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Presentation.Color = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el efecto de dibujo
	/// </summary>
	public TextRenderCommandBuilder WithEffect(SpriteEffects effect)
	{
		// Añade el efecto
		if (LastCommand is not null)
			LastCommand.Presentation.Effect = effect;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el orden de dibujo
	/// </summary>
	public TextRenderCommandBuilder WithZIndex(int zIndex)
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
			foreach (TextRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<TextRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private TextRenderCommand? LastCommand 
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
