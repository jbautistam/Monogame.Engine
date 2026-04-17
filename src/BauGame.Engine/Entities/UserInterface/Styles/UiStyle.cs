using Bau.BauEngine.Entities.Sprites;
using Bau.BauEngine.Entities.UserInterface.Backgrounds;
using Bau.BauEngine.Entities.UserInterface.Borders;
using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de un elemento
/// </summary>
public class UiStyle(UiStylesCollection stylesParent, UiStyle.StyleType type)
{
	/// <summary>
	///		Tipo para el que se define el estilo
	/// </summary>
	public enum StyleType
	{
		/// <summary>Elemento inactivo</summary>
		Disabled,
		/// <summary>Elemento presionado</summary>
		Pressed,
		/// <summary>Elemento seleccionado</summary>
		Selected,
		/// <summary>Elemento con el cursor encima</summary>
		Hover,
		/// <summary>Normal</summary>
		Normal
	}

	/// <summary>
	///		Actualiza los datos
	/// </summary>
	public void Invalidate()
	{
		Dirty = true;
	}

	/// <summary>
	///		Inicializa los datos
	/// </summary>
	private void Start()
	{
		if (Dirty)
		{
			// Inicializa el borde y el fondo
			StartBorder();
			StartBackground();
			StartFont();
			// Indica que ya está inicializado
			Dirty = false;
		}
	}

	/// <summary>
	///		Inicializa el borde
	/// </summary>
	private void StartBorder()
	{
		if (StyleBorder is not null)
		{
			if (!string.IsNullOrWhiteSpace(StyleBorder.Texture))
			{
				UiTextureBorder real = new(this);

					// Asigna la textura
					real.Sprite = new SpriteDefinition(StyleBorder.Texture, StyleBorder.Region);
					// Asigna el borde
					Border = real;
			}
			else
			{
				UiSolidBorder real = new(this);

					// Asigna los posibles datos del borde
					real.CornerRadius = StyleBorder.CornerRadius;
					real.ShadowColor = StyleBorder.ShadowColor;
					real.ShadowOffset = StyleBorder.ShadowOffset;
					real.ShadowBlurRadius = StyleBorder.ShadowBlurRadius;
					// Asigna el borde
					Border = real;
			}
			// Inicializa los datos comunes del borde
			Border.Color = StyleBorder.Color;
			Border.Thickness = StyleBorder.Thickness;
		}
	}

	/// <summary>
	///		Inicializa el fondo
	/// </summary>
	private void StartBackground()
	{
		if (StyleBackground is not null)
		{
			// Carga los datos del fondo
			if (!string.IsNullOrWhiteSpace(StyleBackground.Texture))
			{
				UiBackground real = new(this);

					// Asigna los datos
					real.Sprite = new SpriteDefinition(StyleBackground.Texture, StyleBackground.Region);
					// Guarda el fondo para cargar el resto de atributos
					Background = real;
			}
			else
				Background = new UiSolidColorBackground(this);
			// Asigna los valores comunes
			Background.Color = StyleBackground.Color;
			Background.Opacity = StyleBackground.Opacity;
		}
	}

	/// <summary>
	///		Inicializa la fuente
	/// </summary>
	private void StartFont()
	{
		if (StyleText is not null)
			Font = new SpriteTextDefinition(StyleText.Font);
		else
			Font = new SpriteTextDefinition(StylesParent.Layer.Scene.SceneManager.EngineManager.EngineSettings.DefaultFont);
	}

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public void Update(GameContext gameContext)
    {
		Start();
		Border?.Update(gameContext);
		Background?.Update(gameContext);
		Font?.Update(gameContext);
    }

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext)
	{
		Background?.Draw(renderingManager, position, gameContext);
		Border?.Draw(renderingManager, position, gameContext);
	}

	/// <summary>
	///		Colección padre
	/// </summary>
	public UiStylesCollection StylesParent { get; } = stylesParent;

	/// <summary>
	///		Tipo
	/// </summary>
	public StyleType Type { get; } = type;

	/// <summary>
	///		Margen externo
	/// </summary>
	public UiMargin Margin { get; set; } = new(0);

	/// <summary>
	///		Espaciado interno
	/// </summary>
	public UiMargin Padding { get; set; } = new(0);

	/// <summary>
	///		Fondo
	/// </summary>
	public UiAbstractBackground? Background { get; private set; }

	/// <summary>
	///		Borde
	/// </summary>
	public UiAbstractBorder? Border { get; private set; }

    /// <summary>
    ///     Definición de la fuente
    /// </summary>
    public SpriteTextDefinition? Font { get; private set; }

	/// <summary>
	///		Estilo del fondo
	/// </summary>
	public UiStyleBackground? StyleBackground { get; set; }

	/// <summary>
	///		Estilo del borde
	/// </summary>
	public UiStyleBorder? StyleBorder { get; set; }

	/// <summary>
	///		Estilo del texto
	/// </summary>
	public UiStyleText? StyleText { get; set; }

	/// <summary>
	///		Estilo de presentación
	/// </summary>
	public UiStylePresentation StylePresentation { get; } = new();

	/// <summary>
	///		Indica si se debe recalcular
	/// </summary>
	public bool Dirty { get; private set; } = true;
}