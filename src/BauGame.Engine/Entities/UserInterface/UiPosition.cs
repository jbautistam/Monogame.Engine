using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///		Posición y tamaño de los controles
/// </summary>
public class UiPosition(float x, float y, float width, float height, bool relative = true)
{
    /// <summary>
    ///     Tipo de anclaje
    /// </summary>
    public enum AnchorType
    {
        None,
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
    /// <summary>
    ///     Tipo de dock
    /// </summary>
    public enum DockType
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        Fill
    }

    /// <summary>
    ///     Calcula la posición en pantalla
    /// </summary>
    public void ComputeLayout(Rectangle parentBounds)
    {
        (int width, int height) = ComputeSize();
        Rectangle dockedBounds = ApplyDocking(parentBounds, width, height);
        Point finalPosition = CalculateAnchoredPosition(dockedBounds, parentBounds);

            // Asigna los límites de la pantalla al elemento
            Bounds = new Rectangle(finalPosition.X, finalPosition.Y, dockedBounds.Width, dockedBounds.Height);
            if (Margin is not null)
                Bounds = (Margin ?? new UiMargin(0)).Apply(Bounds);
            // Asigna los límites internos
            ContentBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            if (Padding is not null)
                ContentBounds = (Padding ?? new UiMargin(0)).Apply(Bounds);

        // Calcula el tamaño
        (int width, int heigh) ComputeSize()
        {
            if (Relative)
                return ((int) (parentBounds.Width * MathHelper.Clamp(Width, 0, 1)),
                        (int) (parentBounds.Height * MathHelper.Clamp(Height, 0, 1)));
            else
                return ((int) Width, (int) Height);
        }
    }

    /// <summary>
    ///     Aplica el dock
    /// </summary>
    private Rectangle ApplyDocking(Rectangle parentBounds, int width, int height)
    {
	    return Dock switch
	        {
		        DockType.Top => new Rectangle(parentBounds.X, parentBounds.Y, parentBounds.Width, height),
		        DockType.Bottom => new Rectangle(parentBounds.X, parentBounds.Bottom - height, parentBounds.Width, height),
		        DockType.Left => new Rectangle(parentBounds.X, parentBounds.Y, width, parentBounds.Height),
		        DockType.Right => new Rectangle(parentBounds.Right - width, parentBounds.Y, width, parentBounds.Height),
		        DockType.Fill => parentBounds,
		        _ => new Rectangle(0, 0, width, height),
	        };
	}

    /// <summary>
    ///     Calcula la posición teniendo en cuenta los anclajes
    /// </summary>
    private Point CalculateAnchoredPosition(Rectangle elementBounds, Rectangle parentBounds)
    {
        int x = elementBounds.X;
        int y = elementBounds.Y;

            // Si no está ajustado a los bordes
            if (Dock == DockType.None)
            {
                // Calcular posición relativa
                if (Relative)
                {
                    x = parentBounds.X + (int) (parentBounds.Width * X);
                    y = parentBounds.Y + (int) (parentBounds.Height * Y);
                }
                else
                {
                    x = parentBounds.X + (int) X;
                    y = parentBounds.Y + (int) Y;
                }
                // Aplica el anchor
                switch (Anchor)
                {
                    case AnchorType.TopCenter:
                            x -= elementBounds.Width / 2;
                        break;
                    case AnchorType.TopRight:
                            x -= elementBounds.Width;
                        break;
                    case AnchorType.MiddleLeft:
                            y -= elementBounds.Height / 2;
                        break;
                    case AnchorType.MiddleCenter:
                            x -= elementBounds.Width / 2;
                            y -= elementBounds.Height / 2;
                        break;
                    case AnchorType.MiddleRight:
                            x -= elementBounds.Width;
                            y -= elementBounds.Height / 2;
                        break;
                    case AnchorType.BottomLeft:
                            y -= elementBounds.Height;
                        break;
                    case AnchorType.BottomCenter:
                            x -= elementBounds.Width / 2;
                            y -= elementBounds.Height;
                        break;
                    case AnchorType.BottomRight:
                            x -= elementBounds.Width;
                            y -= elementBounds.Height;
                        break;
                }
            }
            // Devuelve el punto calculado
            return new Point(x, y);
    }

	/// <summary>
	///		Posición X del elemento
	/// </summary>
	public float X { get; } = x;

	/// <summary>
	///		Posición Y del elemento
	/// </summary>
	public float Y { get; } = y;

	/// <summary>
	///		Ancho
	/// </summary>
	public float Width { get; } = width;

	/// <summary>
	///		Alot
	/// </summary>
	public float Height { get; } = height;

	/// <summary>
	///		Límites en la pantalla
	/// </summary>
	public Rectangle Bounds { get; set; } = new((int) x, (int) y, (int) width, (int) height);

	/// <summary>
	///		Límites del contenido interno en la pantalla
	/// </summary>
	public Rectangle ContentBounds { get; set; } = new((int) x, (int) y, (int) width, (int) height);

	/// <summary>
	///		Indica si la posición es relativa
	/// </summary>
	public bool Relative { get; } = relative;

    /// <summary>
    ///     Margen
    /// </summary>
    public UiMargin? Margin { get; set; }

    /// <summary>
    ///     Espaciado interno
    /// </summary>
    public UiMargin? Padding { get; set; }

    /// <summary>
    ///     Dock del elemento
    /// </summary>
    public DockType Dock { get; set; } = DockType.None;

    /// <summary>
    ///     Anclaje del elemento
    /// </summary>
    public AnchorType Anchor { get; set; } = AnchorType.None;
}