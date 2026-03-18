using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Precomputed;

/// <summary>
///     Tabla de ruido (valores aleatorios) precalculada
/// </summary>
public class NoisePrecomputed(int size = 256)
{
    // Variables privadas
    private float[] _noise = new float[1];

    /// <summary>
    ///     Inicializa la tabla: crea un valor aleatorio a partir de una semilla fija por consistencia
    /// </summary>
    public void Initialize()
    {
        Random random = new(42); // Seed fija para consistencia

            // Crea la tabla
            _noise = new float[size];
            // Rellena los valores
            for (int index = 0; index < size; index++)
                _noise[index] = (float) (random.NextDouble() * 2 - 1); // -1 a 1
    }

    /// <summary>
    ///     Obtiene el valor de ruido
    /// </summary>
    public float GetNoise(float t)
    {
        float floatIndex = Normalize(t) * _noise.Length;
        int index0 = (int) floatIndex;
        int index1 = (index0 + 1) % _noise.Length;
        float interp = floatIndex - index0;

            // Devuelve un valor interpolado
            return MathHelper.Lerp(_noise[index0], _noise[index1], interp);

        // Normaliza t para que vaya de 0 a 1
        float Normalize(float t)
        {
            t = t % 1f;
            if (t < 0) 
                return t + 1f;
            else
                return t;
        }
    }
}
