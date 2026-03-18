using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Precomputed;

/// <summary>
///     Valores del cálculo de raíz cuadrada precalculados
/// </summary>
public class SquareRootPrecomputed(int resolution)
{
    // Variables privadas
    private float[] _table = new float[1];
    private float _indexToValue;

    /// <summary>
    ///     Inicializa la tabla
    /// </summary>
    public void Initialize()
    {
        // Crea las variables globales
        _table = new float[resolution];
        _indexToValue = 1f / (resolution - 1);
        // Rellena la tabla
        for (int index = 0; index < resolution; index++)
        {
            float value = index * _indexToValue; // 0 a 1

                // Obtiene el valor de la función
                _table[index] = (float) Math.Sqrt(value);
        }
    }

    /// <summary>
    ///     Obtiene la raíz cuadrada de un valor
    /// </summary>
    public float Sqrt(float value)
    {
        float floatIndex = MathHelper.Clamp(value, 0, 1) / _indexToValue;
        int index0 = (int) floatIndex;
        int index1 = Math.Min(index0 + 1, resolution - 1);

            // Devuelve el valor interpolado
            return MathHelper.Lerp(_table[index0], _table[index1], floatIndex - index0);
    }

    /// <summary>
    ///     Obtiene la raíz cuadrada de un valor sin interpolación
    /// </summary>
    public float SqrtFast(float value)
    {
        int index = (int) (MathHelper.Clamp(value, 0, 1) / _indexToValue);

            // Devuelve el valor de la función
            return _table[Math.Min(index, resolution - 1)];
    }
}
