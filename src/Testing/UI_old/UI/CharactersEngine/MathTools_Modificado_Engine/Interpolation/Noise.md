User: Añade perlin noise
Kimi: Aquí está el Perlin Noise para movimientos orgánicos:

```csharp
public static class Noise
{
    // Perlin Noise 1D simplificado
    private static readonly int[] Permutation = new int[512];
    
    static Noise()
    {
        // Inicializar permutación (valores 0-255 duplicados)
        var p = Enumerable.Range(0, 256).OrderBy(x => Guid.NewGuid()).ToArray();
        for (int i = 0; i < 256; i++)
        {
            Permutation[i] = p[i];
            Permutation[256 + i] = p[i];
        }
    }
    
    private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
    
    private static float Lerp(float a, float b, float t) => a + t * (b - a);
    
    private static float Grad(int hash, float x)
    {
        // Gradientes simples: -1 o 1 según bit bajo del hash
        return (hash & 1) == 0 ? x : -x;
    }
    
    // Perlin Noise 1D
    public static float Perlin1D(float x)
    {
        int X = (int)MathF.Floor(x) & 255;
        x -= MathF.Floor(x);
        
        float u = Fade(x);
        
        int a = Permutation[X];
        int b = Permutation[X + 1];
        
        return Lerp(Grad(a, x), Grad(b, x - 1), u);
    }
    
    // Perlin Noise con octavas (más detalle)
    public static float Perlin1DOctaves(float x, int octaves = 4, float persistence = 0.5f)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Perlin1D(x * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        
        return total / maxValue; // Normalizar a -1..1 aprox
    }
    
    // Ruido de valor suavizado (más rápido, menos suave que Perlin)
    public static float ValueNoise(float x)
    {
        int i = (int)MathF.Floor(x);
        float f = x - i;
        
        float a = Hash(i);
        float b = Hash(i + 1);
        
        return Lerp(a, b, Fade(f));
    }
    
    private static float Hash(int n)
    {
        n = (n << 13) ^ n;
        return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
    }
}
```

## Easing con Perlin Noise

```csharp
public static class Easing
{
    // Shake orgánico con Perlin
    public static Func<float, float> PerlinShake(float intensity, float frequency) => t =>
    {
        // t: 0-1 progreso, retorna offset -1..1 * intensity
        float noise = Noise.Perlin1D(t * frequency);
        return noise * intensity * (1 - t); // Amortiguado al final
    };
    
    // Movimiento errático (para fantasmas, elementos perturbados)
    public static Func<float, float> Erratic(float baseEasing, float noiseAmount, float frequency) => t =>
    {
        float baseValue = t; // o aplicar easing base
        float noise = Noise.Perlin1DOctaves(t * frequency, 3, 0.5f) * noiseAmount;
        return MathHelper.Clamp(baseValue + noise, 0f, 1f);
    };
    
    // Hover/flotar (movimiento cíclico suave)
    public static Func<float, float> Hover(float amplitude, float frequency) => t =>
    {
        float baseProgress = t;
        float hover = Noise.Perlin1DOctaves(t * frequency, 2, 0.5f) * amplitude;
        return baseProgress + hover;
    };
    
    // Wind/viento (para pelo, ropa, hojas)
    public static Func<float, float> Wind(float strength, float gustFrequency) => t =>
    {
        float baseWind = Noise.Perlin1D(t * gustFrequency);
        float turbulence = Noise.Perlin1DOctaves(t * gustFrequency * 3, 4, 0.4f) * 0.3f;
        return (baseWind + turbulence) * strength;
    };
    
    // Typewriter humanizado (con variación en velocidad)
    public static Func<float, float> HumanType(float irregularity) => t =>
    {
        float baseStep = MathF.Floor(t * 20) / 20; // 20 pasos
        float variation = Noise.Perlin1D(t * 50) * irregularity;
        return MathHelper.Clamp(baseStep + variation, 0f, 1f);
    };
}
```

## Interpolador especial de ruido

