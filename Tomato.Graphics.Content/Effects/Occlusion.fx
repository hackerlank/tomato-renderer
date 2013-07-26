#define SAMPLE_COUNT 4
#define FINAL_DIVISION 0.125
const static float2 RandomTextureDimension = float2( 64, 64 );

// SSAO parameters.
float SSAOSamplingRadius : SSAOSAMPDIST;
float SSAOIntensity : SSAOINT;
float SSAODistanceScale : SSAODISTSCALE;
float SSAOConeWidth : SSAOCONEWIDTH;

// View-space far-clipping plane.
float3 FarClippingCorners[ 4 ] : FARCLIPCORNERS;

// Render-target dimension;
float2 RenderTargetDimension : RTDIMENSION;

// Samplers.
sampler DepthNormalSampler : register( s0 );
sampler RandomSampler : register( s1 );

// Compute viewing ray in view-space.
float3 ComputeViewRay( float2 texCoord )
{
	float3 top = lerp( FarClippingCorners[ 0 ], FarClippingCorners[ 1 ], texCoord.x );
	float3 bottom = lerp( FarClippingCorners[ 2 ], FarClippingCorners[ 3 ], texCoord.x );
	return lerp( top, bottom, texCoord.y );
}

void GetViewDepthAndNormal(
	float2 TexCoord,
	out float3 Depth,
	out float3 Normal )
{
	float4 depthNormalBuffer = tex2D( DepthNormalSampler, TexCoord );

	// Get normal vector.
	Normal = depthNormalBuffer.xyz * 2.0f - 1.0f;

	// Get view depth.
	Depth = depthNormalBuffer.a;
}

float ComputeOcclusion(
	in float2 texCoord,
	in float2 offset,
	in float3 position,
	in float3 normal )
{
	// Get the view-space position and normal of the occluder.
	float sampleDepth;
	float3 sampleNormal;
	GetViewDepthAndNormal( texCoord + offset, sampleDepth, sampleNormal );

	float3 viewRay = ComputeViewRay( texCoord + offset );
	float3 samplePosition = viewRay * sampleDepth;

	float3 positionDelta = samplePosition - position;
	const float3 V = normalize( positionDelta );
	const float D = length( positionDelta ) * SSAODistanceScale;

	// Directional factor: (N*V) scaled into [cone_width, 1]
	// Attenuation by distance: 1/(1+d)
	//return smoothstep( SSAOConeWidth, 1, dot( normal, V ) ) * ( 1.0f / ( 1.0f + D ) );

	// For better performance, smoothstep() is replaced by max() function.
	// But, the side effect is that the upper bound is (1-cone_width).
	// To make up this, adjust the overall intensity a bit stronger.	
	return max( 0, dot( normal, V ) - SSAOConeWidth ) * ( 1.0f / ( 1.0f + D ) );
}

