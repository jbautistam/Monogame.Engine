// Barras con Apertura Alterna
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D PreviousTexture;
Texture2D NextTexture;
sampler2D PreviousSampler = sampler_state { Texture = <PreviousTexture>; };
sampler2D NextSampler = sampler_state { Texture = <NextTexture>; };

float Progress;
int BarCount;           // Número de barras (8, 16, 32...)
float2 Direction;       // (1,0)=Horizontal, (0,1)=Vertical, (1,1)=Diagonal
float Smoothness;       // Suavizado de los bordes (0.0 a 0.5)
bool AlternateDirection; // true = barras alternas abren en dirección opuesta

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

// Reemplazar el cálculo de mask por:


float4 PS_Bars(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Determinar en qué barra estamos según dirección
    float projected;
    if (abs(Direction.x) > 0.5 && abs(Direction.y) > 0.5)
    {
        // Diagonal: proyectar en diagonal
        projected = (input.UV.x + input.UV.y) * 0.5;
    }
    else if (abs(Direction.x) > abs(Direction.y))
    {
        // Horizontal: usar coordenada Y
        projected = input.UV.y;
    }
    else
    {
        // Vertical: usar coordenada X  
        projected = input.UV.x;
    }
    
    // Calcular índice de barra y posición dentro de la barra (0-1)
    float barIndex = floor(projected * BarCount);
    float barPos = frac(projected * BarCount); // 0.0 = borde, 0.5 = centro
    
    // Alternar dirección de apertura según barra par/impar (opcional)
    // float alternate = fmod(barIndex, 2.0) == 0.0 ? 1.0 : -1.0;
    
    // Calcular máscara: en qué punto de la transición estamos
    // Progress 0.0 = todo "from", 1.0 = todo "to"
    
    // La barra se abre desde el centro hacia fuera
    float centerDist = abs(barPos - 0.5) * 2.0; // 0=en centro, 1=en borde
    
    // Invertir: 0=en borde (from), 1=en centro (to)
    float mask;
    if (AlternateDirection && fmod(barIndex, 2.0) == 1.0)
    {
        // Barra impar: abre en dirección contraria (o con offset)
        mask = 1.0 - abs((1.0 - barPos) - 0.5) * 2.0;
    }
    else
    {
        // Barra par: comportamiento normal
        mask = 1.0 - centerDist;
    }

    // Ajustar por progreso: cuánto de "to" se ve
    // Cuando Progress=0, solo se ve from (mask < 0)
    // Cuando Progress=1, se ve to en toda la barra (mask > 1)
    float threshold = Progress * (1.0 + Smoothness * 2.0) - Smoothness;
    
    // Suavizar el corte
    float blend = smoothstep(threshold - Smoothness, threshold + Smoothness, mask);
    
    return lerp(from, to, blend);
}

technique Bars
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Bars();
    }
}