```csharp
public class NoiseInterpolator : IInterpolator<Vector2>
{
    private readonly float _amplitudeX;
    private readonly float _amplitudeY;
    private readonly float _frequency;
    private readonly IEasingDefinition _baseEasing;
    
    public NoiseInterpolator(
        float amplitudeX, 
        float amplitudeY, 
        float frequency,
        IEasingDefinition baseEasing = null)
    {
        _amplitudeX = amplitudeX;
        _amplitudeY = amplitudeY;
        _frequency = frequency;
        _baseEasing = baseEasing ?? new LambdaDefinition(Easing.Linear);
    }
    
    public Vector2 Interpolate(Vector2 from, Vector2 to, float t)
    {
        float easedT = _baseEasing.Evaluate(t);
        Vector2 basePos = Vector2.Lerp(from, to, easedT);
        
        // Añadir ruido perpendicular a la dirección del movimiento
        float noiseX = Noise.Perlin1DOctaves(t * _frequency, 3, 0.5f) * _amplitudeX * (1 - t);
        float noiseY = Noise.Perlin1DOctaves(t * _frequency + 100, 3, 0.5f) * _amplitudeY * (1 - t);
        
        return basePos + new Vector2(noiseX, noiseY);
    }
}
```

## Uso en secuencias

```csharp
// Definiciones de ruido
public static class Ease
{
    public static IEasingDefinition Shake(float intensity, float freq) => 
        new LambdaDefinition(Easing.PerlinShake(intensity, freq));
    
    public static IInterpolator<Vector2> NoiseMove(float ampX, float ampY, float freq) => 
        new NoiseInterpolator(ampX, ampY, freq);
}

// Ejemplos
manager.CreateSequence()
    // Movimiento con temblor orgánico (personaje asustado)
    .Move("hero", new Vector2(500, 900), 2f, 
        Ease.NoiseMove(10f, 5f, 3f))
    
    // Hover de objeto flotante
    .Move("ghost", new Vector2(600, 400), 3f, 
        new LambdaDefinition(Easing.Hover(20f, 2f)))
    
    // Texto con velocidad irregular
    .Custom("text", 5f, (actor, t) => {
        float humanT = Easing.HumanType(0.1f)(t);
        // Aplicar a reveal de texto...
    })
    
    // Viento afectando posición
    .Custom("leaf", 4f, (actor, t) => {
        float wind = Easing.Wind(30f, 1.5f)(t);
        actor.Transform.Position.X += wind;
        actor.Transform.Rotation += wind * 0.1f;
    })
    
    // Shake clásico vs Shake orgánico
    .Move("robot", pos, 0.5f, Ease.Shake(5f, 20f))      // Shake mecánico
    .Move("leaf", pos, 2f, Ease.NoiseMove(15f, 8f, 4f))  // Viento natural
    
    .Play();
```

## Función adicional: Turbulencia

```csharp
public static float Turbulence(float x, float y, float z = 0)
{
    // Sumar valor absoluto de octavas para efecto "nubes"
    float total = 0;
    float frequency = 1;
    float amplitude = 1;
    
    for (int i = 0; i < 4; i++)
    {
        total += MathF.Abs(Noise.Perlin1D(x * frequency + i * 100)) * amplitude;
        amplitude *= 0.5f;
        frequency *= 2;
    }
    
    return total;
}
```

¿Quieres que añada también Simplex Noise (más rápido y sin artefactos en 3D) o Worley Noise (para patrones celulares/orgánicos)?
User: Añade simples noise
Kimi: Aquí está el Simplex Noise, más rápido y sin artefactos direccionales:

