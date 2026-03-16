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
    
    protected UiAbstractPopup(UiPopupManager manager, UiPosition position, PopupType type) : base(manager.Layer, position)
    {
        Body = new UiPanel(manager.Layer, position);
        Type = type;
        Layer.Click += (sender, args) => TreatClick(args);
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
    protected override void UpdateSelf(Managers.GameContext gameContext)
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
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        if (!IsClosed)
        {
            Header?.Draw(renderingManager, gameContext);
            Body?.Draw(renderingManager, gameContext);
            Footer?.Draw(renderingManager, gameContext);
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
    ///     Trata las pulsaciones sobre el botón
    /// </summary>
	private void TreatClick(EventArguments.ClickEventArgs args)
	{
        if (CloseButton is not null && args.Component.Id.Equals(CloseButton.Id, StringComparison.CurrentCultureIgnoreCase))
            Close(UiPopupManager.PopupResponse.Cancel);
        else
            TreatClickSelf(args);
	}

    /// <summary>
    ///     Trata las pulsaciones sobre el botón
    /// </summary>
    protected abstract void TreatClickSelf(EventArguments.ClickEventArgs args);

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
    public UiButton? CloseButton { get; set; }
}
