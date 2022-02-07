sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float uIntensity;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 Ubercharged(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float time = (sin(uTime * 16) + 1) / 4 + 0.5f;
	//float wave = 1 - frac(coords.x + uTime * 2);
	color.r *= time;
	color.g *= 0;
	color.b *= 0;
	return color * sampleColor;
}

float4 Uberscreen(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float2 target = float2(0.5, 0.5);
	float d = length(target - coords);
	float ratio = 4 * d * d * uIntensity * uOpacity;
	color.rgb *= 1 - ratio;
	color.rgb += uColor * ratio;

	return color;
}

technique UberShader
{
	pass Ubercharged
	{
		PixelShader = compile ps_2_0 Ubercharged();
	}

	pass Uberscreen
	{
		PixelShader = compile ps_2_0 Uberscreen();
	}
}