```csharp
public static class SimplexNoise
{
    // Permutación y gradientes
    private static readonly int[] Perm = new int[512];
    private static readonly int[] PermMod12 = new int[512];
    
    // Gradientes 3D (12 direcciones principales)
    private static readonly int[][] Grad3 = new int[][]
    {
        new[] {1, 1, 0}, new[] {-1, 1, 0}, new[] {1, -1, 0}, new[] {-1, -1, 0},
        new[] {1, 0, 1}, new[] {-1, 0, 1}, new[] {1, 0, -1}, new[] {-1, 0, -1},
        new[] {0, 1, 1}, new[] {0, -1, 1}, new[] {0, 1, -1}, new[] {0, -1, -1}
    };
    
    static SimplexNoise()
    {
        var p = Enumerable.Range(0, 256).OrderBy(x => Guid.NewGuid()).ToArray();
        for (int i = 0; i < 512; i++)
        {
            Perm[i] = p[i & 255];
            PermMod12[i] = Perm[i] % 12;
        }
    }
    
    // Skewing y unskewing factors
    private const float F3 = 1.0f / 3.0f;
    private const float G3 = 1.0f / 6.0f;
    
    // Simplex Noise 3D (el más completo, se usa para 1D y 2D también)
    public static float Noise(float x, float y, float z)
    {
        // Skew input space
        float s = (x + y + z) * F3;
        int i = FastFloor(x + s);
        int j = FastFloor(y + s);
        int k = FastFloor(z + s);
        
        float t = (i + j + k) * G3;
        float X0 = i - t;
        float Y0 = j - t;
        float Z0 = k - t;
        float x0 = x - X0;
        float y0 = y - Y0;
        float z0 = z - Z0;
        
        // Determinar simplex
        int i1, j1, k1;
        int i2, j2, k2;
        
        if (x0 >= y0)
        {
            if (y0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; }
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; }
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; }
        }
        else
        {
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; }
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; }
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; }
        }
        
        float x1 = x0 - i1 + G3;
        float y1 = y0 - j1 + G3;
        float z1 = z0 - k1 + G3;
        float x2 = x0 - i2 + 2.0f * G3;
        float y2 = y0 - j2 + 2.0f * G3;
        float z2 = z0 - k2 + 2.0f * G3;
        float x3 = x0 - 1.0f + 3.0f * G3;
        float y3 = y0 - 1.0f + 3.0f * G3;
        float z3 = z0 - 1.0f + 3.0f * G3;
        
        // Hash coordinates
        int ii = i & 255;
        int jj = j & 255;
        int kk = k & 255;
        
        // Calculate contributions
        float n0 = 0, n1 = 0, n2 = 0, n3 = 0;
        
        float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 >= 0)
        {
            t0 *= t0;
            int gi0 = PermMod12[ii + Perm[jj + Perm[kk]]];
            n0 = t0 * t0 * Dot(Grad3[gi0], x0, y0, z0);
        }
        
        float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 >= 0)
        {
            t1 *= t1;
            int gi1 = PermMod12[ii + i1 + Perm[jj + j1 + Perm[kk + k1]]];
            n1 = t1 * t1 * Dot(Grad3[gi1], x1, y1, z1);
        }
        
        float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 >= 0)
        {
            t2 *= t2;
            int gi2 = PermMod12[ii + i2 + Perm[jj + j2 + Perm[kk + k2]]];
            n2 = t2 * t2 * Dot(Grad3[gi2], x2, y2, z2);
        }
        
        float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 >= 0)
        {
            t3 *= t3;
            int gi3 = PermMod12[ii + 1 + Perm[jj + 1 + Perm[kk + 1]]];
            n3 = t3 * t3 * Dot(Grad3[gi3], x3, y3, z3);
        }
        
        // Sumar y normalizar a -1..1
        return 32.0f * (n0 + n1 + n2 + n3);
    }
    
    // Sobrecargas 1D y 2D
    public static float Noise(float x)
    {
        return Noise(x, 0, 0);
    }
    
    public static float Noise(float x, float y)
    {
        return Noise(x, y, 0);
    }
    
    // Octavas
    public static float NoiseOctaves(float x, int octaves = 4, float persistence = 0.5f)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Noise(x * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        
        return total / maxValue;
    }
    
    public static float NoiseOctaves(float x, float y, int octaves = 4, float persistence = 0.5f)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Noise(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        
        return total / maxValue;
    }
    
    // Ruido de flujo (Flow Noise) - para movimientos fluidos
    public static float FlowNoise(float x, float y, float time, float rotation = 1f)
    {
        float nx = Noise(x + time * rotation, y);
        float ny = Noise(x - time * rotation, y + 100);
        return Noise(x + nx, y + ny);
    }
    
    // Curl noise (para campos de velocidad 2D)
    public static Vector2 CurlNoise(float x, float y, float epsilon = 0.01f)
    {
        float n1 = Noise(x, y + epsilon);
        float n2 = Noise(x, y - epsilon);
        float n3 = Noise(x + epsilon, y);
        float n4 = Noise(x - epsilon, y);
        
        float dx = (n1 - n2) / (2 * epsilon);
        float dy = (n4 - n3) / (2 * epsilon);
        
        return new Vector2(dy, -dx); // Perpendicular al gradiente
    }
    
    // Helpers
    private static int FastFloor(float x) => x > 0 ? (int)x : (int)x - 1;
    
    private static float Dot(int[] g, float x, float y, float z) => 
        g[0] * x + g[1] * y + g[2] * z;
}
```

