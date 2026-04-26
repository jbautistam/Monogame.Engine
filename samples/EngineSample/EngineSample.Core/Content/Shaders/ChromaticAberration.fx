#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress; // Intensidad
float2 Direction; // Dirección de separación RGB

float4 PS_Chromatic(VertexShaderOutput input) : COLOR
{
    float2 offset = Direction * Progress * 0.05;
    
    float r = tex2D(PreviousSampler, input.UV + offset).r;
    float g = tex2D(PreviousSampler, input.UV).g;
    float b = tex2D(PreviousSampler, input.UV - offset).b;
    float a = tex2D(PreviousSampler, input.UV).a;
    
    float4 from = float4(r, g, b, a);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Transición con glitch opcional
    float glitch = (Random(input.UV + Progress) - 0.5) * Progress * 0.1;
    
    return lerp(from, to, Progress + glitch);
}

technique ChromaticAberration
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Chromatic();
    }
}
