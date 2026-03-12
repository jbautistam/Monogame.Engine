using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos para un <see cref="SpriteRenderCommand"/>
/// </summary>
public class SpriteRenderCommandBuilder : AbstractRenderCommandBuilder
{
	public SpriteRenderCommandBuilder(RenderCommandsBuilder parent, string texture, string? region) : base(parent)
	{
		Create(texture, region);
	}

	/// <summary>
	///		Crea un comando
	/// </summary>
	private void Create(string texture, string? region)
	{
		Commands.Add(new SpriteRenderCommand()
								{
									Sprite = new Entities.Common.SpriteDefinition(texture, region)
								}
					);
	}

	/// <summary>
	///		Crea un nuevo comando <see cref="SpriteRenderCommand"/>
	/// </summary>
	public SpriteRenderCommandBuilder WithCommand(Entities.Common.SpriteDefinition sprite) => WithCommand(sprite.Asset, sprite.Region);

	/// <summary>
	///		Crea un nuevo comando <see cref="SpriteRenderCommand"/>
	/// </summary>
	public SpriteRenderCommandBuilder WithCommand(string texture, string? region)
	{
		// Crea el comando
		Create(texture, region);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la transformación
	/// </summary>
	public SpriteRenderCommandBuilder WithTransform(Vector2 position, Vector2 origin)
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
	public SpriteRenderCommandBuilder WithTransform(Rectangle destination, Vector2 origin)
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
	public SpriteRenderCommandBuilder WithRotation(float rotation)
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
	public SpriteRenderCommandBuilder WithScale(float scale) => WithScale(new Vector2(scale, scale));

	/// <summary>
	///		Añade la escala
	/// </summary>
	public SpriteRenderCommandBuilder WithScale(Vector2 scale)
	{
		// Añade la rotación
		if (LastCommand is not null)
			LastCommand.Transform.Coordinates.Scale = scale;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade el tipo de dibujo
	/// </summary>
	public SpriteRenderCommandBuilder WithDrawType(SpriteRenderCommand.DrawType type)
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
	public SpriteRenderCommandBuilder WithColor(Color color)
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
	public SpriteRenderCommandBuilder WithEffect(SpriteEffects effect)
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
	public SpriteRenderCommandBuilder WithZIndex(int zIndex)
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
			foreach (SpriteRenderCommand command in Commands)
				commands.Add(command);
			// Devuelve los comandos
			return commands;
	}

	/// <summary>
	///		Lista de comandos
	/// </summary>
	private List<SpriteRenderCommand> Commands { get; } = [];

	/// <summary>
	///		Obtiene el último comando
	/// </summary>
	private SpriteRenderCommand? LastCommand 
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
