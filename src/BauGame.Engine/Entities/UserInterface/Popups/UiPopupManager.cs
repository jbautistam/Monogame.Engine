using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Popups;

/// <summary>
///		Manager de popups
/// </summary>
public class UiPopupManager(AbstractUserInterfaceLayer layer)
{
    /// <summary>
    ///     Respuesta de un popup
    /// </summary>
    public enum PopupResponse
    {
        /// <summary>Acepta los cambios</summary>
        Ok,
        /// <summary>Cancela los cambios</summary>
        Cancel,
        /// <summary>Reintentar</summary>
        Retry
    }
    // Variables privadas
    private List<UiAbstractPopup> _popups = [];
    private List<(float time, UiAbstractPopup popup)> _popupsToRemove = [];
    private UiAbstractPopup? _lastModal;
    private float _elapsed;
    
    /// <summary>
    ///     Añade un popup a la lista
    /// </summary>
    public void Add(UiAbstractPopup popup)
    {
        // Asigna el manejador de eventos
        popup.Closed += (sender, args) => ClosePopup(sender as UiAbstractPopup);
        // Marca este popup como el último modal
        if (popup.Type == UiAbstractPopup.PopupType.Modal)
            _lastModal = popup;
        // Añade el popup
        _popups.Add(popup);
    }
    
    /// <summary>
    ///     Cierra un popup
    /// </summary>
    private void ClosePopup(UiAbstractPopup? closedPopup)
    {
        if (closedPopup is not null)
        {
            // Quita el modal activo
            if (closedPopup.Type == UiAbstractPopup.PopupType.Modal)
                _lastModal = null;
            // Marca el popup para eliminar
            _popupsToRemove.Add((_elapsed + 3, closedPopup));
        }
    }
    
    /// <summary>
    ///     Actualiza los popups
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Incrementa el tiempo
        _elapsed += gameContext.DeltaTime;
        // Elimina los popups antiguos
        for (int index = _popupsToRemove.Count - 1; index >= 0; index--)
            if (_popupsToRemove[index].time >= _elapsed)
            {
                // Elimina el popup de la lista actual
                _popups.Remove(_popupsToRemove[index].popup);
                // Elimina el popup de la lista de elementos a eliminar
                _popupsToRemove.RemoveAt(index);
            }
        // Busca el último modal
        if (_lastModal is null)
            for (int index = _popups.Count - 1; index >= 0; index--)
                if (_lastModal is null && _popups[index].Type == UiAbstractPopup.PopupType.Modal)
                    _lastModal = _popups[index];
        // Desactiva todos los popups que no sean el actual
        if (_lastModal is not null)
        {
            // Desactiva los modales
            foreach (UiAbstractPopup popup in _popups)
                if (popup.Type == UiAbstractPopup.PopupType.Modal && popup != _lastModal) 
                    popup.Enabled = false;
            // Activa el actual
            _lastModal.Enabled = true;
        }
        // Si hay algún modal activo, detiene la escena
        Layer.Scene.IsPaused = _lastModal is not null;
        // Actualiza los popups
        foreach (UiAbstractPopup popup in _popups)
            popup.Update(gameContext);
    }
    
    /// <summary>
    ///     Dibuja los popups
    /// </summary>
    public void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibuja un fondo oscuro
        if (_lastModal is not null)
            renderingManager.FiguresRenderer.DrawRectangle(new Rectangle(0, 0, 
                                                                            renderingManager.Scene.Camera.ScreenViewport.Width, 
                                                                            renderingManager.Scene.Camera.ScreenViewport.Height), 
                                                           Color.Black * 0.75f);
        // Dibuja todos los popups abiertos
        foreach (UiAbstractPopup popup in _popups)
            if (!popup.IsClosed)
                popup.Draw(renderingManager, gameContext);
    }

    /// <summary>
    ///     Capa del interface de usuario
    /// </summary>
    public AbstractUserInterfaceLayer Layer { get; } = layer;
}
