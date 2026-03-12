using Microsoft.Xna.Framework;

namespace UI.ParticlesEngine.MathTools;

/// <summary>
///		Tabla precompilada de valores trigonométricos: senos, cosenos, tangentes
/// </summary>
public class TrigonometricPrecomputed(int resolution = 3_600)
{
    // Registros privados
    private record Trigonometric(float Sine, float Cosine, float Tangent);
    // Variables privadas
    private Trigonometric[] _table = new Trigonometric[1];
    private float _indexToAngle;

    /// <summary>
    ///     Inicializa la tabla
    /// </summary>
    public void Initialize()
    {
        // Crea la tabla e inicializa el índice
        _table = new Trigonometric[resolution];
        _indexToAngle = MathHelper.TwoPi / resolution;
        // Crea los elementos de la tabla
        for (int index = 0; index < resolution; index++)
        {
            float angle = index * _indexToAngle;

                // Crea los elementos de la tabla
                _table[index] = new Trigonometric((float) Math.Cos(angle), (float) Math.Sin(angle), (float) Math.Tan(angle));
        }
    }

    /// <summary>
    ///     Obtiene el seno de un ángulo en radianes
    /// </summary>
    public float Sin(float angle)
    {
        (float floatIndex, int index0, int index1) = GetIndices(angle);

            // Interpolación lineal entre los dos valores de la tabla
            return MathHelper.Lerp(_table[index0].Sine, _table[index1].Sine, floatIndex - index0);
    }

    /// <summary>
    ///     Obtiene el coseno de un ángulo en radianes
    /// </summary>
    public float Cos(float angle)
    {
        (float floatIndex, int index0, int index1) = GetIndices(angle);

            // Interpolación lineal entre los dos valores de la tabla
            return MathHelper.Lerp(_table[index0].Cosine, _table[index1].Cosine, floatIndex - index0);
    }

    /// <summary>
    ///     Obtiene la tangente de un ángulo en radianes
    /// </summary>
    public float Tan(float angle)
    {
        (float floatIndex, int index0, int index1) = GetIndices(angle);

            // Interpolación lineal entre los dos valores de la tabla
            return MathHelper.Lerp(_table[index0].Tangent, _table[index1].Tangent, floatIndex - index0);
    }

    /// <summary>
    ///     Obtiene los índices necesarios para interpolar el valor de una función
    /// </summary>
    private (float floatIndex, int index0, int index1) GetIndices(float angle)
    {
        float floatIndex = NormalizeAngle(angle) / _indexToAngle;

            // Devuelve los índices
            return (floatIndex, (int) floatIndex, ((int) floatIndex + 1) % resolution);
    }

    /// <summary>
    ///     Versión ultra-rápida de la función seno sin interpolación (si la resolución es alta)
    /// </summary>
    public float SinFast(float angle) => _table[GetFastIndex(angle)].Sine;

    /// <summary>
    ///     Versión ultra-rápida de la función coseno sin interpolación (si la resolución es alta)
    /// </summary>
    public float CosFast(float angle) => _table[GetFastIndex(angle)].Cosine;

    /// <summary>
    ///     Versión ultra-rápida de la función tangente sin interpolación (si la resolución es alta)
    /// </summary>
    public float TanFast(float angle) => _table[GetFastIndex(angle)].Tangent;

    /// <summary>
    ///     Obtiene el índice para el cálculo de funciones rápidas sin interpolación
    /// </summary>
    private int GetFastIndex(float angle) => (int) (NormalizeAngle(angle) / _indexToAngle) % resolution;

    /// <summary>
    ///     Normaliza el ángulo de 0 a 2π
    /// </summary>
    private float NormalizeAngle(float angle)
    {
        angle = angle % MathHelper.TwoPi;
        if (angle < 0) 
            return angle + MathHelper.TwoPi;
        else
            return angle;
    }
}