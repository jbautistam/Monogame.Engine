using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;

/// <summary>
///		Colección de estilos
/// </summary>
public class UiStylesCollection(AbstractUserInterfaceLayer layer)
{
	// Constantes privadas
	private const string KeySeparator = "_#_";
	// Variables privadas
	private UiStyle? _defaultStyle = null;

	/// <summary>
	///		Invalida los estilos
	/// </summary>
	public void Invalidate()
	{
		Dirty = true;
	}

	/// <summary>
	///		Añade un estilo
	/// </summary>
	public void Add(string key, UiStyle style)
	{
		Styles.Add(GetKey(key, style.Type), style);
	}

	/// <summary>
	///		Crea un estilo
	/// </summary>
	public UiStyle Add(string key, UiStyle.StyleType type, Backgrounds.UiBackground? background, Borders.UiAbstractBorder? border, 
					   Color? color = null, float opacity = 1)
	{
		UiStyle style = new(this, type)
							{
								Background = background,
								Border = border,
								Color = color ?? Color.White,
								Opacity = opacity
							};

			// Añade el estilo a la colección
			Styles.Add(GetKey(key, type), style);
			// Devuelve el estilo creado
			return style;
	}

	/// <summary>
	///		Obtiene el estilo correspondiente a un tipo
	/// </summary>
	public UiStyle? GetStyle(string key, UiStyle.StyleType type) => Search(key, type) ?? GetDefault(key);

	/// <summary>
	///		Obtiene el estilo predeterminado
	/// </summary>
	public UiStyle GetDefault(string? key)
	{
		UiStyle? style = Search(key, UiStyle.StyleType.Normal);

			// Si no se ha encontrado ningún estilo, se coge el primero
			if (Styles.Count > 0)
				foreach (KeyValuePair<string, UiStyle> keyValue in Styles)
					if (keyValue.Key.StartsWith(key + KeySeparator, StringComparison.CurrentCultureIgnoreCase))
						return keyValue.Value;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return DefaultStyle;
	}

	/// <summary>
	///		Busca un estilo
	/// </summary>
	private UiStyle? Search(string? key, UiStyle.StyleType type)
	{
		if (string.IsNullOrWhiteSpace(key))
			return DefaultStyle;
		else if (Styles.TryGetValue(GetKey(key, type), out UiStyle? style))
			return style;
		else
			return null;
	}

	/// <summary>
	///		Obtiene la clave
	/// </summary>
	private string GetKey(string key, UiStyle.StyleType type) => $"{key}{KeySeparator}{type.ToString()}";

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Actualiza el elemento
		foreach (KeyValuePair<string, UiStyle> keyValue in Styles)
			keyValue.Value.Update(gameContext);
		// Indica que ya se ha actualizado
		Dirty = true;
    }

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public void Draw(Camera2D camera, string key, UiStyle.StyleType type, Rectangle position, Managers.GameContext gameContext)
	{
		UiStyle? style = GetStyle(key, type);

			if (style is not null)
				style.Draw(camera, position, gameContext);
	}

	/// <summary>
	///		Elemento padre
	/// </summary>
	public AbstractUserInterfaceLayer Layer { get; } = layer;

	/// <summary>
	///		Indica si se debe recalcular
	/// </summary>
	public bool Dirty { get; private set; } = true;

	/// <summary>
	///		Estilo predeterminado
	/// </summary>
	public UiStyle DefaultStyle
	{
		get
		{
			// Crea el estilo predeterminado si no existía
			if (_defaultStyle is null)
				_defaultStyle = new UiStyle(this, UiStyle.StyleType.Normal)
										{
											Color = Color.White,
											Opacity = 1
										};
			// Devuelve el estilo predeterminado
			return _defaultStyle;
		}
	}

	/// <summary>
	///		Estilos
	/// </summary>
	private Dictionary<string, UiStyle> Styles { get; } = new(StringComparer.CurrentCultureIgnoreCase);
}
