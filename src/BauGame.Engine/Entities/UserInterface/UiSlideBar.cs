using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Componente para mostrar una barra de valores
/// </summary>
public class UiSlideBar(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Orientación de dibujo del control
    /// </summary>
    public enum SliderOrientation
    {
        /// <summary>Orientación horizontal</summary>
        Horizontal,
        /// <summary>Orientación vertical</summary>
        Vertical
    }
    /// <summary>
    ///     Modo de clip de las texturas
    /// </summary>
    public enum ClipMode
    {
        /// <summary>Ajuste de la textura</summary>
        Stretch,
        /// <summary>Ventana sobre la textura</summary>
        Window
    }
    // Variables privadas
    private Rectangle _thumbBounds;
    private int _trackLength, _trackThickness, _thumbLength, _thumbThickness;
    private float _value;

    /// <summary>
    ///     Calcula las coordenadas en pantalla
    /// </summary>
	protected override void ComputeScreenBoundsSelf()
	{
	}

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        if (IsValid())
        {
            // Actualiza las texturas
            Thumb?.Update(gameContext);
            TrackLeft?.Update(gameContext);
            TrackRight?.Update(gameContext);
            // Actualiza las variables de dimensión y la posición del thumb
            UpdateDimensions();
            UpdateThumbBounds();
            // Trata las entradas
            TreatInputs();
        }
    }

    /// <summary>
    ///     Trata las entradas
    /// </summary>
    private void TreatInputs()
    {
        bool leftButtonPressed = GameEngine.Instance.InputManager.MouseManager.IsPressed(Managers.Input.MouseController.MouseStatus.MouseButton.Left);
        Vector2 mousePosition = GameEngine.Instance.InputManager.MouseManager.MousePosition;
        bool isHovering = _thumbBounds.Contains(mousePosition) || GetTrackBounds().Contains(mousePosition);
            
            // Si estamos sobre el control y pulsamos el botón izquierdo, comenzamos el drag
            if (isHovering && leftButtonPressed)
            {
                IsDragging = true;
                UpdateValueFromMouse(mousePosition);
            }
            else if (IsDragging)
            {
                if (leftButtonPressed)
                    UpdateValueFromMouse(mousePosition);
                else if (!leftButtonPressed)
                    IsDragging = false;
            }
    }

    /// <summary>
    ///     Actualiza las dimensiones de las diferentes partes del componente dependiendo de los tamaños de las texturas
    /// </summary>
    private void UpdateDimensions()
    {
        (int width, int height) thumbSize = GetSize(Thumb, 20, 20);
        (int width, int height) leftSize = GetSize(TrackLeft, 10, 10);

    	    if (Orientation == SliderOrientation.Horizontal)
            {
                _trackLength = Position.ContentBounds.Width;
                _trackThickness = leftSize.height;
                _thumbLength = thumbSize.width;
                _thumbThickness = thumbSize.height;
            }
            else
            {
                _trackLength = Position.ContentBounds.Height;
                _trackThickness = leftSize.width;
                _thumbLength = thumbSize.height;
                _thumbThickness = thumbSize.width;
            }

        // Obtiene el ancho de un sprite teniendo en cuenta los nulos
        (int width, int height) GetSize(SpriteDefinition? sprite, int defaultWidth, int defaultHeight)
        {
            if (sprite is null)
                return (defaultWidth, defaultHeight);
            else
            {
                Common.Size size = sprite.GetSize();
                    
                    return ((int) size.Width, (int) size.Height);
            }
        }
    }

    /// <summary>
    ///     Actualiza la posición del thumb basándose en su valor
    /// </summary>
    private void UpdateThumbBounds()
    {
        float percentage = (Value - Minimum) / (Maximum - Minimum);
        int availableSpace = _trackLength - _thumbLength;
        int position = (int) (percentage * availableSpace);
            
            if (Orientation == SliderOrientation.Horizontal)
                _thumbBounds = new Rectangle(Position.ContentBounds.X + position, GetCenter(Position.ContentBounds.Y, Position.ContentBounds.Height), 
                                             _thumbLength, _thumbThickness);
            else // Si estamos en vertical el 0% está abajo y el 100% arriba, por eso lo invertimos
            {
                int centerX = GetCenter(Position.ContentBounds.X, Position.ContentBounds.Width);
                int invertedPosition = availableSpace - position;

                    _thumbBounds = new Rectangle(centerX, Position.ContentBounds.Y + invertedPosition, _thumbThickness, _thumbLength);
            }

        // Obtiene el centro
        int GetCenter(int contentPosition, int width) => contentPosition + (width - _thumbThickness) / 2;
    }

    /// <summary>
    ///     Obtiene los límites del rectángulo del track
    /// </summary>
    private Rectangle GetTrackBounds()
    {
        if (Orientation == SliderOrientation.Horizontal)
            return new Rectangle(Position.ContentBounds.X, GetTrack(Position.ContentBounds.Y, Position.ContentBounds.Height), 
                                    Position.ContentBounds.Width, _trackThickness);
        else
            return new Rectangle(GetTrack(Position.ContentBounds.X, Position.ContentBounds.Width), Position.ContentBounds.Y, 
                                 _trackThickness, Position.ContentBounds.Height);

        // Obtiene la posición del track
        int GetTrack(int position, int width) => position + (width - _trackThickness) / 2;
    }

    /// <summary>
    ///     Actualiza el valor del control dependiendo de la posición del ratón
    /// </summary>
    private void UpdateValueFromMouse(Vector2 point)
    {
        float percentage;
        int availableSpace = _trackLength - _thumbLength;

            // Normaliza el espacio
            if (availableSpace == 0)
                Value = 0;
            else
            {
                // Calcula el porcentaje dependiendo de la orientación            
                if (Orientation == SliderOrientation.Horizontal)
                    percentage = GetRelativePos(point.X, Position.ContentBounds.X) / availableSpace;
                else
                    percentage = 1f - GetRelativePos(point.Y, Position.ContentBounds.Y) / availableSpace; 
                // Asigna el valor
                Value = Minimum + MathHelper.Clamp(percentage, 0f, 1f) * (Maximum - Minimum);
            }

        // Obtiene la posición relativa del cursor a partir de la posición del control en pantalla
        float GetRelativePos(float positionCursor, float positionControl) => positionCursor - positionControl - 0.5f * _thumbLength;
    }

    /// <summary>
    ///     Dibuja los componentes del control
    /// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        // Dibuja los estilos
        Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
        // Dibuja las barras
        if (TrackLeft is not null)
        {
            if (Orientation == SliderOrientation.Horizontal)
                DrawHorizontalTrack(camera, TrackLeft, TrackRight ?? TrackLeft);
            else
                DrawVerticalTrack(camera, TrackLeft, TrackRight ?? TrackLeft);
        }
        // Dibuja el thumb encima de las barras
        if (Thumb is not null)
            camera.RenderingManager.SpriteRenderer.Draw(Thumb, _thumbBounds, Vector2.Zero, 0, Color.White);
    }

    /// <summary>
    ///     Dibuja el control en horizontal
    /// </summary>
    private void DrawHorizontalTrack(Camera2D camera, SpriteDefinition trackLeft, SpriteDefinition trackRight)
    {
        (Rectangle left, Rectangle right) = GetHorizontalRectangles();

            // Dibuja las barras
            if (Mode == ClipMode.Stretch)
            {
                if (left.Width > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackLeft, left, Vector2.Zero, 0, Color.White);
                if (right.Width > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackRight, right, Vector2.Zero, 0, Color.White);
            }
            else
            {
                if (left.Width > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackLeft, Scenes.Rendering.Renderers.SpriteRenderer.DrawMode.WindowFill, 
                                                                     left, Vector2.Zero,
                                                                     new Common.RectangleF(0, 0, GetPercentage(), 1), 0, Color.White);
                if (right.Width > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackRight, Scenes.Rendering.Renderers.SpriteRenderer.DrawMode.WindowFill, 
                                                                     right, Vector2.Zero,
                                                                     new Common.RectangleF(GetPercentage(), 0, 1, 1), 0, Color.White);
            }
    }

    /// <summary>
    ///     Dibuja el control en vertical
    /// </summary>
    private void DrawVerticalTrack(Camera2D camera, SpriteDefinition trackUp, SpriteDefinition trackBottom)
    {
        (Rectangle top, Rectangle bottom) = GetVerticalRectangles();

            // Dibuja las barras
            if (Mode == ClipMode.Stretch)
            {
                // Parte inferior: desde el thumb hacia abajo
                if (bottom.Height > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackBottom, bottom, Vector2.Zero, 0, Color.White);
                // Parte superior: desde arriba hasta el thumb
                if (top.Height > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackUp, top, Vector2.Zero, 0, Color.White);
            }
            else
            {
                // Parte inferior: desde el thumb hacia abajo
                if (bottom.Height > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackBottom, Scenes.Rendering.Renderers.SpriteRenderer.DrawMode.WindowFill, 
                                                                     bottom, Vector2.Zero, 
                                                                     new Common.RectangleF(0, GetPercentage(), 1, 1),
                                                                     0, Color.White);
                // Parte superior: desde arriba hasta el thumb
                if (top.Height > 0)
                    camera.RenderingManager.SpriteRenderer.Draw(trackUp, Scenes.Rendering.Renderers.SpriteRenderer.DrawMode.WindowFill, 
                                                                     top, Vector2.Zero, 
                                                                     new Common.RectangleF(0, 0, 1, GetPercentage()),
                                                                     0, Color.White);
            }
    }

    /// <summary>
    ///     Obtiene los rectángulos de dibujo en horizontal
    /// </summary>
    private (Rectangle left, Rectangle right) GetHorizontalRectangles()
    {
        int trackY = Position.ContentBounds.Y + (Position.ContentBounds.Height - _trackThickness) / 2;
        int leftWidth = _thumbBounds.Center.X - Position.ContentBounds.X;
        int rightStart = _thumbBounds.Center.X;
        int rightWidth = Position.ContentBounds.Right - rightStart;

            // Devuelve los rectángulos de dibujo
            return (new Rectangle(Position.ContentBounds.X, trackY, leftWidth, _trackThickness), 
                    new Rectangle(rightStart, trackY, rightWidth, _trackThickness));
    }

    /// <summary>
    ///     Obtiene los rectángulos de dibujo en vertical
    /// </summary>
    private (Rectangle top, Rectangle bottom) GetVerticalRectangles()
    {
        int trackX = Position.ContentBounds.X + (Position.ContentBounds.Width - _trackThickness) / 2;
        int bottomStart = _thumbBounds.Center.Y;
        int bottomHeight = Position.ContentBounds.Bottom - bottomStart;
        int topHeight = _thumbBounds.Center.Y - Position.ContentBounds.Y;

            // Devuelve los rectángulos de dibujo
            return (new Rectangle(trackX, Position.ContentBounds.Y, _trackThickness, topHeight),
                    new Rectangle(trackX, bottomStart, _trackThickness, bottomHeight));
    }

    /// <summary>
    ///     Obtiene el porcentaje del valor
    /// </summary>
    private float GetPercentage()
    {
        if (Maximum - Minimum == 0)
            return 0;
        else
            return Value / (Maximum - Minimum);
    }

    /// <summary>
    ///     Comprueba si el control está correctamente definido
    /// </summary>
    private bool IsValid() => Thumb is not null && TrackLeft is not null && TrackRight is not null;

    /// <summary>
    ///     Textura del thumb del control
    /// </summary>
    public SpriteDefinition? Thumb { get; set; }

    /// <summary>
    ///     Textura de la parte izquierda / superior al thumb
    /// </summary>
    public SpriteDefinition? TrackLeft { get; set; }

    /// <summary>
    ///     Textura de la parte derecha / inferior al thumb
    /// </summary>
    public SpriteDefinition? TrackRight { get; set; }
        
    /// <summary>
    ///     Valor actual del control
    /// </summary>
    public float Value 
    { 
        get { return _value; } 
        set
        {
            if (_value != value)
            {
                float oldValue = _value;

                    // Actualiza el valor
                    _value = value;
                    // Lanza el evento de modificación
                    Layer.RaiseValueChanged(new EventArguments.ValueChangedEventArgs(this, oldValue, _value));
            }
        }
    }

    /// <summary>
    ///     Valor mínimo para el control
    /// </summary>
    public float Minimum { get; set; }

    /// <summary>
    ///     Valos máximo para el control
    /// </summary>
    public float Maximum { get; set; } = 100;

    /// <summary>
    ///     Paso
    /// </summary>
    public float Step { get; set; } = 1;

    /// <summary>
    ///     Orientación del control
    /// </summary>
    public SliderOrientation Orientation { get; set; } = SliderOrientation.Horizontal;

    /// <summary>
    ///     Modo de dibujo
    /// </summary>
    public ClipMode Mode { get; set;  } = ClipMode.Stretch;

    /// <summary>
    ///     Espacio entre el thumb y las barras
    /// </summary>
    public int ThumbPadding { get; set; } = 2;

    /// <summary>
    ///     Indica si se está arrastrando el thumb
    /// </summary>
    public bool IsDragging { get; private set; }
}