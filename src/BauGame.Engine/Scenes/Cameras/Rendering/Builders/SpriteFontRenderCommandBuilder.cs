using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="SpriteFontRenderCommand"/>
/// </summary>
public class SpriteFontRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public SpriteFontRenderCommandBuilder(RenderCommandsBuilder parent, string? font, float lineSpacing, bool wrapText) : base(parent)
	{
		Create(font, lineSpacing, wrapText);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(string? font, float lineSpacing, bool wrapText)
	{
		Commands.Add(new SpriteFontRenderCommand()
								{
									Font = font,
									LineSpacing = lineSpacing,
									WrapText = wrapText
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="SpriteFontRenderCommand"/>
	/// </summary>
	public SpriteFontRenderCommandBuilder WithCommand(string? font, float lineSpacing, bool wrapText)
	{
		// Crea el comando
		Create(font, lineSpacing, wrapText);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public SpriteFontRenderCommandBuilder WithTransform(Rectangle destination, Vector2 origin)
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
	public SpriteFontRenderCommandBuilder WithRotation(float rotation)
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
	public SpriteFontRenderCommandBuilder WithScale(float scale) => WithScale(new Vector2(scale, scale));

	/// <summary>
	///		Añade la escala
	/// </summary>
	public SpriteFontRenderCommandBuilder WithScale(Vector2 scale)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Scale = scale;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el orden de dibujo
	/// </summary>
	public SpriteFontRenderCommandBuilder WithZIndex(int zIndex)
	{
		// Añade el orden de dibujo
		if (LastCommand is not null)
			LastCommand.ZIndex = zIndex;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la alineación
	/// </summary>
	public SpriteFontRenderCommandBuilder WithAlignment(UiLabel.HorizontalAlignmentType horizontalAlignment, UiLabel.VerticalAlignmentType verticalAlignment)
	{
		// Añade la rotación
		if (LastCommand is not null)
		{
			LastCommand.HorizontalAlignment = horizontalAlignment;
			LastCommand.VerticalAlignment = verticalAlignment;
		}
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una sección de texto
	/// </summary>
	public SpriteFontRenderCommandBuilder WithText(string text, bool bold, bool italic, Color color, string? font = null)
	{
		// Añade el texto
		if (LastCommand is not null)
			LastCommand.Texts.Add(new SpriteFontTextRenderCommand()
											{
												Font = font,
												Text = text,
												Color = color,
												Bold = bold,
												Italic = italic
											}
								 );
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
			foreach (SpriteFontRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<SpriteFontRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private SpriteFontRenderCommand? LastCommand 
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
