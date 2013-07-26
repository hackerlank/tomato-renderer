static const float3 LuminanceVector = float3( 0.2125f, 0.7154f, 0.0721f );

static const float SampleDistance0 = 1.0f / ( 64.0f * 3.0f );
static const float SampleDistance1 = 1.0f / 64.0f;
static const float SampleDistance2 = 1.0f / 16.0f;
static const float SampleDistance3 = 1.0f / 4.0f;

sampler SceneSampler : register( s0 ); 

static const float2 TextureOffsets0[ 9 ] = 
{
	float2( -SampleDistance0, -SampleDistance0 ),
	float2( -SampleDistance0, 0 ),
	float2( -SampleDistance0, +SampleDistance0 ),
	float2( 0, -SampleDistance0 ),
	float2( 0, 0 ),
	float2( 0, +SampleDistance0 ),
	float2( +SampleDistance0, -SampleDistance0 ),
	float2( +SampleDistance0, 0 ),
	float2( +SampleDistance0, +SampleDistance0 ) 
};

float2 TextureOffsets1[ 16 ] =
{
	float2( -1.5f * SampleDistance1, -1.5f * SampleDistance1 ),
	float2( -1.5f * SampleDistance1, -0.5f * SampleDistance1 ),
	float2( -1.5f * SampleDistance1, +0.5f * SampleDistance1 ),
	float2( -1.5f * SampleDistance1, +1.5f * SampleDistance1 ),	
	float2( -0.5f * SampleDistance1, -1.5f * SampleDistance1 ),
	float2( -0.5f * SampleDistance1, -0.5f * SampleDistance1 ),
	float2( -0.5f * SampleDistance1, +0.5f * SampleDistance1 ),
	float2( -0.5f * SampleDistance1, +1.5f * SampleDistance1 ),	
	float2( +0.5f * SampleDistance1, -1.5f * SampleDistance1 ),
	float2( +0.5f * SampleDistance1, -0.5f * SampleDistance1 ),
	float2( +0.5f * SampleDistance1, +0.5f * SampleDistance1 ),
	float2( +0.5f * SampleDistance1, +1.5f * SampleDistance1 ),	
	float2( +1.5f * SampleDistance1, -1.5f * SampleDistance1 ),
	float2( +1.5f * SampleDistance1, -0.5f * SampleDistance1 ),
	float2( +1.5f * SampleDistance1, +0.5f * SampleDistance1 ),
	float2( +1.5f * SampleDistance1, +1.5f * SampleDistance1 )
};

float2 TextureOffsets2[ 16 ] =
{
	float2( -1.5f * SampleDistance2, -1.5f * SampleDistance2 ),
	float2( -1.5f * SampleDistance2, -0.5f * SampleDistance2 ),
	float2( -1.5f * SampleDistance2, +0.5f * SampleDistance2 ),
	float2( -1.5f * SampleDistance2, +1.5f * SampleDistance2 ),	
	float2( -0.5f * SampleDistance2, -1.5f * SampleDistance2 ),
	float2( -0.5f * SampleDistance2, -0.5f * SampleDistance2 ),
	float2( -0.5f * SampleDistance2, +0.5f * SampleDistance2 ),
	float2( -0.5f * SampleDistance2, +1.5f * SampleDistance2 ),	
	float2( +0.5f * SampleDistance2, -1.5f * SampleDistance2 ),
	float2( +0.5f * SampleDistance2, -0.5f * SampleDistance2 ),
	float2( +0.5f * SampleDistance2, +0.5f * SampleDistance2 ),
	float2( +0.5f * SampleDistance2, +1.5f * SampleDistance2 ),	
	float2( +1.5f * SampleDistance2, -1.5f * SampleDistance2 ),
	float2( +1.5f * SampleDistance2, -0.5f * SampleDistance2 ),
	float2( +1.5f * SampleDistance2, +0.5f * SampleDistance2 ),
	float2( +1.5f * SampleDistance2, +1.5f * SampleDistance2 )
};

float2 TextureOffsets3[ 16 ] =
{
	float2( -1.5f * SampleDistance3, -1.5f * SampleDistance3 ),
	float2( -1.5f * SampleDistance3, -0.5f * SampleDistance3 ),
	float2( -1.5f * SampleDistance3, +0.5f * SampleDistance3 ),
	float2( -1.5f * SampleDistance3, +1.5f * SampleDistance3 ),	
	float2( -0.5f * SampleDistance3, -1.5f * SampleDistance3 ),
	float2( -0.5f * SampleDistance3, -0.5f * SampleDistance3 ),
	float2( -0.5f * SampleDistance3, +0.5f * SampleDistance3 ),
	float2( -0.5f * SampleDistance3, +1.5f * SampleDistance3 ),	
	float2( +0.5f * SampleDistance3, -1.5f * SampleDistance3 ),
	float2( +0.5f * SampleDistance3, -0.5f * SampleDistance3 ),
	float2( +0.5f * SampleDistance3, +0.5f * SampleDistance3 ),
	float2( +0.5f * SampleDistance3, +1.5f * SampleDistance3 ),	
	float2( +1.5f * SampleDistance3, -1.5f * SampleDistance3 ),
	float2( +1.5f * SampleDistance3, -0.5f * SampleDistance3 ),
	float2( +1.5f * SampleDistance3, +0.5f * SampleDistance3 ),
	float2( +1.5f * SampleDistance3, +1.5f * SampleDistance3 )
};


struct VI
{
	float4 Position : POSITION0;
	float2 TexCoord0 : TEXCOORD0;
};

struct VO
{
    float4 Position : POSITION0;
	float2 TexCoord0 : TEXCOORD0;
};

VO VS( VI input )
{
    VO output;

	output.Position = input.Position;
	output.TexCoord0 = input.TexCoord0;

    return output;
}

float4 Luminance0PS( VO input ) : COLOR0
{
	float logSum = 0;

	[unroll]
	for( int i = 0 ; i < 9 ; i++ )
	{
		float4 sample = tex2D( SceneSampler, input.TexCoord0 + TextureOffsets0[ i ] );
		
		logSum += log( dot( sample.rgb, LuminanceVector ) + 0.0001f );
	}
	
	logSum /= 9.0f;
    
	return float4( logSum, 0, 0, 0 );
}

float4 Luminance1PS( VO input ) : COLOR0
{
	float4 sample = 0.0f;

	[unroll]
	for( int i = 0 ; i < 16 ; i++ )
	{
		sample += tex2D( SceneSampler, input.TexCoord0 + TextureOffsets1[ i ] );
	}
    
	return ( sample / 16 );
}

float4 Luminance2PS( VO input ) : COLOR0
{
	float4 sample = 0.0f;

	[unroll]
	for( int i = 0 ; i < 16 ; i++ )
	{
		sample += tex2D( SceneSampler, input.TexCoord0 + TextureOffsets2[ i ] );
	}
    
	return ( sample / 16 );
}

float4 Luminance3PS( VO input ) : COLOR0
{
	float logSum = 0.0f;

	[unroll] 
	for( int i = 0 ; i < 16 ; i++ )
	{
		logSum += tex2D( SceneSampler, input.TexCoord0 + TextureOffsets3[ i ] ).r;
	}

	return float4( exp( logSum / 16.0f ), 0, 0, 0 );
}

technique Luminance
{
    pass Luminance0
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 Luminance0PS();
    }

	pass Luminance1
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 Luminance1PS();
	}

	pass Luminance2
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 Luminance2PS();
	}

	pass Luminance3
	{
		VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 Luminance3PS();
	}
}
