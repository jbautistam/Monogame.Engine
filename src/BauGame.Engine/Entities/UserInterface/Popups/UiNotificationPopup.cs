namespace Bau.BauEngine.Entities.UserInterface.Popups;

/// <summary>
///     Popup de notificación
/// </summary>
public abstract class UiNotificationPopup(UiPopupManager manager, UiPosition position, float duration) : UiAbstractPopup(manager, position, PopupType.NonModal)
{
    // Variables privadas
    private float _elapsed = 0;

    /// <summary>
    ///     Actualiza el popup
    /// </summary>
    protected override void UpdatePopup(Managers.GameContext gameContext)
    {
        // Actualiza el tiempo pasado
        _elapsed += gameContext.DeltaTime;
        // Cierra la notificación si se ha pasado el momento
        if (_elapsed >= Duration)
            Close(UiPopupManager.PopupResponse.Cancel);
    }

    /// <summary>
    ///     Duración de la notificación en pantalla
    /// </summary>
    public float Duration { get; set; } = duration;
}
