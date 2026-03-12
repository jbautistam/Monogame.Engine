#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 Center; // 0.5, 0.5 = centro
float Shape; // 0 = círculo, 1 = rectángulo, 0.5 = ovalo
float Smoothness;

float4 PS_Iris(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    float2 diff = input.UV - Center;
    diff.x *= Shape; // Deformar para formas
    
    float dist = length(diff);
    float maxDist = length(float2(Shape, 1.0));
    
    float normalizedDist = dist / maxDist;
    float edge = smoothstep(Progress - Smoothness, Progress, 1.0 - normalizedDist);
    
    return lerp(from, to, edge);
}

technique Iris
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Iris();
    }
}
