namespace Bau.Libraries.BauGame.Engine.Tools.Tween;

/// <summary>
///     Funciones de easing para interpolación
/// </summary>
public static class EaseFunctions
{
    #region Linear
    public static float Linear(float t) => t;
    #endregion

    #region Quadratic
    public static float QuadIn(float t) => t * t;
    public static float QuadOut(float t) => 1 - (1 - t) * (1 - t);
    public static float QuadInOut(float t) => t < 0.5f ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;
    #endregion

    #region Cubic
    public static float CubicIn(float t) => t * t * t;
    public static float CubicOut(float t) => 1 - (float)Math.Pow(1 - t, 3);
    public static float CubicInOut(float t) => t < 0.5f ? 4 * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 3) / 2;
    #endregion

    #region Quartic
    public static float QuartIn(float t) => t * t * t * t;
    public static float QuartOut(float t) => 1 - (float)Math.Pow(1 - t, 4);
    public static float QuartInOut(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 4) / 2;
    #endregion

    #region Quintic
    public static float QuintIn(float t) => t * t * t * t * t;
    public static float QuintOut(float t) => 1 - (float)Math.Pow(1 - t, 5);
    public static float QuintInOut(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 5) / 2;
    #endregion

    #region Sine
    public static float SineIn(float t) => 1 - (float)Math.Cos(t * Math.PI / 2);
    public static float SineOut(float t) => (float)Math.Sin(t * Math.PI / 2);
    public static float SineInOut(float t) => -(float)(Math.Cos(Math.PI * t) - 1) / 2;
    #endregion

    #region Exponential
    public static float ExpoIn(float t) => t == 0 ? 0 : (float)Math.Pow(2, 10 * t - 10);
    public static float ExpoOut(float t) => t == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * t);
    public static float ExpoInOut(float t) => t == 0 
        ? 0 
        : t == 1 
        ? 1 
        : t < 0.5 
        ? (float)Math.Pow(2, 20 * t - 10) / 2 
        : (2 - (float)Math.Pow(2, -20 * t + 10)) / 2;
    #endregion

    #region Circular
    public static float CircIn(float t) => 1 - (float)Math.Sqrt(1 - (t * t));
    public static float CircOut(float t) => (float)Math.Sqrt(1 - (t - 1) * (t - 1));
    public static float CircInOut(float t) => t < 0.5
        ? (1 - (float)Math.Sqrt(1 - (2 * t) * (2 * t))) / 2
        : ((float)Math.Sqrt(1 - (2 * t - 2) * (2 * t - 2)) + 1) / 2;
    #endregion

    #region Back
    public static float BackIn(float t) => 2.70158f * t * t * t - 1.70158f * t * t;
    public static float BackOut(float t) => 1 + 2.70158f * (float)Math.Pow(t - 1, 3) + 1.70158f * (float)Math.Pow(t - 1, 2);
    public static float BackInOut(float t) => t < 0.5
        ? (float)(Math.Pow(2 * t, 2) * (7.189819f * t - 2.5949095f)) / 2
        : (float)(Math.Pow(2 * t - 2, 2) * (7.189819f * (t - 1) + 2.5949095f) + 2) / 2;
    #endregion

    #region Bounce
    public static float BounceOut(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2 / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        else if (t < 2.5 / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    public static float BounceIn(float t) => 1 - BounceOut(1 - t);
        
    public static float BounceInOut(float t) => t < 0.5
        ? (1 - BounceOut(1 - 2 * t)) / 2
        : (1 + BounceOut(2 * t - 1)) / 2;
    #endregion

    #region Elastic
    public static float ElasticIn(float t)
    {
        const float c4 = (2 * (float)Math.PI) / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : -(float)(Math.Pow(2, 10 * t - 10) * Math.Sin((t * 10 - 10.75) * c4));
    }

    public static float ElasticOut(float t)
    {
        const float c4 = (2 * (float)Math.PI) / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : (float)(Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1);
    }

    public static float ElasticInOut(float t)
    {
        const float c5 = (2 * (float)Math.PI) / 4.5f;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : t < 0.5
            ? -(float)(Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * c5)) / 2
            : (float)(Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * c5)) / 2 + 1;
    }
    #endregion
}
