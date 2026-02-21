#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 Direction; // Dirección del blur
int Samples; // 15
float BlurAmount; // 0.1

float4 PS_DirectionalBlur(VertexShaderOutput input) : COLOR
{
    float4 from = float4(0,0,0,0);
    float4 to = tex2D(NextSampler, input.UV);
    
    float2 dir = normalize(Direction) * BlurAmount * Progress;
    
    for(int i = -Samples/2; i < Samples/2; i++)
    {
        float t = (float)i / (float)(Samples/2);
        float2 offset = dir * t;
        from += tex2D(PreviousSampler, input.UV + offset);
    }
    from /= (float)Samples;
    
    return lerp(from, to, Progress);
}

technique DirectionalBlur
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_DirectionalBlur();
    }
}
