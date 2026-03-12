namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.ProgressBars;

/// <summary>
///     Configuración de animación para la barra de progreso
/// </summary>
public class ProgressAnimation
{
    /// <summary>
    ///     Obtiene el progreso
    /// </summary>
    public float GetValue(float delta, float currentValue, float targetValue)
    {
        if (Enabled && Math.Abs(currentValue - targetValue) > 0.001f)
        {
            float progress = 1;
            float value = targetValue;

                // Incrementa el tiempo pasado
                Elapsed += delta;
                // Calcula el prgreso
                if (Duration > 0)
                    progress = Elapsed / Duration;
                // Devuelve el valor interpolado
                return Tools.MathTools.Easing.EasingFunctionsHelper.Interpolate(currentValue, targetValue, progress, Easing);
        }
        else
            return targetValue;
    }

    /// <summary>
    ///     Indica si se debe animar la barra de progreso
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     Tiempo pasado desde el inicio de la animación
    /// </summary>
    public float Elapsed { get; private set; }

    /// <summary>
    ///     Duración de la animación
    /// </summary>
    public float Duration { get; set; } = 5f;

    /// <summary>
    ///     Modo de ajuste
    /// </summary>
    public Tools.MathTools.Easing.EasingFunctionsHelper.EasingType Easing { get; set; } = Tools.MathTools.Easing.EasingFunctionsHelper.EasingType.Linear;
}
