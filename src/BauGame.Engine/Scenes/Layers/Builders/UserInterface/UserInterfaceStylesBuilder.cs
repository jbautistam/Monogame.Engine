using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Borders;
using Bau.BauEngine.Entities.UserInterface.Backgrounds;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Scenes.Layers.Builders.UserInterface;

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
	///		Crea un fondo con una textura
	/// </summary>
	public UserInterfaceStylesBuilder WithBackground(string texture, string? region, Color? color = null, float opacity = 1)
	{
		// Genera el estilo
		if (LastStyle is not null)
			LastStyle.Background = new UiBackground(LastStyle)
											{
												Sprite = new Entities.Sprites.SpriteDefinition(texture, region),
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
		// Asigna el fondo
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
	///		Crea un borde con color sólido
	/// </summary>
	public UserInterfaceStylesBuilder WithBorder(Color color, int thickness, float opacity = 1)
	{
		// Asigna el borde
		if (LastStyle is not null)
			LastStyle.Border = new UiSolidBorder(LastStyle)
												{
													Color = color,
													Thickness = thickness,
													Opacity = opacity
												};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea un borde con una textura
	/// </summary>
	public UserInterfaceStylesBuilder WithBorder(string asset, string? region, int thickness, float opacity = 1)
	{
		// Asigna el borde
		if (LastStyle is not null)
			LastStyle.Border = new UiTextureBorder(LastStyle)
													{
														Sprite = new Entities.Sprites.SpriteDefinition(asset, region),
														Thickness = thickness,
														Opacity = opacity
													};
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Crea un borde redondeado
	/// </summary>
	public UserInterfaceStylesBuilder WithRoundedBorder(Color color, int radius)
	{
		// Asigna el borde
		if (LastStyle is not null)
			LastStyle.Border = new UiRoundedBorder(LastStyle)
												{
													Color = color,
													Radius = radius
												};
		// Devuelve el generador
		return this;
	}
	
	/// <summary>
	///		Crea un borde con sombra
	/// </summary>
	public UserInterfaceStylesBuilder WithShadowBorder(Color color, Color shadowColor, Vector2 offset, int blurRadius)
	{
		// Asigna el borde
		if (LastStyle is not null)
			LastStyle.Border = new UiShadowBorder(LastStyle)
												{
													Color = color,
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
