#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 Direction; // (1,0) = derecha, (0,1) = abajo, (-1,1) = diagonal
float Smoothness; // 0 = duro, 0.5 = suave

float4 PS_Wipe(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Proyección de UV en dirección del wipe
    float2 dir = normalize(Direction);
    float projected = dot(input.UV - 0.5, dir) + 0.5;
    
    float edge = smoothstep(Progress - Smoothness, Progress, projected);
    return lerp(from, to, edge);
}

technique Wipe
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_Wipe();
    }
}
