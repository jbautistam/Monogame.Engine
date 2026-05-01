namespace Bau.BauEngine.Scenes.Cameras.Behaviors;

/// <summary>
///     Base de los comportamientos de cámara
/// </summary>
public abstract class AbstractCameraBehavior(Definitions.AbstractCameraBase camera, float duration) : Entities.GameObjects.AbstractEntityWithDuration(duration), Entities.Common.Collections.ISecureListItem
{
    /// <summary>
    ///     Cámara a la que se asocia el efecto
    /// </summary>
    public Definitions.AbstractCameraBase Camera { get; } = camera;
}