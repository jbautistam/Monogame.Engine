namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups;

/// <summary>
///     Cuadro de diálogo no modal
/// </summary>
public abstract class UiNonModalPopup(UiPopupManager manager, UiPosition position) : UiAbstractPopup(manager, position, PopupType.NonModal)
{
    // Variables privadas
    private UiButton? _cancelButton, _okButton;

    /// <summary>
    ///     Botón para aceptar el cuadro de diálogo
    /// </summary>
    public UiButton? AcceptButton
    {
        get { return _okButton; }
        set
        {
            _okButton = value;
            if (_okButton is not null)
                _okButton.Click += (sender, args) => Close(UiPopupManager.PopupResponse.Ok);
        }
    }

    /// <summary>
    ///     Botón para cancelar el cuadro de diálogo
    /// </summary>
    public UiButton? CancelButton
    {
        get { return _cancelButton; }
        set
        {
            _cancelButton = value;
            if (_cancelButton is not null)
                _cancelButton.Click += (sender, args) => Close(UiPopupManager.PopupResponse.Cancel);
        }
    }
}