## Easing con Simplex Noise

```csharp
public static class Easing
{
    // Shake orgánico superior (sin patrones visibles)
    public static Func<float, float> SimplexShake(float intensity, float frequency) => t =>
    {
        float noise = SimplexNoise.NoiseOctaves(t * frequency, 3, 0.5f);
        return noise * intensity * (1 - t);
    };
    
    // Movimiento fluido tipo "agua" o "humo"
    public static Func<float, float> Fluid(float baseSpeed, float turbulence) => t =>
    {
        float flow = SimplexNoise.FlowNoise(t * baseSpeed, 0, t * 2, 0.5f);
        return t + flow * turbulence;
    };
    
    // Trayectoria errática natural (para insectos, partículas)
    public static Func<float, float> ErraticSimplex(float baseEasing, float chaos) => t =>
    {
        float baseVal = t; // o aplicar easing base
        float disorder = SimplexNoise.NoiseOctaves(t * 8, 4, 0.6f) * chaos;
        return MathHelper.Clamp(baseVal + disorder, 0f, 1f);
    };
    
    // Viento más realista (con turbulencia)
    public static Func<float, float> WindSimplex(float strength, float scale) => t =>
    {
        float main = SimplexNoise.Noise(t * scale);
        float detail = SimplexNoise.NoiseOctaves(t * scale * 3, 3, 0.4f) * 0.3f;
        float gust = SimplexNoise.Noise(t * scale * 0.3f + 100) * 0.5f;
        return (main + detail + gust) * strength;
    };
    
    // Temblor de cámara orgánico
    public static Func<float, Vector2> CameraShake(float intensity, float frequency) => t =>
    {
        float x = SimplexNoise.Noise(t * frequency);
        float y = SimplexNoise.Noise(t * frequency + 100);
        float damp = 1 - t;
        return new Vector2(x, y) * intensity * damp;
    };
}
```

## Interpolador Simplex mejorado

```csharp
public class SimplexInterpolator : IInterpolator<Vector2>
{
    private readonly float _amplitude;
    private readonly float _frequency;
    private readonly float _timeScale;
    private readonly IEasingDefinition _baseEasing;
    
    public SimplexInterpolator(
        float amplitude, 
        float frequency,
        float timeScale = 1f,
        IEasingDefinition baseEasing = null)
    {
        _amplitude = amplitude;
        _frequency = frequency;
        _timeScale = timeScale;
        _baseEasing = baseEasing ?? new LambdaDefinition(Easing.Linear);
    }
    
    public Vector2 Interpolate(Vector2 from, Vector2 to, float t)
    {
        float easedT = _baseEasing.Evaluate(t);
        Vector2 basePos = Vector2.Lerp(from, to, easedT);
        
        // Simplex 2D para movimiento orgánico
        float noiseX = SimplexNoise.NoiseOctaves(t * _frequency, easedT * _timeScale, 3, 0.5f);
        float noiseY = SimplexNoise.NoiseOctaves(t * _frequency + 100, easedT * _timeScale, 3, 0.5f);
        
        float damp = 1 - t; // Amortiguación al final
        return basePos + new Vector2(noiseX, noiseY) * _amplitude * damp;
    }
}
```

## Comparación y uso

```csharp
// Perlin vs Simplex
manager.CreateSequence()
    // Perlin: ligeros patrones diagonales (a veces visibles)
    .Move("old", pos, 3f, new NoiseInterpolator(20f, 2f, 0.5f))
    
    // Simplex: completamente orgánico, sin patrones
    .Move("new", pos, 3f, new SimplexInterpolator(20f, 2f, 0.5f))
    
    // Fluido tipo agua
    .Custom("water", 5f, (actor, t) => {
        float flow = Easing.Fluid(2f, 0.3f)(t);
        actor.Transform.Position.Y += MathF.Sin(flow * MathF.PI) * 10;
    })
    
    // Viento con turbulencia realista
    .Custom("leaf", 4f, (actor, t) => {
        float wind = Easing.WindSimplex(40f, 1.5f)(t);
        Vector2 curl = SimplexNoise.CurlNoise(t * 3, actor.Transform.Position.Y * 0.01f);
        actor.Transform.Position += new Vector2(wind, 0) + curl * 5;
    })
    
    // Curl noise para remolinos
    .Custom("particle", 6f, (actor, t) => {
        Vector2 curl = SimplexNoise.CurlNoise(
            actor.Transform.Position.X * 0.01f, 
            actor.Transform.Position.Y * 0.01f + t
        );
        actor.Transform.Position += curl * 2;
    })
    
    .Play();
```

