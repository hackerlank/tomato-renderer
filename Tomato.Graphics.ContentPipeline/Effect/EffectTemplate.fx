// Light buffer
sampler LightSampler : register( s5 );

// Occlusion buffer
sampler OcclusionSampler : register( s6 );

struct GeometryPassVI
{
    float4 Position : POSITION0;
#if VERTEX_HAS_COLOR0
	float4 Color0 : COLOR0;
#endif
#if VERTEX_HAS_NORMAL
	float4 Normal : NORMAL0;
#endif
#if VERTEX_HAS_BINORMAL
	float4 Binormal : BINORMAL0;
#endif
#if VERTEX_HAS_TANGENT
	float4 Tangent : TANGENT0;
#endif
#if VERTEX_HAS_TEXCOORD0
	float2 TexCoord0 : TEXCOORD0;
#endif
#if VERTEX_HAS_TEXCOORD1
	float2 TexCoord1 : TEXCOORD1;
#endif
#if VERTEX_HAS_TEXCOORD2
	float2 TexCoord2 : TEXCOORD2;
#endif
};

struct GeometryPassVO
{
    float4 Position : POSITION0;
#if VERTEX_HAS_NORMAL
	float4 Normal : TEXCOORD0;
#endif
#if VERTEX_HAS_BINORMAL
	float4 Binormal : TEXCOORD1;
#endif
#if VERTEX_HAS_TANGENT
	float4 Tangent : TEXCOORD2;
#endif
	float ViewDepth : TEXCOORD3;
};

struct MaterialPassVI
{
    float4 Position : POSITION0;
#if VERTEX_HAS_COLOR0
	float4 Color0 : COLOR0;
#endif
#if VERTEX_HAS_NORMAL
	float4 Normal : NORMAL0;
#endif
#if VERTEX_HAS_BINORMAL
	float4 Binormal : BINORMAL0;
#endif
#if VERTEX_HAS_TANGENT
	float4 Tangent : TANGENT0;
#endif
#if VERTEX_HAS_TEXCOORD0
	float2 TexCoord0 : TEXCOORD0;
#endif
#if VERTEX_HAS_TEXCOORD1
	float2 TexCoord1 : TEXCOORD1;
#endif
#if VERTEX_HAS_TEXCOORD2
	float2 TexCoord2 : TEXCOORD2;
#endif
};

struct MaterialPassVO
{
    float4 Position : POSITION0;
#if VERTEX_HAS_COLOR0
	float4 Color0 : COLOR0;
#endif
#if VERTEX_HAS_TEXCOORD0
	float2 TexCoord0 : TEXCOORD0;
#endif
	float4 ScreenPosition : TEXCOORD1;
};

// Compute viewing ray in view-space.
float3 ComputeViewRay( float2 textureCoordinate )
{
	float3 top = lerp( FarClippingCorners[ 0 ], FarClippingCorners[ 1 ], textureCoordinate.x );
	float3 bottom = lerp( FarClippingCorners[ 2 ], FarClippingCorners[ 3 ], textureCoordinate.x );
	return lerp( top, bottom, textureCoordinate.y );
}

GeometryPassVO GeometryPassVS( GeometryPassVI Input )
{
    GeometryPassVO output = ( GeometryPassVO )0;

	// Position
    output.Position = mul( Input.Position, WorldViewProjection );

	// View-depth
	output.ViewDepth = mul( Input.Position, WorldView ).z;

	// Normal
#if VERTEX_HAS_NORMAL
	output.Normal = mul( Input.Normal.xyz, (float3x4)WorldView );
#endif

    return output;
}

float4 GeometryPassPS( GeometryPassVO Input ) : COLOR0
{
	// Normalized view-depth in [0,1].
	float viewDepth = -Input.ViewDepth / CameraProjectionParameters.w;

	// View-space normal.
	float3 viewNormal = ( normalize( Input.Normal.xyz ) + 1.0f ) * 0.5f;

	return float4( viewNormal, viewDepth );
}

