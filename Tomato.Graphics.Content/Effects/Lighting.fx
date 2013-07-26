const static float ClipThreshold = 0.000001f;

// View-space far-clipping plane.
float3 FarClippingCorners[ 4 ] : FARCLIPCORNERS;

// Half-texel offset.
float2 HalfTexelOffset : HALFTEXOFFSET;

// Depth-normal buffer texture
sampler DepthNormalBufferSampler : register( s0 );

struct DirectionalLightVI
{
    float4 Position : POSITION0;
	float3 LightDirection : TEXCOORD0;
	float4 LightColor : TEXCOORD1;
};

struct DirectionalLightVO
{
    float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
	float3 LightDirection : TEXCOORD1;
	float4 LightColor : TEXCOORD2;
	float3 ViewRay : TEXCOORD3;
};

struct PointLightVI
{
    float4 Position : POSITION0;
	float3 LightPosition : TEXCOORD0;
	float4 LightColor : TEXCOORD1;
	float2 LightAttenuation : TEXCOORD2;
};

struct PointLightVO
{
    float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
	float3 LightPosition : TEXCOORD1;
	float4 LightColor : TEXCOORD2;
	float2 LightAttenuation : TEXCOORD3;
	float3 ViewRay : TEXCOORD4;
};

struct SpotLightVI
{
    float4 Position : POSITION0;
	float3 LightPosition : TEXCOORD0;
	float3 LightDirection : TEXCOORD1;
	float4 LightColor : TEXCOORD2;
	float2 LightAttenuation : TEXCOORD3;
	float3 LightSpotAttenuation : TEXCOORD4;
};

struct SpotLightVO
{
    float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
	float3 LightPosition : TEXCOORD1;
	float3 LightDirection : TEXCOORD2;
	float4 LightColor : TEXCOORD3;
	float2 LightAttenuation : TEXCOORD4;
	float3 LightSpotAttenuation : TEXCOORD5;
	float3 ViewRay : TEXCOORD6;
};

// Compute viewing ray in view-space.
float3 ComputeViewRay( float2 textureCoordinate )
{
	float3 top = lerp( FarClippingCorners[ 0 ], FarClippingCorners[ 1 ], textureCoordinate.x );
	float3 bottom = lerp( FarClippingCorners[ 2 ], FarClippingCorners[ 3 ], textureCoordinate.x );
	return lerp( top, bottom, textureCoordinate.y );
}

DirectionalLightVO DirectionalLightVS( DirectionalLightVI Input )
{
    DirectionalLightVO output = ( DirectionalLightVO )0;

	// Input position is in [0,1] space.
	// So get the [-1,1] space coordinate.
	float2 projected = ( Input.Position.xy * 2.0f - 1.0f ) * float2( 1, -1 );

	// Position
	output.Position = float4( projected.xy, 0, 1 );
	output.ScreenPosition = float4( Input.Position.xy, 0, 1 );
	output.ScreenPosition.xy += HalfTexelOffset;

	// View-ray
	output.ViewRay = ComputeViewRay( Input.Position.xy );

	// Light properties
	output.LightDirection = Input.LightDirection;
	output.LightColor = Input.LightColor;

    return output;
}

float4 DirectionalLightPS( DirectionalLightVO Input ) : COLOR0
{
	float2 deviceCoordinate = Input.ScreenPosition.xy;

	// Get view-space position and normal vector from the depth-normal buffer.
	float3 position;
	float3 normal;
	{
		float4 depthNormalBuffer = tex2D( DepthNormalBufferSampler, deviceCoordinate );

		// Get normal vector.
		normal = depthNormalBuffer.xyz * 2.0f - 1.0f;

		// Get view depth and view-space position
		float viewDepth = depthNormalBuffer.a;
		position = Input.ViewRay.xyz * viewDepth;
	}

	// N dot L
	float ndotl = max( 0, dot( -Input.LightDirection, normal ) );

	float3 diffuseTerm = Input.LightColor.rgb * ndotl;
	float specularTerm = 0;
	
	return float4( diffuseTerm, specularTerm );	
}

PointLightVO PointLightVS( PointLightVI Input )
{
    PointLightVO output = ( PointLightVO )0;

	// Input position is in [0,1] space.
	// So get the [-1,1] space coordinate.
	float2 projected = ( Input.Position.xy * 2.0f - 1.0f ) * float2( 1, -1 );

	// Position
	output.Position = float4( projected.xy, 0, 1 );
	output.ScreenPosition = float4( Input.Position.xy, 0, 1 );
	output.ScreenPosition.xy += HalfTexelOffset;

	// View-ray
	output.ViewRay = ComputeViewRay( Input.Position.xy );

	// Light properties
	output.LightPosition = Input.LightPosition;
	output.LightColor = Input.LightColor;
	output.LightAttenuation = Input.LightAttenuation;

    return output;
}

