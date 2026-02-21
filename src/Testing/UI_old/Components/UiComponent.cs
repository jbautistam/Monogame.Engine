using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI;

/// <summary>
///     Componente base para interface de usuario
/// </summary>
public abstract class UiComponent
{
	/// <summary>
	///     Tipos de anclaje
	/// </summary>
	public enum AnchorType
    {
        /// <summary>Coordenada superior izquierda</summary>
        TopLeft, 
        /// <summary>Centro de la parte superior</summary>
        TopCenter, 
        /// <summary>Coordenada supoerior derecha</summary>
        TopRight,
        /// <summary>Centro de la parte izquierda</summary>
        MiddleLeft, 
        /// <summary>Centro</summary>
        Center, 
        /// <summary>Centro de la parte derecha</summary>
        MiddleRight,
        /// <summary>Coordenada inferior izquiera</summary>
        BottomLeft, 
        /// <summary>Centro de la parte inferior</summary>
        BottomCenter, 
        /// <summary>Coordenada inferior derecha</summary>
        BottomRight,
        /// <summary>Expande para llenar el espacio disponible</summary>
        Stretch,
        /// <summary>Expande para llenar el espacio disponible ignorando los márgenes</summary>
        Fill
    }
    // Variables privadas
    private Rectangle _absoluteBounds, _contentBounds;
    private bool _transformDirty = true;

    /// <summary>
    ///     Fuerza que se recalculen los datos absolutos
    /// </summary>
    public void Invalidate()
    {
        // Indica que hay que corregir el cálculo de tamaños
        _transformDirty = true;
        // Actualiza el componente en concreto
        InvalidateSelf();
    }

    /// <summary>
    ///     Actualiza los datos absolutos forzando el recálulo
    /// </summary>
    protected abstract void InvalidateSelf();

    /// <summary>
    ///     Calcula las coordenadas absolutas
    /// </summary>
    protected virtual void CalculateTransformations()
    {
        if (!_transformDirty)
        {
            Rectangle parentBounds;
            
            // Obtiene las dimensiones del viewport del padre o de toda la pantalla
            if (Parent is null)
                parentBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            else
                parentBounds = Parent.ContentBounds;

            // Aplica margen al rectángulo disponible
            int marginLeft = (int) (Margin.Left * parentBounds.Width);
            int marginTop = (int) (Margin.Top * parentBounds.Height);
            int marginRight = (int) (Margin.Right * parentBounds.Width);
            int marginBottom = (int) (Margin.Bottom * parentBounds.Height);

            Rectangle availableBounds = new(parentBounds.X + marginLeft, parentBounds.Y + marginTop,
                                            parentBounds.Width - marginLeft - marginRight,
                                            parentBounds.Height - marginTop - marginBottom);

            // Convierte coordenadas relativas a absolutas
            _absoluteBounds = RelativeBounds.ToAbsolute(availableBounds.Width, availableBounds.Height);
            // Ajusta a la posición disponible
            _absoluteBounds.X += availableBounds.X;
            _absoluteBounds.Y += availableBounds.Y;
            // Aplica ancla si no ocupa todo el espacio
            ApplyAnchor(availableBounds);

            // Calcula área de contenido (aplicando padding y el ancho de los bordes)
            int padLeft = (int) (Padding.Left * _absoluteBounds.Width);
            int padTop = (int) (Padding.Top * _absoluteBounds.Height);
            int padRight = (int) (Padding.Right * _absoluteBounds.Width);
            int padBottom = (int) (Padding.Bottom * _absoluteBounds.Height);
            int borderThickness = Border?.Thickness ?? 0;

            // Calcula el área del contenido
            _contentBounds = new Rectangle(_absoluteBounds.X + padLeft + borderThickness,
                                            _absoluteBounds.Y + padTop + borderThickness,
                                            _absoluteBounds.Width - padLeft - padRight - 2 * borderThickness,
                                            _absoluteBounds.Height - padTop - padBottom - 2 * borderThickness);
            // Indica que se ha recalculado
            _transformDirty = false;
        }
    }

