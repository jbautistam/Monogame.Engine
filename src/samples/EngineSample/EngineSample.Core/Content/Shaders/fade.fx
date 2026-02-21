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
float4 FadeColor; // Para fade to color antes de fade in

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

float4 PS_Fade(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Opcional: fade through color (flash to white/black)
    if (FadeColor.a > 0)
    {
        float mid = abs(Progress - 0.5) * 2; // 1 en medio, 0 en extremos
        float4 mixed = lerp(from, to, Progress);
        return lerp(mixed, FadeColor, (1 - mid) * FadeColor.a);
    }
    
    return lerp(from, to, Progress);
}

technique Fade
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Fade();
    }
}
