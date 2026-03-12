namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups;

/// <summary>
///     Cuadro de diálogo no modal
/// </summary>
public abstract class UiNonModalPopup(UiPopupManager manager, UiPosition position) : UiAbstractPopup(manager, position, PopupType.NonModal)
{
    /// <summary>
    ///     Trata las pulsaciones sobre los botones
    /// </summary>
	protected override void TreatClickSelf(EventArguments.ClickEventArgs args)
	{
        if (AcceptButton is not null && args.Component.Id.Equals(AcceptButton.Id, StringComparison.CurrentCultureIgnoreCase))
            Close(UiPopupManager.PopupResponse.Ok);
        else if (CancelButton is not null && args.Component.Id.Equals(CancelButton.Id, StringComparison.CurrentCultureIgnoreCase))
            Close(UiPopupManager.PopupResponse.Cancel);
	}

    /// <summary>
    ///     Botón para aceptar el cuadro de diálogo
    /// </summary>
    public UiButton? AcceptButton { get; set; }

    /// <summary>
    ///     Botón para cancelar el cuadro de diálogo
    /// </summary>
    public UiButton? CancelButton { get; set; }
}
