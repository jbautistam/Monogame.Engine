namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Precomputed;

/// <summary>
///     Clase con funciones matemáticas precompiladas
/// </summary>
public class PrecomputedFunctions
{
    // Variables privadas
    private TrigonometricPrecomputed? _sineCosine;
    private SquareRootPrecomputed? _squareRoot;
    private NoisePrecomputed? _noise;

    /// <summary>
    ///     Calcula el seno de un ángulo desde la tabla precalculada
    /// </summary>
    public float Sin(float angle) => Trigonometric.Sin(angle);

    /// <summary>
    ///     Calcula el coseno de un ángulo desde la tabla precalculada
    /// </summary>
    public float Cos(float angle) => Trigonometric.Cos(angle);

    /// <summary>
    ///     Calcula la tangente de un ángulo desde la tabla precalculada
    /// </summary>
    public float Tan(float angle) => Trigonometric.Tan(angle);

    /// <summary>
    ///     Calcula el seno de un ángulo desde la tabla precalculada de la forma rápida (sin interpolación)
    /// </summary>
    public float SinFast(float angle) => Trigonometric.SinFast(angle);

    /// <summary>
    ///     Calcula el coseno de un ángulo desde la tabla precalculada de la forma rápida (sin interpolación)
    /// </summary>
    public float CosFast(float angle) => Trigonometric.CosFast(angle);

    /// <summary>
    ///     Calcula la tangente de un ángulo desde la tabla precalculada de la forma rápida (sin interpolación)
    /// </summary>
    public float TanFast(float angle) => Trigonometric.TanFast(angle);

    /// <summary>
    ///     Calcula la raiz cuadrada de un valor desde la tabla precalculada
    /// </summary>
    public float Sqrt(float value) => SquareRoot.Sqrt(value);

    /// <summary>
    ///     Calcula la raiz cuadrada de un valor desde la tabla precalculada de la forma rápida (sin interpolación)
    /// </summary>
    public float SqrtFast(float value) => SquareRoot.SqrtFast(value);

    /// <summary>
    ///     Obtiene un valor aleatorio a partir de la tabla de ruido precalculada
    /// </summary>
    public float Rnd(float value) => Noise.GetNoise(value);

    /// <summary>
    ///     Tabla precalculada para funciones trigonométricas: senos, cosenos, tangentes
    /// </summary>
    public TrigonometricPrecomputed Trigonometric
    {
        get 
        {
            if (_sineCosine is null)
            {
                _sineCosine = new TrigonometricPrecomputed(3_600);
                _sineCosine.Initialize();
            }
            return _sineCosine;
        }
    }

    /// <summary>
    ///     Tabla precalculada para raíces cuadradas
    /// </summary>
    public SquareRootPrecomputed SquareRoot
    {
        get 
        {
            // Genera la tabla si no estaba ya en memoria
            if (_squareRoot is null)
            {
                _squareRoot = new SquareRootPrecomputed(10_000);
                _squareRoot.Initialize();
            }
            // Devuelve la tabla generada
            return _squareRoot;
        }
    }

    /// <summary>
    ///     Tabla precalculada para valores aleatorios
    /// </summary>
    public NoisePrecomputed Noise
    {
        get 
        {
            // Genera la tabla si no estaba ya en memoria
            if (_noise is null)
            {
                _noise = new NoisePrecomputed(256);
                _noise.Initialize();
            }
            // Devuelve la tabla generada
            return _noise;
        }
    }
}

/*
public class FloatLUT
{
    private float[] _table;
    private int _resolution;
    private float _indexToValue;
    private float _maxValue;

    public FloatLUT(Func<float, float> function, float maxValue, int resolution = 1000)
    {
        _resolution = resolution;
        _maxValue = maxValue;
        _table = new float[resolution];
        _indexToValue = maxValue / (resolution - 1);

        for (int i = 0; i < resolution; i++)
        {
            float x = i * _indexToValue;
            _table[i] = function(x);
        }
    }

    public float Evaluate(float x)
    {
        x = MathHelper.Clamp(x, 0, _maxValue);
        float floatIndex = x / _indexToValue;
        int index0 = (int)floatIndex;
        int index1 = Math.Min(index0 + 1, _resolution - 1);

        float t = floatIndex - index0;
        return MathHelper.Lerp(_table[index0], _table[index1], t);
    }
}
*/