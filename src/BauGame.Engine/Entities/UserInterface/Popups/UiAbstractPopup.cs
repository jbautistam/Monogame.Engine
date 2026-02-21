namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups;

/// <summary>
///     Definición de un popup
/// </summary>
public abstract class UiAbstractPopup : UiElement
{
    /// <summary>
    ///     Tipo de popup
    /// </summary>
    public enum PopupType
    {
        /// <summary>Cuadro de diálogo</summary>
        Modal,
        /// <summary>Ventana no modal</summary>
        NonModal
    }
    // Eventos públicos    
    public event EventHandler<EventArguments.PopupClosedEventArgs>? Closed;
    // Variables privadas
    private UiButton? _closeButton;
    
    protected UiAbstractPopup(UiPopupManager manager, UiPosition position, PopupType type) : base(manager.Layer, position)
    {
        Body = new UiPanel(manager.Layer, position);
        Type = type;
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
    {
        //TODO: debería recolocar los elementos
    }

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (!IsClosed)
        {
            // Actualiza el popup
            UpdatePopup(gameContext);
            // Actualiza los componentes
            Header?.Update(gameContext);
            Body?.Update(gameContext);
            Footer?.Update(gameContext);
        }
    }

    /// <summary>
    ///     Actualiza el popup
    /// </summary>
    protected abstract void UpdatePopup(Managers.GameContext gameContext);

    /// <summary>
    ///     Dibuja el elemento
    /// </summary>
    public override void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        if (!IsClosed)
        {
            Header?.Draw(camera, gameContext);
            Body?.Draw(camera, gameContext);
            Footer?.Draw(camera, gameContext);
        }
    }
    
    /// <summary>
    ///     Cierra el popup
    /// </summary>
    public void Close(UiPopupManager.PopupResponse result)
    {
        // Lanza el evento
        Closed?.Invoke(this, new EventArguments.PopupClosedEventArgs(result));
        // Desactiva el popup
        Enabled = false;
        IsClosed = true;
    }

    /// <summary>
    ///     Tipo
    /// </summary>
    public PopupType Type { get; }

    /// <summary>
    ///     Indica si se ha cerrado
    /// </summary>
    public bool IsClosed { get; private set; }

    /// <summary>
    ///     Cabecera del popup
    /// </summary>
    public UiPanel? Header { get; set; }
    
    /// <summary>
    ///     Cuerpo del popup
    /// </summary>
    public UiPanel? Body { get; set; }

    /// <summary>
    ///     Pie del popup
    /// </summary>
    public UiPanel? Footer { get; set; }

    /// <summary>
    ///     Botón para cerrar el cuadro de diálogo
    /// </summary>
    public UiButton? CloseButton
    {
        get { return _closeButton; }
        set
        {
            _closeButton = value;
            if (_closeButton is not null)
                _closeButton.Click += (sender, args) => Close(UiPopupManager.PopupResponse.Cancel);
        }
    }
}
