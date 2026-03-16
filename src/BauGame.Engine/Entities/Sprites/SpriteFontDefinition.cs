using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

/// <summary>
///		Definición de fuente
/// </summary>
public class SpriteFontDefinition
{
	// Variables privadas
	private string? _font;
	private SpriteFont? _spriteFont;
	private bool _isDirty;

	public SpriteFontDefinition(string? font)
	{
		Font = font;
		Renderer = new SpriteFontRenderer(this);
	}

	/// <summary>
	///		Actualiza los datos del sprite
	/// </summary>
	public void Update(GameContext gameContext)
	{
	}

	/// <summary>
	///		Carga la fuente si es la primera vez o ha habido modificaciones
	/// </summary>
	public SpriteFont? LoadAsset()
	{
		// Carga la fuente si no estaba ya en memoria o se ha modificado
		if (_isDirty)
		{
			// Carga la fuente
			if (!string.IsNullOrEmpty(Font))
				_spriteFont = GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<SpriteFont>(Font);
			// Si no ha podido cargar la fuente, carga la predeterminada
			if (_spriteFont is null && !string.IsNullOrWhiteSpace(GameEngine.Instance.EngineSettings.DefaultFont))
				_spriteFont = GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<SpriteFont>(GameEngine.Instance.EngineSettings.DefaultFont);
			// Indica que se ha cargado con las últimas modificaciones
			_isDirty = false;
		}
		// Devuelve la fuente cargada
		return _spriteFont;
	}

	/// <summary>
	///		Nombre de la fuente
	/// </summary>
	public string? Font
	{ 
		get { return _font;}
		set
		{
			if (!string.IsNullOrWhiteSpace(value) && !value.Equals(_font, StringComparison.CurrentCultureIgnoreCase))
			{
				_font = value;
				_isDirty = true;
			}
		}
	}

	/// <summary>
	///		Efecto aplicado al sprite
	/// </summary>
	public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

	/// <summary>
	///		Rutinas de dibujo de la fuente
	/// </summary>
	public SpriteFontRenderer Renderer { get; }
}