## Ventajas de Simplex sobre Perlin

| Aspecto | Perlin | Simplex |
|---------|--------|---------|
| Artefactos | Patrones direccionales visibles | Ninguno |
| Velocidad | O(2^n) | O(n) - más rápido en alta dimensión |
| Continuidad | Derivada discontinua en grid | Siempre continua |
| Memoria | Tabla de 256 | Tabla de 512 (pero más eficiente) |

¿Quieres que añada también **Worley Noise** (para patrones celulares/orgánicos tipo piedra, piel, nubes volumétricas) o **Value Noise** (más rápido aún, para efectos retro/pixelados)?
User: Añade value noise
Kimi: Aquí está el Value Noise, más rápido y simple, ideal para efectos retro y pixelados:

```csharp
public static class ValueNoise
{
    // Tabla de permutación
    private static readonly int[] Perm = new int[512];
    
    static ValueNoise()
    {
        var p = Enumerable.Range(0, 256).OrderBy(x => Guid.NewGuid()).ToArray();
        for (int i = 0; i < 512; i++)
            Perm[i] = p[i & 255];
    }
    
    // Interpolación lineal
    private static float Lerp(float a, float b, float t) => a + t * (b - a);
    
    // Curva de suavizado (opcional: lineal = retro, suave = normal)
    private static float Smooth(float t) => t * t * (3 - 2 * t);
    private static float Linear(float t) => t;
    
    // Value Noise 1D
    public static float Noise(float x, bool smooth = true)
    {
        int i = FastFloor(x);
        float f = x - i;
        
        int h0 = Perm[i & 255];
        int h1 = Perm[(i + 1) & 255];
        
        // Valores pseudoaleatorios de los hashes
        float v0 = (h0 / 255f) * 2 - 1; // -1 a 1
        float v1 = (h1 / 255f) * 2 - 1;
        
        float t = smooth ? Smooth(f) : Linear(f);
        return Lerp(v0, v1, t);
    }
    
    // Value Noise 2D
    public static float Noise(float x, float y, bool smooth = true)
    {
        int ix = FastFloor(x);
        int iy = FastFloor(y);
        float fx = x - ix;
        float fy = y - iy;
        
        int x0 = ix & 255;
        int x1 = (ix + 1) & 255;
        int y0 = iy & 255;
        int y1 = (iy + 1) & 255;
        
        // Hash de esquinas
        float v00 = Hash(Perm[x0 + Perm[y0]]);
        float v10 = Hash(Perm[x1 + Perm[y0]]);
        float v01 = Hash(Perm[x0 + Perm[y1]]);
        float v11 = Hash(Perm[x1 + Perm[y1]]);
        
        float tx = smooth ? Smooth(fx) : Linear(fx);
        float ty = smooth ? Smooth(fy) : Linear(fy);
        
        // Interpolación bilineal
        float xLerp0 = Lerp(v00, v10, tx);
        float xLerp1 = Lerp(v01, v11, tx);
        
        return Lerp(xLerp0, xLerp1, ty);
    }
    
    // Value Noise 3D
    public static float Noise(float x, float y, float z, bool smooth = true)
    {
        int ix = FastFloor(x);
        int iy = FastFloor(y);
        int iz = FastFloor(z);
        float fx = x - ix;
        float fy = y - iy;
        float fz = z - iz;
        
        int x0 = ix & 255, x1 = (ix + 1) & 255;
        int y0 = iy & 255, y1 = (iy + 1) & 255;
        int z0 = iz & 255, z1 = (iz + 1) & 255;
        
        // 8 esquinas del cubo
        float v000 = Hash(Perm[x0 + Perm[y0 + Perm[z0]]]);
        float v100 = Hash(Perm[x1 + Perm[y0 + Perm[z0]]]);
        float v010 = Hash(Perm[x0 + Perm[y1 + Perm[z0]]]);
        float v110 = Hash(Perm[x1 + Perm[y1 + Perm[z0]]]);
        float v001 = Hash(Perm[x0 + Perm[y0 + Perm[z1]]]);
        float v101 = Hash(Perm[x1 + Perm[y0 + Perm[z1]]]);
        float v011 = Hash(Perm[x0 + Perm[y1 + Perm[z1]]]);
        float v111 = Hash(Perm[x1 + Perm[y1 + Perm[z1]]]);
        
        float tx = smooth ? Smooth(fx) : Linear(fx);
        float ty = smooth ? Smooth(fy) : Linear(fy);
        float tz = smooth ? Smooth(fz) : Linear(fz);
        
        // Interpolación trilineal
        float x00 = Lerp(v000, v100, tx);
        float x10 = Lerp(v010, v110, tx);
        float x01 = Lerp(v001, v101, tx);
        float x11 = Lerp(v011, v111, tx);
        
        float y0 = Lerp(x00, x10, ty);
        float y1 = Lerp(x01, x11, ty);
        
        return Lerp(y0, y1, tz);
    }
    
    // Octavas
    public static float NoiseOctaves(float x, int octaves = 4, float persistence = 0.5f, bool smooth = true)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Noise(x * frequency, smooth) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        
        return total / maxValue;
    }
    
    public static float NoiseOctaves(float x, float y, int octaves = 4, float persistence = 0.5f, bool smooth = true)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Noise(x * frequency, y * frequency, smooth) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        
        return total / maxValue;
    }
    
    // Tileable (seamless) para texturas que se repiten
    public static float NoiseTileable(float x, float y, int period, bool smooth = true)
    {
        // Sample 4 veces para crear periodicidad
        float s0 = Noise(x, y, smooth);
        float s1 = Noise(x + period, y, smooth);
        float s2 = Noise(x, y + period, smooth);
        float s3 = Noise(x + period, y + period, smooth);
        
        // Blend suave en los bordes
        float fx = (x % period) / period;
        float fy = (y % period) / period;
        
        float tx = smooth ? Smooth(fx) : Linear(fx);
        float ty = smooth ? Smooth(fy) : Linear(fy);
        
        float xLerp0 = Lerp(s0, s1, tx);
        float xLerp1 = Lerp(s2, s3, tx);
        
        return Lerp(xLerp0, xLerp1, ty);
    }
    
    // Discretizado (efecto pixelado/retro)
    public static float NoisePixelated(float x, float y, int pixelSize, bool smooth = true)
    {
        // Redondear a grid de píxeles
        float px = MathF.Floor(x / pixelSize) * pixelSize;
        float py = MathF.Floor(y / pixelSize) * pixelSize;
        return Noise(px, py, smooth);
    }
    
    // Helpers
    private static int FastFloor(float x) => x > 0 ? (int)x : (int)x - 1;
    
    private static float Hash(int n)
    {
        // Hash simple pero efectivo
        n = (n << 13) ^ n;
        n = n * (n * n * 15731 + 789221) + 1376312589;
        return ((n >> 7) & 0xFFFFFF) / 8388607.5f - 1f; // -1 a 1
    }
}
```

