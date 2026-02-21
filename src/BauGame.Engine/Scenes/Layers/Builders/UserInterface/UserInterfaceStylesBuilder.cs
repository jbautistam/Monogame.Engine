using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Borders;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Interface de generación de estilos
/// </summary>
public class UserInterfaceStylesBuilder
{
	public UserInterfaceStylesBuilder(AbstractUserInterfaceLayer layer)
	{
		Styles = new UiStylesCollection(layer);
	}
	
	/// <summary>
	///		Crea un estilo
	/// </summary>
	public UserInterfaceStylesBuilder WithStyle(string name, UiStyle.StyleType type)
	{
		// Genera el estilo
		LastStyle = Styles.Add(name, type, null, null);
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea una textura
	/// </summary>
	public UserInterfaceStylesBuilder WithBackground(string texture, Color? color = null, float opacity = 1)
	{
		// Genera el estilo
		if (LastStyle is not null)
			LastStyle.Background = new UiBackground(LastStyle)
											{
												Texture = texture,
												Color = color ?? Color.White,
												Opacity = opacity
											};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea una textura de fondo
	/// </summary>
	public UserInterfaceStylesBuilder WithBackground(string texture, UiTextureBackground.TextureScaleMode mode, Color? color = null, float opacity = 1)
	{
		// Asinga el fondo
		if (LastStyle is not null)
			LastStyle.Background = new UiTextureBackground(LastStyle)
												{
													Texture = texture,
													ScaleMode = mode,
													Color = color ?? Color.White,
													Opacity = opacity
												};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea una textura sólida
	/// </summary>
	public UserInterfaceStylesBuilder WithBackground(Color color, float opacity = 1)
	{
		// Asinga el fondo
		if (LastStyle is not null)
			LastStyle.Background = new UiSolidColorBackground(LastStyle)
												{
													Color = color,
													Opacity = opacity
												};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea un borde
	/// </summary>
	public UserInterfaceStylesBuilder WithBorder(Color color, int thicknes, float opacity = 1)
	{
		// Asinga el fondo
		if (LastStyle is not null)
			LastStyle.Border = new UiSolidBorder(LastStyle)
												{
													Color = color,
													Thickness = thicknes,
													Opacity = opacity
												};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea un borde
	/// </summary>
	public UserInterfaceStylesBuilder WithBorder(Color shadowColor, Vector2 offset, int blurRadius)
	{
		// Asinga el fondo
		if (LastStyle is not null)
			LastStyle.Border = new UiShadowBorder(LastStyle)
												{
													ShadowColor = shadowColor,
													Offset = offset,
													BlurRadius = blurRadius
												};
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna un color
	/// </summary>
	public UserInterfaceStylesBuilder WithColor(Color color)
	{
		// Genera el estilo
		if (LastStyle is not null)
			LastStyle.Color = color;
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Asigna la opacidad
	/// </summary>
	public UserInterfaceStylesBuilder WithOpacity(float opacity)
	{
		// Genera la opacidad
		if (LastStyle is not null)
			LastStyle.Opacity = opacity;
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Asigna el margen
	/// </summary>
	public UserInterfaceStylesBuilder WithMargin(int all) => ApplyMargin(GetMargin(all, all, all, all));
	
	/// <summary>
	///		Asigna el margen
	/// </summary>
	public UserInterfaceStylesBuilder WithMargin(int horizontal, int vertical) => ApplyMargin(GetMargin(horizontal, vertical, horizontal, vertical));
	
	/// <summary>
	///		Asigna el margen
	/// </summary>
	public UserInterfaceStylesBuilder WithMargin(int left, int top, int right, int bottom) => ApplyMargin(GetMargin(left, top, right, bottom));

	/// <summary>
	///		Aplica el margen
	/// </summary>
	private UserInterfaceStylesBuilder ApplyMargin(UiMargin margin)
	{
		// Aplica el margen
		if (LastStyle is not null)
			LastStyle.Margin = margin;
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Asigna el espaciado interno
	/// </summary>
	public UserInterfaceStylesBuilder WithPadding(int all) => ApplyPadding(GetMargin(all, all, all, all));
	
	/// <summary>
	///		Asigna el espaciado interno
	/// </summary>
	public UserInterfaceStylesBuilder WithPadding(int horizontal, int vertical) => ApplyPadding(GetMargin(horizontal, vertical, horizontal, vertical));
	
	/// <summary>
	///		Asigna el espaciado interno
	/// </summary>
	public UserInterfaceStylesBuilder WithPadding(int left, int top, int right, int bottom) => ApplyPadding(GetMargin(left, top, right, bottom));

	/// <summary>
	///		Aplica el margen
	/// </summary>
	private UserInterfaceStylesBuilder ApplyPadding(UiMargin padding)
	{
		// Aplica el espaciado
		if (LastStyle is not null)
			LastStyle.Padding = padding;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Crea el margen
	/// </summary>
	private UiMargin GetMargin(int left, int top, int right, int bottom) => new(left, top, right, bottom);

	/// <summary>
	///		Genera la colección de estilos
	/// </summary>
	public UiStylesCollection Build() => Styles;

	/// <summary>
	///		Estilos
	/// </summary>
	public UiStylesCollection Styles { get; }

	/// <summary>
	///		Último estilo generado
	/// </summary>
	public UiStyle? LastStyle { get; private set; } 
}