    /// <summary>
    ///     Aplica el anclaje a los límites
    /// </summary>
    private void ApplyAnchor(Rectangle availableBounds)
    {
        // Si el componente no llena el espacio, ajusta según ancla
        if (Anchor != AnchorType.Stretch && Anchor != AnchorType.Fill && Anchor != AnchorType.TopLeft)
        {
            float centerX = availableBounds.X + 0.5f * availableBounds.Width;
            float centerY = availableBounds.Y + 0.5f * availableBounds.Height;

                // Ajusta los límites dependiendo del anclaje
                switch (Anchor)
                {
                    case AnchorType.TopCenter:
                            _absoluteBounds.X = (int) (centerX - 0.5f * _absoluteBounds.Width);
                        break;
                    case AnchorType.TopRight:
                            _absoluteBounds.X = availableBounds.Right - _absoluteBounds.Width;
                        break;
                    case AnchorType.MiddleLeft:
                            _absoluteBounds.Y = (int) (centerY - 0.5f * _absoluteBounds.Height);
                        break;
                    case AnchorType.Center:
                            _absoluteBounds.X = (int) (centerX - 0.5f * _absoluteBounds.Width);
                            _absoluteBounds.Y = (int) (centerY - 0.5f * _absoluteBounds.Height);
                        break;
                    case AnchorType.MiddleRight:
                            _absoluteBounds.X = availableBounds.Right - _absoluteBounds.Width;
                            _absoluteBounds.Y = (int) (centerY - 0.5f * _absoluteBounds.Height);
                        break;
                    case AnchorType.BottomLeft:
                            _absoluteBounds.Y = availableBounds.Bottom - _absoluteBounds.Height;
                        break;
                    case AnchorType.BottomCenter:
                            _absoluteBounds.X = (int) (centerX - 0.5f * _absoluteBounds.Width);
                            _absoluteBounds.Y = availableBounds.Bottom - _absoluteBounds.Height;
                        break;
                    case AnchorType.BottomRight:
                            _absoluteBounds.X = availableBounds.Right - _absoluteBounds.Width;
                            _absoluteBounds.Y = availableBounds.Bottom - _absoluteBounds.Height;
                        break;
                }
        }
    }

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (Enabled)
            UpdateSelf(gameTime);
    }

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
    protected abstract void UpdateSelf(GameTime gameTime);

    /// <summary>
    ///     Dibuja el componente
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (Visible)
        {
            // Dibuja el fondo si existe
            if (Background is not null)
                Background.Draw(spriteBatch, AbsoluteBounds, ContentBounds);
            // Dibuja este componente fondo propio
            DrawSelf(spriteBatch, gameTime);
            // Dibuja el borde, si existe, encima de todo lo demás
            if (Border is not null)
                Border.Draw(spriteBatch, AbsoluteBounds);
        }
    }

    /// <summary>
    ///     Dibuja los datos específicos del componente
    /// </summary>
    protected abstract void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime);

    protected GraphicsDevice GraphicsDevice => Parent?.GraphicsDevice ?? GetRootGraphicsDevice();

    private GraphicsDevice GetRootGraphicsDevice()
    {
        // En una implementación real, esto vendría de un UIManager o similar
        throw new InvalidOperationException(
            "Componente raíz necesita un GraphicsDevice asignado. " +
            "Usa UIManager para establecer el contexto gráfico."
        );
    }

    /// <summary>
    ///     Identificador del componente
    /// </summary>
    public string Name { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Componente padre
    /// </summary>
    public UiComponent? Parent { get; private set; }

    /// <summary>
    ///     Indica si el elemento está visible
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    ///     Indica si el elemento está activo
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Coordenadas relativas
    /// </summary>
    public RectangleRelative RelativeBounds { get; set; } = new(0, 0, 1, 1);

    /// <summary>
    ///     Padding interno (espacio entre borde y contenido)
    /// </summary>
    public Spacing Padding { get; set; } = new(0);

    /// <summary>
    ///     Margen externo (espacio reservado fuera del componente)
    /// </summary>
    public Spacing Margin { get; set; } = new(0);

    /// <summary>
    ///     Anclaje para posicionamiento automático
    /// </summary>
    public AnchorType Anchor { get; set; } = AnchorType.TopLeft;

    /// <summary>
    ///     Coordenadas absolutas
    /// </summary>
    public Rectangle AbsoluteBounds 
    { 
        get 
        {
            CalculateTransformations();
            return _absoluteBounds;
        }
    }

    /// <summary>
    ///     Coordenadas del contenido (aplicando el padding)
    /// </summary>
    public Rectangle ContentBounds
    {
        get
        {
            CalculateTransformations();
            return _contentBounds;
        }
    }

    /// <summary>
    ///     Fondo del componente
    /// </summary>
    public Brushes.IBackground? Background { get; set; }

    /// <summary>
    ///     Borde del componente
    /// </summary>
    public Brushes.IBorder? Border { get; set; }
}
