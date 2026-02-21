using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///     Comportamiento para darle una sacudida a la cámara
/// </summary>
public class ShakeBehavior(AbstractCameraBase camera, float duration, float intensity = 10, float frequency = 20) : AbstractCameraBehavior(camera, duration)
{
    // Variables privadas
    private float _currentTrauma;
    private Vector2 _originalPosition;
    private float _seed = Tools.Randomizer.GetRandom(0, 100f);

    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        // Inicializa con los valores de la cámara
        if (!IsInitialized)
            _originalPosition = Camera.State.Transform.Position;
        // Calacula el efecto
        _currentTrauma = MathHelper.Clamp(1f - Progress, 0, 1);
        _currentTrauma = _currentTrauma * _currentTrauma;
        // Cambia la posición de la cámara            
        Camera.SetPosition(_originalPosition + GetOffset(_currentTrauma));

        // Obtiene el desplazamiento sobre la posición original
        Vector2 GetOffset(float trauma)
        {
            float shake = trauma * Intensity;
            float noiseX = PerlinNoise(_seed + ElapsedTime * Frequency);
            float noiseY = PerlinNoise(_seed + 100f + ElapsedTime * Frequency);

                // Devuelve el vector de desplazamiento
                return new Vector2(noiseX * shake, noiseY * shake);
        }
    }

    /// <summary>
    ///     Al terminar recupera la posición original
    /// </summary>
    protected override void OnComplete()
    {
        Camera.SetPosition(_originalPosition);
    }

    /// <summary>
    ///     Ruido Perlin sobre X
    /// </summary>
    private float PerlinNoise(float x) => (float) (Math.Sin(x) * 0.5 + Math.Sin(x * 2.3) * 0.25 + Math.Sin(x * 4.7) * 0.125);

    /// <summary>
    ///     Intensidad del efecto
    /// </summary>
    public float Intensity { get; set; } = intensity;

    /// <summary>
    ///     Frecuencia del efecto
    /// </summary>
    public float Frequency { get; set; } = frequency;
}