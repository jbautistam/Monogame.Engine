using Bau.BauEngine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///     Base para los controles sobre los que se puede pulsar
/// </summary>
public abstract class UiElementClickable : UiElement
{
    // Variables privadas
    private float _timeFromLastClick;

    public UiElementClickable(AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position) {}

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (Enabled)
        {
            Vector2 mousePosition = Layer.Scene.SceneManager.EngineManager.InputManager.MouseManager.MousePosition;
            bool wasHovered = IsHovered;

                // Incrementa el tiempo pasado desde el último click
                _timeFromLastClick += gameContext.DeltaTime;
                // Indica que por ahora no está pulsado
                IsPressed = false;
                // Comprueba si el ratón está encima del control
                IsHovered = Position.Bounds.Contains(mousePosition);
                // Detecta clicks
                if (IsHovered)
                {
                    // Indica si se ha pulsado el elemento
                    IsPressed = Layer.Scene.SceneManager.EngineManager.InputManager.MouseManager.IsPressed(Managers.Input.MouseController.MouseStatus.MouseButton.Left);
                    // Lanza el evento de pulsación
                    if (IsPressed && _timeFromLastClick > 1f)
                    {
                        // Lanza los eventos de pulsación sobre el componente
                        UpdatePressed();
                        Layer.RaiseCommandClick(new EventArguments.ClickEventArgs(this));
                        // Inicializa el temporizador para volver a pulsar el botón
                        _timeFromLastClick = 0;
                    }
                }
                // Actualiza los elementos
                UpdateComponent(gameContext);
        }
    }

    /// <summary>
    ///     Actualiza los datos del componente
    /// </summary>
    protected abstract void UpdateComponent(Managers.GameContext gameContext);

    /// <summary>
    ///     Actualiza el estado cuando se ha pulsado sobre el elemento
    /// </summary>
    protected virtual void UpdatePressed()
    {
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibujar los datos de fondo si existe
        Layer.DrawStyle(renderingManager, Style, State, Position.ContentBounds, gameContext);
        // Dibuja el componente
        DrawComponent(renderingManager, gameContext);
    }

    /// <summary>
    ///     Dibuja el componente
    /// </summary>
    protected abstract void DrawComponent(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext);

    /// <summary>
    ///     Obtiene el estilo correspondiente al estado
    /// </summary>
    protected Styles.UiStyle? GetStyle()
    {
        if (!string.IsNullOrWhiteSpace(Style))
            return Layer.Styles.GetStyle(Style, State);
        else
            return null;
    }

    /// <summary>
    ///     Indica si el cursor está sobre el botón
    /// </summary>
    public bool IsHovered { get; private set; }

    /// <summary>
    ///     Indica si el botón está presionado
    /// </summary>
    public bool IsPressed { get; private set; }

    /// <summary>
    ///     Indica si está seleccionado
    /// </summary>
    public bool IsSelected { get; protected set; }

    /// <summary>
    ///     Estado del botón
    /// </summary>
    public Styles.UiStyle.StyleType State
    {
        get
        {
            if (!Enabled)
                return Styles.UiStyle.StyleType.Disabled;
            else if (IsPressed)
                return Styles.UiStyle.StyleType.Pressed;
            else if (IsSelected)
                return Styles.UiStyle.StyleType.Selected;
            else if (IsHovered)
                return Styles.UiStyle.StyleType.Hover;
            else
                return Styles.UiStyle.StyleType.Normal;
        }
    }
}