float4 PointLightPS( PointLightVO Input ) : COLOR0
{
	float2 deviceCoordinate = Input.ScreenPosition.xy;

	// Get view-space position and normal vector from the depth-normal buffer.
	float3 position;
	float3 normal;
	{
		float4 depthNormalBuffer = tex2D( DepthNormalBufferSampler, deviceCoordinate );

		// Get normal vector.
		normal = depthNormalBuffer.xyz * 2.0f - 1.0f;

		// Get view depth and view-space position
		float viewDepth = depthNormalBuffer.a;
		position = Input.ViewRay.xyz * viewDepth;
	}

	// Distance to the light.
	float3 toLight = Input.LightPosition - position;
	float distance = length( toLight );
	toLight = normalize( toLight );

	// N dot L
	float ndotl = max( 0, dot( toLight, normal ) );

	// Attenuation factor
	float distanceAttenuation = pow( abs( 1 - smoothstep( 0, Input.LightAttenuation.x, distance ) ), Input.LightAttenuation.y );

	// Clip out.
	clip( ndotl * distanceAttenuation - ClipThreshold );

	// N dot H
	float3 half = normalize( toLight + normalize( -position ) );
	float ndoth = max( 0.00001f, dot( normal, half ) );

	float3 diffuseTerm = Input.LightColor.rgb * ndotl * distanceAttenuation;
	float specularTerm = pow( ndoth, Input.LightColor.a ) * ndotl * distanceAttenuation;
	
	return float4( diffuseTerm, specularTerm );	
}

SpotLightVO SpotLightVS( SpotLightVI Input )
{
    SpotLightVO output = ( SpotLightVO )0;

	// Input position is in [0,1] space.
	// So get the [-1,1] space coordinate.
	float2 projected = ( Input.Position.xy * 2.0f - 1.0f ) * float2( 1, -1 );

	// Position
	output.Position = float4( projected.xy, 0, 1 );
	output.ScreenPosition = float4( Input.Position.xy, 0, 1 );
	output.ScreenPosition.xy += HalfTexelOffset;

	// View-ray
	output.ViewRay = ComputeViewRay( Input.Position.xy );

	// Light properties
	output.LightPosition = Input.LightPosition;
	output.LightDirection = Input.LightDirection;
	output.LightColor = Input.LightColor;
	output.LightAttenuation = Input.LightAttenuation;
	output.LightSpotAttenuation = Input.LightSpotAttenuation;

    return output;
}

float4 SpotLightPS( SpotLightVO Input ) : COLOR0
{
	float2 deviceCoordinate = Input.ScreenPosition.xy;

	// Get view-space position and normal vector from the depth-normal buffer.
	float3 position;
	float3 normal;
	{
		float4 depthNormalBuffer = tex2D( DepthNormalBufferSampler, deviceCoordinate );

		// Get normal vector.
		normal = depthNormalBuffer.xyz * 2.0f - 1.0f;

		// Get view depth and view-space position
		float viewDepth = depthNormalBuffer.a;
		position = Input.ViewRay.xyz * viewDepth;
	}

	// Distance to the light.
	float3 toLight = Input.LightPosition - position;
	float distance = length( toLight );
	toLight = normalize( toLight );

	// N dot L
	float ndotl = max( 0, dot( toLight, normal ) );

	// Attenuation factor
	float attenuation = pow( abs( 1 - smoothstep( 0, Input.LightAttenuation.x, distance ) ), Input.LightAttenuation.y );

	// Spot attenuation
	float A = dot( -toLight, Input.LightDirection.xyz );
	float M = Input.LightSpotAttenuation.x - Input.LightSpotAttenuation.y;
	float V = A - Input.LightSpotAttenuation.y;
	attenuation *= pow( smoothstep( 0, M, V ), Input.LightSpotAttenuation.z );

	// Clip out.
	clip( ndotl * attenuation - ClipThreshold );

	// N dot H
	float3 half = normalize( toLight + normalize( -position ) );
	float ndoth = max( 0.00001f, dot( normal, half ) );

	float3 diffuseTerm = Input.LightColor.rgb * ndotl * attenuation;
	float specularTerm = pow( ndoth, Input.LightColor.a ) * ndotl * attenuation;
	
	return float4( diffuseTerm, specularTerm );	
}

technique DeferredLighting
{
	pass DirectionalLightingPass
	{
		AlphaBlendEnable = TRUE;
		SrcBlend = ONE;
		DestBlend = ONE;
		ZEnable = FALSE;
		ZWriteEnable = FALSE;

		VertexShader = compile vs_2_0 DirectionalLightVS();
		PixelShader = compile ps_2_0 DirectionalLightPS();
	}
	
	pass PointLightingPass
	{
		AlphaBlendEnable = TRUE;
		SrcBlend = ONE;
		DestBlend = ONE;
		ZEnable = FALSE;
		ZWriteEnable = FALSE;

		VertexShader = compile vs_2_0 PointLightVS();
		PixelShader = compile ps_2_0 PointLightPS();
	}	

	pass SpotLightingPass
	{
		AlphaBlendEnable = TRUE;
		SrcBlend = ONE;
		DestBlend = ONE;
		ZEnable = FALSE;
		ZWriteEnable = FALSE;

		VertexShader = compile vs_2_0 SpotLightVS();
		PixelShader = compile ps_2_0 SpotLightPS();
	}	
}