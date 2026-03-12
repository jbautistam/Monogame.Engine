#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 Center;
int Samples; // 20
float Strength; // 2.0

float4 PS_ZoomBlur(VertexShaderOutput input) : COLOR
{
    float2 dir = input.UV - Center;
    float dist = length(dir);
    
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Blur en "from" (salida)
    float4 blurred = float4(0,0,0,0);
    float totalWeight = 0;
    
    for(int i = 0; i < Samples; i++)
    {
        float t = (float)i / (float)Samples;
        float scale = 1.0 + t * Progress * Strength;
        float weight = 1.0 - t;
        
        float2 sampleUV = Center + dir * scale;
        blurred += tex2D(PreviousSampler, sampleUV) * weight;
        totalWeight += weight;
    }
    blurred /= totalWeight;
    
    return lerp(blurred, to, Progress);
}

technique ZoomBlur
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_ZoomBlur();
    }
}
