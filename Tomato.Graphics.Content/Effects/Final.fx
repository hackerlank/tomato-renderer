// View-space far-clipping plane.
float3 FarClippingCorners[ 4 ] : FARCLIPCORNERS;

// Filmic tone mapping parameters.
//float ToneMappingKeyValue : FTMKV = 0.36f;
float ToneMappingExposureAdjustment : FTMEA = 10.0f;
float FilmicToneMapShoulderStrength : FTMSS = 0.15f;
float FilmicToneMapLinearStrength : FTMLS = 0.5f;
float FilmicToneMapLinearAngle : FTMLA = 0.1f;
float FilmicToneMapToeStrength : FTMTS = 0.2f;
float FilmicToneMapToeNumerator : FTMTN = 0.02f;
float FilmicToneMapToeDenominator : FTMTD = 0.3f;
float FilmicToneMapLinearWhitePoint : FTMWP = 11.2f;

// Depth-of-Field
float DepthOfFieldDistance : DOFDISTANCE = 10.0f;
float DepthOfFieldRange : DOFRANGE = 5.0f;

sampler SceneSampler : register( s0 ); 
sampler BlurredSceneSampler : register( s1 );
sampler DepthNormalSampler : register( s2 );
//sampler LuminanceSampler : register( s3 );

// Compute viewing ray in view-space.
float3 ComputeViewRay( float2 textureCoordinate )
{
	float3 top = lerp( FarClippingCorners[ 0 ], FarClippingCorners[ 1 ], textureCoordinate.x );
	float3 bottom = lerp( FarClippingCorners[ 2 ], FarClippingCorners[ 3 ], textureCoordinate.x );
	return lerp( top, bottom, textureCoordinate.y );
}

float3 FilmicToneMap( float3 X )
{
	float A = FilmicToneMapShoulderStrength;
    float B = FilmicToneMapLinearStrength;
    float C = FilmicToneMapLinearAngle;
    float D = FilmicToneMapToeStrength;
    float E = FilmicToneMapToeNumerator;
    float F = FilmicToneMapToeDenominator;

	return 
		( ( X * ( A * X + C * B ) + D * E ) 
		/ ( X * ( A * X + B ) + D * F ) ) 
		- E / F;
}

struct VI
{
	float4 Position : POSITION0;
	float2 TexCoord0 : TEXCOORD0;
};

struct VO
{
    float4 Position : POSITION0;
	float2 TexCoord0 : TEXCOORD0;
	float3 ViewRay : TEXCOORD1;
};

VO VS( VI input )
{
    VO output;

	output.Position = input.Position;
	output.TexCoord0 = input.TexCoord0;
	output.ViewRay = ComputeViewRay( input.TexCoord0 );

    return output;
}

float4 PS( 
	VO input,
	uniform int colorOutputOverride, 
	uniform bool bApplyDepthOfField = true,
	uniform bool bApplyToneMap = true ) : COLOR0
{
	// Get view-space position and normal vector from the depth-normal buffer.
	float viewDepth;
	float3 position;
	float3 normal;
	{
		float4 depthNormalBuffer = tex2D( DepthNormalSampler, input.TexCoord0 );

		// Get normal vector.
		normal = depthNormalBuffer.xyz * 2.0f - 1.0f;

		// Get view depth and view-space position
		viewDepth = depthNormalBuffer.a;
		position = input.ViewRay.xyz * viewDepth;
	}

	// Input scene color.
	float4 scene = tex2D( SceneSampler, input.TexCoord0 );
	float4 blurredScene = tex2D( BlurredSceneSampler, input.TexCoord0 );

	// Apply the depth-of-field.
	if( bApplyDepthOfField )
	{
        float blurFactor = smoothstep( 
			0.0f, 
			DepthOfFieldRange, 
			abs( ( viewDepth * -FarClippingCorners[ 0 ].z ) - DepthOfFieldDistance ) );
        scene = lerp( scene, blurredScene, blurFactor );
    }

	// Apply the filmic tone maping.
	if( bApplyToneMap )
	{
		// Need to retrieve this value from the real scene image.
		//float averageLuminance = tex2D( LuminanceSampler, float2( 0.5f, 0.5f ) ).r;
		//float exposureAdjustment = ToneMappingKeyValue / averageLuminance;
		float exposureAdjustment = ToneMappingExposureAdjustment;
		float exposureBias = 2.0f;

		float3 whiteScale = 1.0f / FilmicToneMap( FilmicToneMapLinearWhitePoint.xxx );
		scene.rgb = FilmicToneMap( scene.rgb * exposureAdjustment * exposureBias ) * whiteScale;		
	}

	// Apply gamma correction.
	scene.rgb = pow( abs( scene.rgb ), 1.0f / 2.2f );

	if( colorOutputOverride == 1 )
	{
		return float4( scene.r, 0, 0, 1 );
	}
	else if( colorOutputOverride == 2 )
	{
		return float4( 0, scene.g, 0, 1 );
	}
	else if( colorOutputOverride == 3 )
	{
		return float4( 0, 0, scene.b, 1 );
	}
	else if( colorOutputOverride == 4 )
	{
		return float4( scene.aaa, 1 );
	}
	else
	{
		return scene.rgba;
	}
}

technique Final
{
    pass DefaultPass
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 0, false, false );
    }

	pass DoFPass
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 0, true, false );
    }
	
	pass ToneMapPass
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 0, false, true );
    }

	pass DoFToneMapPass
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 0, true, true );
    }
	
	pass RedChannelOnlyPass
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 1 );
	}

	pass GreenChannelOnlyPass
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 2 );
	}

	pass BlueChannelOnlyPass
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 3 );
	}

	pass AlphaChannelOnlyPass
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS( 4 );
	}
}
