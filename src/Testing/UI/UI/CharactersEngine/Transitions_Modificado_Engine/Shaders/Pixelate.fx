#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 ScreenSize;
float MaxPixelSize; // 64.0

float4 PS_Pixelate(VertexShaderOutput input) : COLOR
{
    // Curva: pequeño -> grande -> pequeño
    float phase = sin(Progress * 3.14159265); // 0 -> 1 -> 0
    float pixelSize = lerp(1.0, MaxPixelSize, phase);
    
    float2 pixelatedUV = floor(input.UV * ScreenSize / pixelSize) * pixelSize / ScreenSize;
    
    float4 from = tex2D(PreviousSampler, pixelatedUV);
    float4 to = tex2D(NextSampler, pixelatedUV);
    
    // Crossfade simultáneo al pixelado
    return lerp(from, to, Progress);
}

technique Pixelate
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Pixelate();
    }
}
