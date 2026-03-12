#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D NoiseTexture;
sampler2D NoiseSampler = sampler_state { Texture = <NoiseTexture>; };

float Progress;
float EdgeWidth; // 0.1
float4 EdgeColor;

// Función de ruido simple si no hay textura
float Random(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

float4 PS_NoiseDissolve(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    float noise = tex2D(NoiseSampler, input.UV).r;
    // Fallback: float noise = Random(input.UV * 100);
    
    float lower = Progress - EdgeWidth;
    float upper = Progress;
    
    float alpha = smoothstep(lower, upper, noise);
    
    // Borde brillante durante transición
    float edgeMask = smoothstep(lower, lower + EdgeWidth * 0.3, noise) * 
                     (1 - smoothstep(upper - EdgeWidth * 0.3, upper, noise));
    
    float4 result = lerp(from, to, alpha);
    return lerp(result, EdgeColor, edgeMask * EdgeColor.a);
}

technique NoiseDissolve
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_NoiseDissolve();
    }
}
