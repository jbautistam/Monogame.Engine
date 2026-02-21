using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///     Efecto de retroceso sobre la cámara
/// </summary>
public class RecoilBehavior(AbstractCameraBase camera, Vector2 direction, float decay = 0.9f) : AbstractCameraBehavior(camera, 0)
{
    // Variables privadas
    private Vector2 _currentRecoil, _originalPosition, _computedPosition;

    /// <summary>
    ///     Añade un retroceso adicional al actual
    /// </summary>
    public void AddRecoil(float amount, Vector2 direction)
    {
        // Incrementa el retroceso actual
        _currentRecoil += direction * amount;
        // Normaliza el resultado para que no salga fuera de los límites
        _currentRecoil.ClampLength(MaxRecoil);
    }

    /// <summary>
    ///     Actualiza el estado del comportamiento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        Vector2 target;

            // Inicializa los datos
            if (!IsInitialized)
            {
                _originalPosition = Camera.State.Transform.Position;
                _computedPosition = _originalPosition;
            }
            // Calcula la posición destino
            target = _computedPosition + _currentRecoil;

            // Acumula el retroceso            
            _currentRecoil *= Decay;
            // Cambia la posición de la cámara    
            Camera.SetPosition(target);
            // Precalcula la posición para el siguiente
            _computedPosition = _computedPosition.Lerp(Camera.State.Transform.Position - _currentRecoil, RecoverySpeed * gameContext.DeltaTime);
            // Comprueba si ha terminado con el efecto
            if (_currentRecoil.LengthSquared() < 0.01f)
                IsComplete = true;
    }

    /// <summary>
    ///     Al terminar el efecto vuelve a la posición original
    /// </summary>
    protected override void OnComplete()
    {
        Camera.SetPosition(_originalPosition);
    }

    /// <summary>
    ///     Dirección del retroceso
    /// </summary>
    public Vector2 Direction { get; } = direction;

    /// <summary>
    ///     Máximo retroceso
    /// </summary>
    public float MaxRecoil { get; set; } = 20f;

    /// <summary>
    ///     Velocidad de recuperación
    /// </summary>
    public float RecoverySpeed { get; set; } = 10f;

    /// <summary>
    ///     Cantida aplicada en cada momento
    /// </summary>
    public float Decay { get; set; } = decay;
}
