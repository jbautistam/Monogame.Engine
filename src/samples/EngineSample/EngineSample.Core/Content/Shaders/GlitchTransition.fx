#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float GlitchIntensity;
float Time;

float4 PS_Glitch(VertexShaderOutput input) : COLOR
{
    float glitch = step(Random(input.UV + Time), Progress);
    float2 offset = float2(Random(input.UV.y + Time) - 0.5, 0) * glitch * 0.1;
    
    float4 from = tex2D(PreviousSampler, input.UV + offset);
    float4 to = tex2D(NextSampler, input.UV);
    
    // RGB split aleatorio
    float r = tex2D(PreviousSampler, input.UV + offset + float2(0.01, 0)).r;
    float b = tex2D(PreviousSampler, input.UV + offset - float2(0.01, 0)).b;
    
    from.gb = float2(to.g, b);
    from.r = r;
    
    return lerp(from, to, Progress);
}
