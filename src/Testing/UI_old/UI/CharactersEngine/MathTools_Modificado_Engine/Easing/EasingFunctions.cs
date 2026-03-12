namespace UI.CharactersEngine.MathTools.Easing;

/// <summary>
///     Funciones de easing
/// </summary>
public static class EasingFunctions
{
    // Lineal
    public static float Linear(float t) => t;
    
    // Cuadráticos
    public static float QuadIn(float t) => t * t;
    public static float QuadOut(float t) => 1 - (1 - t) * (1 - t);
    public static float QuadInOut(float t) => t < 0.5f ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2;
    
    // Cúbicos
    public static float CubicIn(float t) => t * t * t;
    public static float CubicOut(float t) => 1 - MathF.Pow(1 - t, 3);
    public static float CubicInOut(float t) => t < 0.5f ? 4 * t * t * t : 1 - MathF.Pow(-2 * t + 2, 3) / 2;
    
    // Cuárticos
    public static float QuartIn(float t) => t * t * t * t;
    public static float QuartOut(float t) => 1 - MathF.Pow(1 - t, 4);
    public static float QuartInOut(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - MathF.Pow(-2 * t + 2, 4) / 2;
    
    // Quinticos
    public static float QuintIn(float t) => t * t * t * t * t;
    public static float QuintOut(float t) => 1 - MathF.Pow(1 - t, 5);
    public static float QuintInOut(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 - MathF.Pow(-2 * t + 2, 5) / 2;

    // Seno - muy suave
    public static float SineIn(float t) => 1 - MathF.Cos((t * MathF.PI) / 2);
    public static float SineOut(float t) => MathF.Sin((t * MathF.PI) / 2);
    public static float SineInOut(float t) => -(MathF.Cos(MathF.PI * t) - 1) / 2;
    
    // Circular - aceleración brusca al final
    public static float CircIn(float t) => 1 - MathF.Sqrt(1 - MathF.Pow(t, 2));
    public static float CircOut(float t) => MathF.Sqrt(1 - MathF.Pow(t - 1, 2));
    public static float CircInOut(float t)
    {
        if (t < 0.5f)
            return (1 - MathF.Sqrt(1 - MathF.Pow(2 * t, 2))) / 2;
        else
            return (MathF.Sqrt(1 - MathF.Pow(-2 * t + 2, 2)) + 1) / 2;
    }

    // Exponencial - empieza casi parado
    public static float ExpoIn(float t) => t == 0 ? 0 : MathF.Pow(2, 10 * (t - 1));
    public static float ExpoOut(float t) => t == 1 ? 1 : 1 - MathF.Pow(2, -10 * t);
    public static float ExpoInOut(float t)
    {
        if (t == 0 || t == 1)
            return t;
        else if (t < 0.5f)
            return MathF.Pow(2, 20 * t - 10) / 2;
        else
            return (2 - MathF.Pow(2, -20 * t + 10)) / 2;
    }

    // Back - se pasa del target y vuelve (efecto goma)
    public static float BackIn(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }
    
    public static float BackOut(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + c3 * MathF.Pow(t - 1, 3) + c1 * MathF.Pow(t - 1, 2);
    }
    
    public static float BackInOut(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        
        return t < 0.5f
            ? (MathF.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
            : (MathF.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }

    // Elastic - como un muelle
    public static float ElasticIn(float t)
    {
        const float c4 = (2 * MathF.PI) / 3;
        
        return t == 0 ? 0 : t == 1 ? 1 : 
            -MathF.Pow(2, 10 * (t - 1)) * MathF.Sin((t - 1.1f) * 5 * c4);
    }
    
    public static float ElasticOut(float t)
    {
        const float c4 = (2 * MathF.PI) / 3;
        
        return t == 0 ? 0 : t == 1 ? 1 : 
            MathF.Pow(2, -10 * t) * MathF.Sin((t - 0.1f) * 5 * c4) + 1;
    }
    
    public static float ElasticInOut(float t)
    {
        const float c5 = (2 * MathF.PI) / 4.5f;
        
        return t == 0 ? 0 : t == 1 ? 1 :
            t < 0.5f
                ? -(MathF.Pow(2, 20 * t - 10) * MathF.Sin((20 * t - 11.125f) * c5)) / 2
                : (MathF.Pow(2, -20 * t + 10) * MathF.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
    }

    // Bounce - como una pelota rebotando
    public static float BounceOut(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;
        
        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    public static float BounceIn(float t) => 1 - BounceOut(1 - t);
    
    public static float BounceInOut(float t)
    {
        if (t < 0.5f)
            return (1 - BounceOut(1 - 2 * t)) / 2;
        else
            return (1 + BounceOut(2 * t - 1)) / 2;
    }

    // Smoothstep - muy suave, ideal para fades
    public static float SmoothStep(float t) => t * t * (3 - 2 * t);
    
    // SmootherStep - aún más suave
    public static float SmootherStep(float t) => t * t * t * (t * (t * 6 - 15) + 10);
    
    // Flash - para impactos (rápido al principio, lento al final)
    public static float Flash(float t) => MathF.Pow(t, 0.3f);
    
    // Anticipate - se prepara antes de lanzarse (como un golpe)
    public static float Anticipate(float t)
    {
        const float c = 1.70158f;
        return t * t * ((c + 1) * t - c);
    }
    
    // Overshoot configurable
    public static Func<float, float> Overshoot(float amount) => t => t * t * ((amount + 1) * t - amount);
    
    // Elasticidad configurable
    public static Func<float, float> Elastic(float amplitude, float period) => t =>
    {
        if (t == 0 || t == 1) return t;
        float s = period / (2 * MathF.PI) * MathF.Asin(1 / amplitude);
        return amplitude * MathF.Pow(2, -10 * t) * MathF.Sin((t - s) * (2 * MathF.PI) / period) + 1;
    };
    
    // Steps - para efectos de máquina de escribir o pixelado
    public static Func<float, float> Steps(int count) => t => MathF.Floor(t * count) / count;
    
    // Parabólica - sube y baja (para saltos)
    public static float Parabola(float t, float peak = 0.5f)
    {
        // t: 0->1, retorna 0->1->0 con pico en 'peak'
        if (t < peak)
            return t / peak;
        else
            return (1 - t) / (1 - peak);
    }
}

/*
public static class Ease
{
    public static IEasingDefinition Linear => new LambdaDefinition(Easing.Linear);
    public static IEasingDefinition QuadOut => new LambdaDefinition(Easing.QuadOut);
    public static IEasingDefinition BackOut => new LambdaDefinition(Easing.BackOut);
    public static IEasingDefinition ElasticOut => new LambdaDefinition(Easing.ElasticOut);
    public static IEasingDefinition BounceOut => new LambdaDefinition(Easing.BounceOut);
    public static IEasingDefinition SmoothStep => new LambdaDefinition(Easing.SmoothStep);
    
    // Con parámetros
    public static IEasingDefinition Overshoot(float amount) => 
        new LambdaDefinition(Easing.Overshoot(amount));
    
    public static IEasingDefinition Elastic(float amp, float period) => 
        new LambdaDefinition(Easing.Elastic(amp, period));
    
    public static IEasingDefinition Custom(Func<float, float> func) => 
        new LambdaDefinition(func);
}
*/