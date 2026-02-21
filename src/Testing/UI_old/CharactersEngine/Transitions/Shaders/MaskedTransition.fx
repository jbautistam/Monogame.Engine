#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Usa una textura en escala de grises como máscara de transición
Texture2D MaskTexture;
float Progress;

float4 PS_Masked(VertexShaderOutput input) : COLOR
{
    float mask = tex2D(MaskSampler, input.UV).r;
    float edge = smoothstep(Progress - 0.1, Progress, mask);
    return lerp(from, to, edge);
}