MaterialPassVO MaterialPassVS( MaterialPassVI Input )
{
    MaterialPassVO output = ( MaterialPassVO )0;

	// Position
	float4 projection = mul( Input.Position, WorldViewProjection );
    output.Position = projection;
	output.ScreenPosition = float4( 
		0.5 * ( float2( projection.x + projection.w, projection.w - projection.y ) + projection.w * RenderTargetDimension.zw ),
		projection.zw );

	// Color0
#if VERTEX_HAS_COLOR0
	output.Color0 = Input.Color0;
#endif

	// TexCoord0
#if VERTEX_HAS_TEXCOORD0
	output.TexCoord0 = Input.TexCoord0;
#endif

    return output;
}

float4 MaterialPassPS( 
	MaterialPassVO Input ) : COLOR0
{
	// Compute normalized (x,y) coorinate in [0,1].
	float2 deviceCoordinate = Input.ScreenPosition.xy / Input.ScreenPosition.w;

	// Get the diffuse and specular term of accumulated lights from the light buffer.
	float3 lightDiffuse = 0;
	float3 lightSpecular = 0;
	{
		float4 lightBuffer = tex2D( LightSampler, deviceCoordinate );

		// Diffuse term.
		lightDiffuse = lightBuffer.rgb;

		// Specular term.
		// Light buffer's alpha channel contains a specular term value which is 
		//		pow( N.H, LightShineness ) * attenuation * N.L
		// . And we don't have [attenuation * N.L] value separately, 
		// we approximate that value with the luminance value of diffuse term.
		float diffuseLuminance = max( dot( float3( 0.2125f, 0.7154f, 0.0721f ), lightDiffuse ), 0.01f );

		// And then, to apply object's shineness, we compute the specular term as
		//		pow( pow( N.H, LightShineness ), ObjectShineness ) * ( attenuation * N.L )
		// where [attenuation * N.L] is approximated above.
		lightSpecular = pow( saturate( lightBuffer.a / diffuseLuminance ), ObjectShineness );
		lightSpecular *= diffuseLuminance;		
	}

	// Get occlusion amount.
	float occlusion = tex2D( OcclusionSampler, deviceCoordinate ).r;

	// Vertex color
	float3 vertexColor = 0;
#if VERTEX_HAS_COLOR0
	vertexColor = Input.Color0.rgb;
#endif

	// Diffuse texture
	float4 diffuseMap = 0;
#if VERTEX_HAS_TEXCOORD0 && DIFFUSE_MAP_ENABLED
	diffuseMap = tex2D( DiffuseSampler, Input.TexCoord0 );
	
	// Gamma correction
	diffuseMap.rgb = pow( abs( diffuseMap.rgb ), 2.2f );
#endif

	// Input diffuse color
	float3 diffuseColor;
#if VERTEX_HAS_COLOR0
	diffuseColor = vertexColor;
#elif VERTEX_HAS_TEXCOORD0 && DIFFUSE_MAP_ENABLED
	diffuseColor = diffuseMap.rgb;
#else
	diffuseColor = float3( 1, 1, 1 );
#endif

	float3 finalColor = 
		diffuseColor * ( ObjectEmissive + lightDiffuse * ObjectDiffuse )
		+ ( lightSpecular * ObjectSpecular );

	finalColor *= ( 1.0f - occlusion );

	return float4( finalColor, 1 );
}

technique DeferredLighting
{
    pass GeometryPass
	{
		AlphaBlendEnable = FALSE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;

        VertexShader = compile vs_2_0 GeometryPassVS();
        PixelShader = compile ps_2_0 GeometryPassPS();
    }

	pass MaterialPass
	{
		AlphaBlendEnable = FALSE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;

		VertexShader = compile vs_2_0 MaterialPassVS();
		PixelShader = compile ps_2_0 MaterialPassPS();
	}
}
