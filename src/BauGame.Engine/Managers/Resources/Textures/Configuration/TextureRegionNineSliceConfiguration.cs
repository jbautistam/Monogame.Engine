using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Managers.Resources.Textures.Configuration;

/// <summary>
///		Configuración de una textura NineSlice
/// </summary>
public class TextureRegionNineSliceConfiguration
{
	/// <summary>
	///		Ancho de la esquina superior izquierda
	/// </summary>
	public int TopLeftWidth { get; set; }

	/// <summary>
	///		Alto de la esquina superior izquierda
	/// </summary>
	public int TopLeftHeight { get; set; }

	/// <summary>
	///		Ancho de la esquina superior derecha
	/// </summary>
	public int TopRightWidth { get; set; }

	/// <summary>
	///		Alto de la esquina superior derecha
	/// </summary>
	public int TopRightHeight { get; set; }

	/// <summary>
	///		Ancho de la esquina inferior izquierda
	/// </summary>
	public int BottomLeftWidth { get; set; }

	/// <summary>
	///		Alto de la esquina inferior izquierda
	/// </summary>
	public int BottomLeftHeight { get; set; }

	/// <summary>
	///		Ancho de la esquina inferior derecha
	/// </summary>
	public int BottomRightWidth { get; set; }

	/// <summary>
	///		Alto de la esquina inferior derecha
	/// </summary>
	public int BottomRightHeight { get; set; }

	/// <summary>
	///		Indica si se debe rellenar el fondo
	/// </summary>
	public bool FillBackground { get; set; }

	/// <summary>
	///		Color de fondo (si se rellena)
	/// </summary>
	public Color BackgroundColor { get; set; } = Color.White;

	/// <summary>
	///		Opacidad del fondo
	/// </summary>
	public float BackgroundOpacity { get; set; } = 1;

	/// <summary>
	///		Altura del borde superior
	/// </summary>
	public int TopEdgeHeight => Math.Max(TopLeftHeight, TopRightHeight);

	/// <summary>
	///		Altura del borde inferior
	/// </summary>
	public int BottomEdgeHeight => Math.Max(BottomLeftHeight, BottomRightHeight);

	/// <summary>
	///		Altura sumada de los bordes
	/// </summary>
	public int EdgesHeight => TopEdgeHeight + BottomEdgeHeight;

	/// <summary>
	///		Anchura del borde izquierdo
	/// </summary>
	public int LeftEdgeWidth => Math.Max(TopLeftWidth, BottomLeftWidth);

	/// <summary>
	///		Anchura del borde derecho
	/// </summary>
	public int RightEdgeWidth => Math.Max(TopRightWidth, BottomRightWidth);

	/// <summary>
	///		Anchura sumada de los bordes
	/// </summary>
	public int EdgesWidth => LeftEdgeWidth + RightEdgeWidth;
}
