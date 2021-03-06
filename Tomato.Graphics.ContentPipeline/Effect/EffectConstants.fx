// Render-target dimension
// x: width
// y: height
// z: 1 / width
// w: 1 / height
float4 RenderTargetDimension : RTDIMENSION;

// Transformation matrices
float4x4 World : WORLD;
float4x4 View : VIEW;
float4x4 Projection : PROJECTION;
float4x4 WorldView : WORLDVIEW;
float4x4 ViewProjection : VIEWPROJECTION;
float4x4 WorldViewProjection : WORLDVIEWPROJECTION;

// Object material properties
float3 ObjectEmissive : OBJECTEMISSIVE;
float3 ObjectDiffuse : OBJECTDIFFUSE;
float3 ObjectSpecular : OBJECTSPECULAR;
float ObjectShineness : OBJECTSHINENESS;

// Camera projection parameters 
// x: field of view
// y: aspect ratio
// z: near clip
// w: far clip
float4 CameraProjectionParameters : PROJECTIONPARAMS;

// View-space far-clipping plane.
float3 FarClippingCorners[ 4 ] : FARCLIPCORNERS;

// Diffuse texture and sampler
#if DIFFUSE_MAP_ENABLED
texture DiffuseTexture : DIFFUSEMAP;
sampler DiffuseSampler : register( s0 );
#endif
