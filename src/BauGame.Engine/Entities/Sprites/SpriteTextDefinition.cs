using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Entities.Sprites;

/// <summary>
///		Definición de fuente
/// </summary>
public class SpriteTextDefinition
{
	// Variables privadas
	private string? _font;
	private SpriteFont? _spriteFont;
	private bool _isDirty;

	public SpriteTextDefinition(string? font)
	{
		Font = font;
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
	public SpriteFont? LoadAsset(Scenes.AbstractScene scene)
	{
		// Carga la fuente si no estaba ya en memoria o se ha modificado
		if (_isDirty)
		{
			// Carga la fuente
			if (!string.IsNullOrEmpty(Font))
				_spriteFont = scene.SceneManager.EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<SpriteFont>(Font);
			// Si no ha podido cargar la fuente, carga la predeterminada
			if (_spriteFont is null && !string.IsNullOrWhiteSpace(scene.SceneManager.EngineManager.EngineSettings.DefaultFont))
				_spriteFont = scene.SceneManager.EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<SpriteFont>(scene.SceneManager.EngineManager.EngineSettings.DefaultFont);
			// Indica que se ha cargado con las últimas modificaciones
			_isDirty = false;
		}
		// Devuelve la fuente cargada
		return _spriteFont;
	}

	/// <summary>
	///		Mide la longitud de una cadena
	/// </summary>
	public Vector2 MeasureString(string text)
	{
		if (_spriteFont is not null)
			return _spriteFont.MeasureString(text) * TextScale;
		else
			return Vector2.One;
	}

	/// <summary>
	///		Obtiene el espaciado entre líneas
	/// </summary>
	public float GetLineSpacing()
	{
		if (_spriteFont is not null)
			return TextScale * _spriteFont.LineSpacing * LineSpacing;
		else
			return TextScale * LineSpacing;
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
	///		Espaciado entre líneas
	/// </summary>
	public float LineSpacing { private get; set; } = 1;

    /// <summary>
    ///     Escala del texto
    /// </summary>
    public float TextScale { get; set; } = 1;
}
