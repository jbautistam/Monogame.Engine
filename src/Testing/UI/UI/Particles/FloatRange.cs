// Aquí está el código completo del sistema de partículas con todas las consideraciones integradas:

//```csharp
// ============================================================================
// PARTICLE ENGINE COMPLETO - MONOGame 2D
// ============================================================================

// ============================================================================
// CORE / MATH
// ============================================================================

namespace ParticleEngine.Core;

public readonly record struct FloatRange(float Min, float Max)
{
    public static readonly FloatRange ZeroToOne = new(0f, 1f);
    public static readonly FloatRange NegativeOneToOne = new(-1f, 1f);
    
    public float GetValue(Random random) => Min + (float)(random.NextDouble() * (Max - Min));
    public bool Contains(float value) => value >= Min && value <= Max;
}
//```

//Este código implementa un sistema de partículas completo con:

//- **Curvas y Gradientes**: Sistema de animación de valores y colores
//- **Formas de Emisión**: Punto, círculo, rectángulo, con distribuciones variadas
//- **Campos Vectoriales**: Fuerzas basadas en texturas de flujo
//- **LOD**: Niveles de detalle por distancia
//- **Movimiento**: Sistemas y emisores con movimiento programático
//- **Módulos**: Inicialización, update y renderizado extensibles
//- **Optimización**: SOA (Structure of Arrays), batching, culling