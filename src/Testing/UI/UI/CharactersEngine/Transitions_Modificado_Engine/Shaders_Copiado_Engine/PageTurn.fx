#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Progress;
float2 Direction; // (1,0) = derecha, (-1,0) = izquierda
float ShadowWidth; // 0.1

float4 PS_PageTurn(VertexShaderOutput input) : COLOR
{
    float2 uv = input.UV;
    float proj = dot(uv - 0.5, normalize(Direction)) + 0.5;
    
    // Curva de la página (perspectiva simulada)
    float turn = smoothstep(Progress - 0.1, Progress, proj);
    float curve = sin(turn * 3.14159265) * 0.1 * (1.0 - abs(proj - 0.5) * 2);
    
    float2 distortedUV = uv;
    distortedUV.y += curve * Direction.x; // Curvatura según dirección
    
    float4 from = tex2D(PreviousSampler, distortedUV);
    float4 to = tex2D(NextSampler, uv);
    
    // Sombra en el pliegue
    float shadow = smoothstep(Progress - ShadowWidth, Progress, proj) * 
                   (1 - smoothstep(Progress, Progress + ShadowWidth, proj));
    float4 shadowColor = float4(0,0,0,0.5);
    
    float4 result = lerp(from, to, turn);
    return lerp(result, shadowColor, shadow * (1 - turn));
}

technique PageTurn
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PS_PageTurn();
    }
}