## Easing con Value Noise

```csharp
public static class Easing
{
    // Shake retro/pixelado (8-bit style)
    public static Func<float, float> RetroShake(float intensity, int pixelSize = 4) => t =>
    {
        float discreteT = MathF.Floor(t * 20) / 20f; // 20fps efecto
        float noise = ValueNoise.Noise(discreteT * 10, false); // No suavizado
        return noise * intensity * (1 - t);
    };
    
    // Movimiento con snap (se siente mecánico/digital)
    public static Func<float, float> Snap(float steps, float jitter = 0.1f) => t =>
    {
        float baseStep = MathF.Floor(t * steps) / steps;
        float noise = ValueNoise.Noise(t * 50, false) * jitter;
        return MathHelper.Clamp(baseStep + noise, 0f, 1f);
    };
    
    // Glitch digital (para efectos de corrupción)
    public static Func<float, float> Glitch(float intensity, float threshold = 0.7f) => t =>
    {
        float noise = ValueNoise.NoiseOctaves(t * 20, 2, 0.5f, false);
        if (noise > threshold)
            return t + (noise - threshold) * intensity * 10;
        return t;
    };
    
    // Typewriter mecánico (con variación de velocidad realista)
    public static Func<float, float> MechanicalType(float baseSpeed, float variance) => t =>
    {
        float chars = t * baseSpeed;
        float noise = ValueNoise.Noise(MathF.Floor(chars), false);
        float delay = noise > 0.3f ? (noise - 0.3f) * variance : 0;
        return (chars + delay) / baseSpeed;
    };
    
    // Scanlines (para efectos CRT)
    public static Func<float, float> Scanline(float intensity, int lines) => t =>
    {
        float scan = MathF.Sin(t * lines * MathF.PI);
        float noise = ValueNoise.Noise(t * 100, false) * 0.3f;
        return t + (scan + noise) * intensity * 0.01f;
    };
    
    // Dithering (para fades con patrón)
    public static Func<float, float> DitherFade(float threshold = 0.5f) => t =>
    {
        float noise = ValueNoise.Noise(MathF.Floor(t * 16), MathF.Floor(t * 16 * 1.618f), false);
        noise = noise * 0.5f + 0.5f; // 0 a 1
        return t > threshold + noise * 0.2f ? 1f : t;
    };
}
```

