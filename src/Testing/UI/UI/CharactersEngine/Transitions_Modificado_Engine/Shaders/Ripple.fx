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
float Frequency; // 20.0
float Amplitude; // 0.02
float Speed;

float4 PS_Ripple(VertexShaderOutput input) : COLOR
{
    float2 diff = input.UV - Center;
    float dist = length(diff);
    
    float wave = sin(dist * Frequency - Progress * Speed) * Amplitude * (1.0 - Progress);
    float2 rippleUV = input.UV + normalize(diff) * wave;
    
    float4 from = tex2D(PreviousSampler, rippleUV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Distorsión máxima en medio de transición
    float distortion = sin(Progress * 3.14159265);
    
    return lerp(from, to, Progress);
}

technique Ripple
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Ripple();
    }
}
