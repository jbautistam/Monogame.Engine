using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Control de barra de progreso para el interface de usuario
/// </summary>
public class UiProgressBar : UiElement
{
    // Enumerados públicos
    /// <summary>
    ///     Orientación
    /// </summary>
    public enum OrientationMode
    {
        /// <summary>Orientación horizontal</summary>
        Horizontal,
        /// <summary>Orientación vertical</summary>
        Vertical
    }
    // Variables privadas
    private float _currentValue = 0;
    private float _animationTimer;
    private bool _isAnimating;
    private bool _isInitialized;

    public UiProgressBar(AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Position.Padding = new UiMargin(10);
    }

    /// <summary>
    ///     Inicializa el componente
    /// </summary>
    private void Initialize()
	{
        if (!_isInitialized)
        {
            // Carga las texturas
            if (!string.IsNullOrWhiteSpace(BackgroundBarTexture))
                BackgroundBarTexture2D = Layer.Scene.LoadSceneAsset<Texture2D>(BackgroundBarTexture);
            if (!string.IsNullOrWhiteSpace(Texture))
                Texture2D = Layer.Scene.LoadSceneAsset<Texture2D>(Texture);
            // Indica que ya está inicializado
            _isInitialized = true;
        }
	}

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void UpdateSelf(Managers.GameContext gameContext)
    {
        // Inicializa las texturas
        Initialize();
        // Normaliza el valor
        Value = Math.Clamp(Value, 0, Maximum);
        // Actualiza los elementos
        Background?.Update(gameContext);
        // 1. Animación de progreso (tweening suave)
        if (_currentValue != Value)
        {
            float progress = Math.Min(_animationTimer / MathHelper.Clamp(AnimationTime, 0.1f, AnimationTime), 1f);
            float eased = 1f - (1f - progress) * (1f - progress);
            
                // Cambia el valor actual interpolando
                _currentValue = MathHelper.Lerp(_currentValue, Value, eased);
                // Termina la animación
                if (progress >= 1f)
                {
                    _currentValue = Value;
                    _isAnimating = false;
                    _animationTimer = 0;
                }
                else
                    _animationTimer += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        Rectangle target, backgroundRectangle;
        float percent = _currentValue / Maximum;
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);

            // Dibuja la textura de fondo si existe
            Background?.Draw(camera, Position.Bounds, gameContext);
            // Calcular rectángulo de relleno según orientación y modo
            switch (Orientation)
            {
                case OrientationMode.Horizontal:
                        target = new Rectangle(Position.ContentBounds.X, Position.ContentBounds.Y, 
                                               (int) (Position.ContentBounds.Width * percent),
                                               Position.ContentBounds.Height);
                    break;
                default:
                        target = new Rectangle(Position.ContentBounds.X, Position.ContentBounds.Y, 
                                               Position.ContentBounds.Width,
                                               (int) (Position.ContentBounds.Height * percent));
                break;
            }
            // Calcula un rectángulo ligeramente más pequeño cuando tenemos una textura de fondo de la barra de progreso
            backgroundRectangle = target;
            if (BackgroundBarTexture2D is not null)
                target = new Rectangle(target.X + 2, target.Y + 2, target.Width - 4, target.Height - 4);
            // Dibuja las texturas
            if (BackgroundBarTexture2D is not null)
                camera.SpriteBatchController.Draw(BackgroundBarTexture2D, backgroundRectangle, style.Color * style.Opacity);
            if (Texture2D is not null)
                camera.SpriteBatchController.Draw(Texture2D, target, style.Color * style.Opacity);
    }

    /// <summary>
    ///     Fondo de todo el control
    /// </summary>
    public Backgrounds.UiBackground? Background { get; set; }

    /// <summary>
    ///     Fondo de la barra de progreso
    /// </summary>
    public string? BackgroundBarTexture { get; set; }

	/// <summary>
	///		Textura del fondo de la barra de progreso
	/// </summary>
	private Texture2D? BackgroundBarTexture2D { get; set; }

    /// <summary>
    ///     Imagen de la barra de progreso
    /// </summary>
    public string? Texture { get; set; }

	/// <summary>
	///		Textura de la barra de progreso
	/// </summary>
	private Texture2D? Texture2D { get; set; }

    /// <summary>
    ///     Valor máximo
    /// </summary>
    public int Maximum { get; set; }

    /// <summary>
    ///     Valor actual
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    ///     Orientación
    /// </summary>
    public OrientationMode Orientation { get; set; } = OrientationMode.Horizontal;

    /// <summary>
    ///     Duración de la animación del relleno
    /// </summary>
    public float AnimationTime { get; set; } = 0.1f;
}