## Comparación de ruidos

```csharp
// Uso comparativo
manager.CreateSequence()
    // Value Noise - rápido, retro, pixel-friendly
    .Move("retro", pos, 2f, new ValueInterpolator(15f, 4f, false))
    
    // Perlin - suave pero con patrones
    .Move("classic", pos, 2f, new NoiseInterpolator(15f, 4f))
    
    // Simplex - orgánico, sin patrones
    .Move("organic", pos, 2f, new SimplexInterpolator(15f, 4f))
    
    // Efectos específicos Value Noise
    .Custom("glitch", 1f, (actor, t) => {
        if (ValueNoise.Noise(t * 30, false) > 0.8f)
            actor.Transform.Position.X += 50; // Salto brusco
    })
    
    .Custom("crt", 3f, (actor, t) => {
        // Efecto scanline con jitter
        float scan = MathF.Sin(t * 60) * 2;
        float jitter = ValueNoise.Noise(MathF.Floor(t * 30), false) * 3;
        actor.Transform.Position.Y += scan + jitter;
    })
    
    // Movimiento snap (mecha/robot)
    .Move("robot", pos, 1f, Ease.Snap(8, 0.05f))
    
    // Fade con dithering
    .Fade("pixel", 0f, 1f, Ease.DitherFade(0.5f))
    
    .Play();
```

## Interpolador Value específico

```csharp
public class ValueInterpolator : IInterpolator<Vector2>
{
    private readonly float _amplitude;
    private readonly float _frequency;
    private readonly bool _smooth;
    private readonly int? _pixelSize; // Para efecto retro
    
    public ValueInterpolator(float amplitude, float frequency, bool smooth = true, int? pixelSize = null)
    {
        _amplitude = amplitude;
        _frequency = frequency;
        _smooth = smooth;
        _pixelSize = pixelSize;
    }
    
    public Vector2 Interpolate(Vector2 from, Vector2 to, float t)
    {
        Vector2 basePos = Vector2.Lerp(from, to, t);
        
        float nx, ny;
        
        if (_pixelSize.HasValue)
        {
            // Modo pixelado
            float px = MathF.Floor(t * _frequency / _pixelSize.Value) * _pixelSize.Value;
            nx = ValueNoise.Noise(px, 0, _smooth);
            ny = ValueNoise.Noise(px + 100, 0, _smooth);
        }
        else
        {
            // Modo continuo
            nx = ValueNoise.NoiseOctaves(t * _frequency, 3, 0.5f, _smooth);
            ny = ValueNoise.NoiseOctaves(t * _frequency + 100, 3, 0.5f, _smooth);
        }
        
        float damp = 1 - t;
        return basePos + new Vector2(nx, ny) * _amplitude * damp;
    }
}
```

## Rendimiento comparativo

| Ruido | Velocidad | Calidad | Uso ideal |
|-------|-----------|---------|-----------|
| Value | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | Retro, pixel art, glitch |
| Perlin | ⭐⭐⭐ | ⭐⭐⭐⭐ | Clásico, compatible viejo |
| Simplex | ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | Orgánico, profesional, 3D |

¿Quieres que añada **Worley Noise** (celular/Voronoi, para texturas orgánicas tipo piedra, piel, burbujas) o **Fractal Brownian Motion (FBM)** como wrapper universal para cualquier tipo de ruido?