float ComputeSSAO( 
	in float2 texCoord,
	in float3 viewRay,
	in float3 position,
	in float3 normal ) 
{
	// Sampling direction vectors for various number of elements.
	// Note that these vectors have different lengths.
	//const float3 samples[ 16 ] = { float3(0.53812504, 0.18565957, -0.43192), float3( 0.13790712, 0.24864247, 0.44301823), float3( 0.33715037, 0.56794053, -0.005789503), float3( -0.6999805, -0.04511441, -0.0019965635), float3( 0.06896307, -0.15983082, -0.85477847), float3( 0.056099437, 0.006954967, -0.1843352), float3( -0.014653638, 0.14027752, 0.0762037), float3( 0.010019933, -0.1924225, -0.034443386), float3( -0.35775623, -0.5301969, -0.43581226), float3( -0.3169221, 0.106360726, 0.015860917), float3( 0.010350345, -0.58698344, 0.0046293875), float3( -0.08972908, -0.49408212, 0.3287904), float3( 0.7119986, -0.0154690035, -0.09183723), float3( -0.053382345, 0.059675813, -0.5411899), float3( 0.035267662, -0.063188605, 0.54602677), float3( -0.47761092, 0.2847911, -0.0271716) };
	//const float3 samples[ 8 ] = { float3(0.24710192, 0.6445882, 0.033550154), float3( 0.00991752, -0.21947019, 0.7196721), float3( 0.25109035, -0.1787317, -0.011580509), float3( -0.08781511, 0.44514698, 0.56647956), float3( -0.011737816, -0.0643377, 0.16030222), float3( 0.035941467, 0.04990871, -0.46533614), float3( -0.058801126, 0.7347013, -0.25399926), float3( -0.24799341, -0.022052078, -0.13399573) };
	//const float3 samples[ 12 ] = { float3(-0.13657719, 0.30651027, 0.16118456), float3( -0.14714938, 0.33245975, -0.113095455), float3( 0.030659059, 0.27887347, -0.7332209), float3( 0.009913514, -0.89884496, 0.07381549), float3( 0.040318526, 0.40091, 0.6847858), float3( 0.22311053, -0.3039437, -0.19340435), float3( 0.36235332, 0.21894878, -0.05407306), float3( -0.15198798, -0.38409665, -0.46785462), float3( -0.013492276, -0.5345803, 0.11307949), float3( -0.4972847, 0.037064247, -0.4381323), float3( -0.024175806, -0.008928787, 0.17719103), float3( 0.694014, -0.122672155, 0.33098832) };
	//const float3 samples[ 10 ] = { float3(-0.010735935, 0.01647018, 0.0062425877), float3( -0.06533369, 0.3647007, -0.13746321), float3( -0.6539235, -0.016726388, -0.53000957), float3( 0.40958285, 0.0052428036, -0.5591124), float3( -0.1465366, 0.09899267, 0.15571679), float3( -0.44122112, -0.5458797, 0.04912532), float3( 0.03755566, -0.10961345, -0.33040273), float3( 0.019100213, 0.29652783, 0.066237666), float3( 0.8765323, 0.011236004, 0.28265962), float3( 0.29264435, -0.40794238, 0.15964167) };

	// However, I used the simplest sampling pattern for efficiency. :)
	const float2 samples[ 4 ] = { float2( 1, 0 ), float2( -1, 0 ), float2( 0, 1 ), float2( 0, -1 ) };
	
	// Get a random direction vector.
	// Random texture is tiled over the screen.
	float2 random = tex2D( RandomSampler, RenderTargetDimension * texCoord / RandomTextureDimension ).xy;
	random = normalize( random * 2.0f - 1.0f );

	// Determine the sampling radius inversely proportionally to the view-depth.	
	float radius = SSAOSamplingRadius / -position.z;
	
	float occlusion = 0;
	[unroll] 
	for( int i = 0 ; i < SAMPLE_COUNT ; ++i )
	{
		// Get two texture-coordinate offset vectors.
		float2 offset1 = reflect( samples[ i ], random ) * radius;
		// Get the second offset by rotating the first one by 45 degrees.
		float2 offset2 = float2( 
			offset1.x * 0.707f - offset1.y * 0.707f,
			offset1.x * 0.707f + offset1.y * 0.707f );
			
		// Accumulate occlusion factors.
		occlusion += ComputeOcclusion( texCoord, offset1, position, normal );				
		occlusion += ComputeOcclusion( texCoord, offset2, position, normal );				
	}
	
	return SSAOIntensity * ( occlusion * FINAL_DIVISION );
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

float4 PS( VO input ) : COLOR0
{
	// Get the view-space position and normal
	float depth;
	float3 normal;
	GetViewDepthAndNormal( input.TexCoord0, depth, normal );
	float3 position = input.ViewRay * depth;

	// Compute screen-space ambient occlusion factor.
	float ambientOcclusion = saturate( ComputeSSAO( input.TexCoord0, input.ViewRay, position, normal ) );

	//ambientOcclusion = 1 - ambientOcclusion;
	return float4( ambientOcclusion.xxx, 0 );
}

technique Occlusion
{
    pass OcclusionPass
    {
        VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PS();
    }
}