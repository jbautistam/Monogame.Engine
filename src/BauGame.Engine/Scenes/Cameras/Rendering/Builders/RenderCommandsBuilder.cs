using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;

/// <summary>
///		Generador de comandos
/// </summary>
public class RenderCommandsBuilder
{
	// Variables privadas
	private List<AbstractRenderCommandBuilder> _builders = [];

	/// <summary>
	///		Crea un generador para un comando <see cref="RectangleRenderCommand"/>
	/// </summary>
	public RectangleRenderCommandBuilder WithCommand(int thickness, bool fill)
	{
		RectangleRenderCommandBuilder builder = new(this, thickness, fill);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Crea un generador para un comando <see cref="SpriteRenderCommand"/>
	/// </summary>
	public SpriteRenderCommandBuilder WithCommand(Entities.Common.SpriteDefinition sprite)
	{
		SpriteRenderCommandBuilder builder = new(this, sprite.Asset, sprite.Region);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Crea un generador para un comando <see cref="SpriteRenderCommand"/>
	/// </summary>
	public SpriteRenderCommandBuilder WithCommand(string texture, string? region)
	{
		SpriteRenderCommandBuilder builder = new(this, texture, region);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Crea un generador para un comando <see cref="SpriteFontRenderCommand"/>
	/// </summary>
	public SpriteFontRenderCommandBuilder WithCommand(string? font, float lineSpacing, bool wrapText)
	{
		SpriteFontRenderCommandBuilder builder = new(this, font, lineSpacing, wrapText);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Crea un generador para un comando <see cref="TextureRenderCommand"/>
	/// </summary>
	public TextureRenderCommandBuilder WithCommand(Texture2D texture, Rectangle source)
	{
		TextureRenderCommandBuilder builder = new(this, texture, source);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Crea un generador para un comando <see cref="TextRenderCommand"/>
	/// </summary>
	public TextRenderCommandBuilder WithCommand(SpriteFont font, string text)
	{
		TextRenderCommandBuilder builder = new(this, font, text);

			// Añade el generador
			_builders.Add(builder);
			// Devuelve el generador
			return builder;
	}

	/// <summary>
	///		Genera los comandos
	/// </summary>
	public List<AbstractRenderCommand> Build()
	{
		List<AbstractRenderCommand> commands = [];

			// Genera los comandos
			foreach (AbstractRenderCommandBuilder builder in _builders)
				commands.AddRange(builder.Build());
			// Devuelve los comandos
			return commands;
	}
}