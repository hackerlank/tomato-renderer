using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tomato.Graphics.Vertex;

namespace Tomato.Graphics
{
	public class LightingPass : SceneRenderPass
	{
		private const int LightsPerDraw = 100;

		private IList<Light> m_visibleLights = null;

		private Camera m_sceneCamera = null;

		private Effect m_effect = null;

		private VertexBuffer m_directionalLightsVertexBuffer = null;
		private IndexBuffer m_directionalLightsIndexBuffer = null;
		private VertexBuffer m_pointLightsVertexBuffer = null;
		private IndexBuffer m_pointLightsIndexBuffer = null;
		private VertexBuffer m_spotLightsVertexBuffer = null;
		private IndexBuffer m_spotLightsIndexBuffer = null;

		[Browsable( false )]
		public Effect Effect { get { return m_effect; } }

		public LightingPass( Renderer renderer, Camera sceneCamera )
			: base( renderer, "Lighting Pass" )
		{
			Camera = null;
			m_sceneCamera = sceneCamera;

			// Get the lighting-pass effect object.
			//m_effect = Renderer.LoadEffect( "Effects/Lighting" );
			m_effect = Renderer.LoadInternalEffect( "Effects/Lighting" );

			// Prepare buffer.
			m_directionalLightsVertexBuffer = Renderer.CreateVertexBuffer( "VB Directional Lights", DirectionalLightVertex.VertexDeclaration, LightsPerDraw * 4, BufferUsage.WriteOnly );
			m_directionalLightsIndexBuffer = Renderer.CreateIndexBuffer( "IB Directional Lights", IndexElementSize.SixteenBits, LightsPerDraw * 6, BufferUsage.WriteOnly );
			m_pointLightsVertexBuffer = Renderer.CreateVertexBuffer( "VB Point Lights", PointLightVertex.VertexDeclaration, LightsPerDraw * 4, BufferUsage.WriteOnly );
			m_pointLightsIndexBuffer = Renderer.CreateIndexBuffer( "IB Point Lights", IndexElementSize.SixteenBits, LightsPerDraw * 6, BufferUsage.WriteOnly );
			m_spotLightsVertexBuffer = Renderer.CreateVertexBuffer( "VB Spot Lights", SpotLightVertex.VertexDeclaration, LightsPerDraw * 4, BufferUsage.WriteOnly );
			m_spotLightsIndexBuffer  = Renderer.CreateIndexBuffer( "IB Spot Lights", IndexElementSize.SixteenBits, LightsPerDraw * 6, BufferUsage.WriteOnly );
		}

		public void SetVisibleLights( IList<Light> visibleLights )
		{
			m_visibleLights = visibleLights;
		}

		protected override void OnRender( TimeSpan elapsedTime )
		{
			if( m_visibleLights.Count > 0 )
			{
				// Set additional textures.
				if( Textures != null )
				{
					Textures.SetToRenderer( Renderer );
				}

				// Render direction lights.
				RenderDirectionalLights( ( from l in m_visibleLights where l.LightType == LightType.Directional select l ).ToArray() );

				// Render point lights.
				RenderPointLights( ( from l in m_visibleLights where l.LightType == LightType.Point select l ).ToArray() );

				// Render spot lights.
				RenderSpotLights( ( from l in m_visibleLights where l.LightType == LightType.Spot select l ).ToArray() );
			}
		}

		private void RenderDirectionalLights( Light[] lights )
		{
			if( lights.Length > 0 )
			{
				for( int draw = 0 ; draw < lights.Length ; draw += LightsPerDraw )
				{
					// Get the number of lights to draw for this step.
					int lightsToDraw = System.Math.Min( LightsPerDraw, lights.Length - draw );

					// Prepare vertices and indices array.
					DirectionalLightVertex[] vertices = new DirectionalLightVertex[ lightsToDraw * 4 ];
					ushort[] indices = new ushort[ lightsToDraw * 6 ];

					// For each light, 
					for( int i = 0 ; i < lightsToDraw ; ++i )
					{
						int lightIndex = draw + i;

						// Get the projected extent in [0,1]
						float x, y, width, height;
						lights[ lightIndex ].GetProjectedExtent( m_sceneCamera, out x, out y, out width, out height );

						// Get the four vertices of rectangular mesh.
						Vector3 position0 = new Vector3( x, y, 0 );
						Vector3 position1 = new Vector3( x, y + height, 0 );
						Vector3 position2 = new Vector3( x + width, y, 0 );
						Vector3 position3 = new Vector3( x + width, y + height, 0 );

						// Get the view-space direction vector
						Vector3 lightDirection = Vector3.TransformNormal( lights[ lightIndex ].WorldDirection, m_sceneCamera.ViewMatrix );

						// Set the vertices.
						vertices[ i * 4 + 0 ] = new DirectionalLightVertex(
							position0,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower );
						vertices[ i * 4 + 1 ] = new DirectionalLightVertex(
							position1,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower );
						vertices[ i * 4 + 2 ] = new DirectionalLightVertex(
							position2,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower );
						vertices[ i * 4 + 3 ] = new DirectionalLightVertex(
							position3,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower );

						// Set the indicies.
						indices[ i * 6 + 0 ] = ( ushort )( i * 4 + 0 );
						indices[ i * 6 + 1 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 2 ] = ( ushort )( i * 4 + 1 );
						indices[ i * 6 + 3 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 4 ] = ( ushort )( i * 4 + 3 );
						indices[ i * 6 + 5 ] = ( ushort )( i * 4 + 1 );
					}

					// Fill buffers.
					Renderer.ResetBuffers();
					m_directionalLightsVertexBuffer.SetData<DirectionalLightVertex>( vertices );
					m_directionalLightsIndexBuffer.SetData<ushort>( indices );

					// Apply an effect pass.
					Renderer.ApplyEffect( m_effect, "DeferredLighting.DirectionalLightingPass" );

					// Let it rendered.
					Renderer.Render( m_directionalLightsVertexBuffer, 0, m_directionalLightsIndexBuffer, PrimitiveType.TriangleList, indices.Length, 0, lightsToDraw * 2 );
				}
			}
		}

		private void RenderPointLights( Light[] lights )
		{
			if( lights.Length > 0 )
			{
				for( int draw = 0 ; draw < lights.Length ; draw += LightsPerDraw )
				{
					// Get the number of lights to draw for this step.
					int lightsToDraw = System.Math.Min( LightsPerDraw, lights.Length - draw );

					// Prepare vertices and indices array.
					PointLightVertex[] vertices = new PointLightVertex[ lightsToDraw * 4 ];
					ushort[] indices = new ushort[ lightsToDraw * 6 ];

					// For each light, 
					for( int i = 0 ; i < lightsToDraw ; ++i )
					{
						int lightIndex = draw + i;

						// Get the projected extent in [0,1]
						float x, y, width, height;
						lights[ lightIndex ].GetProjectedExtent( m_sceneCamera, out x, out y, out width, out height );

						// Get the four vertices of rectangular mesh.
						Vector3 position0 = new Vector3( x, y, 0 );
						Vector3 position1 = new Vector3( x, y + height, 0 );
						Vector3 position2 = new Vector3( x + width, y, 0 );
						Vector3 position3 = new Vector3( x + width, y + height, 0 );

						// Get the view-space light position.
						Vector3 lightPosition = Vector3.Transform( lights[ lightIndex ].WorldPosition, m_sceneCamera.ViewMatrix );

						// Set the vertices.
						vertices[ i * 4 + 0 ] = new PointLightVertex(
							position0,
							lightPosition,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent );
						vertices[ i * 4 + 1 ] = new PointLightVertex(
							position1,
							lightPosition,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent );
						vertices[ i * 4 + 2 ] = new PointLightVertex(
							position2,
							lightPosition,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent );
						vertices[ i * 4 + 3 ] = new PointLightVertex(
							position3,
							lightPosition,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent );

						// Set the indicies.
						indices[ i * 6 + 0 ] = ( ushort )( i * 4 + 0 );
						indices[ i * 6 + 1 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 2 ] = ( ushort )( i * 4 + 1 );
						indices[ i * 6 + 3 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 4 ] = ( ushort )( i * 4 + 3 );
						indices[ i * 6 + 5 ] = ( ushort )( i * 4 + 1 );
					}

					// Fill buffers.
					Renderer.ResetBuffers();
					m_pointLightsVertexBuffer.SetData<PointLightVertex>( vertices );
					m_pointLightsIndexBuffer.SetData<ushort>( indices );					

					// Apply an effect pass.
					Renderer.ApplyEffect( m_effect, "DeferredLighting.PointLightingPass" );

					// Let it rendered.
					Renderer.Render( m_pointLightsVertexBuffer, 0, m_pointLightsIndexBuffer, PrimitiveType.TriangleList, indices.Length, 0, lightsToDraw * 2 );
				}
			}
		}

		private void RenderSpotLights( Light[] lights )
		{
			if( lights.Length > 0 )
			{
				for( int draw = 0 ; draw < lights.Length ; draw += LightsPerDraw )
				{
					// Get the number of lights to draw for this step.
					int lightsToDraw = System.Math.Min( LightsPerDraw, lights.Length - draw );

					// Prepare vertices and indices array.
					SpotLightVertex[] vertices = new SpotLightVertex[ lightsToDraw * 4 ];
					ushort[] indices = new ushort[ lightsToDraw * 6 ];

					// For each light, 
					for( int i = 0 ; i < lightsToDraw ; ++i )
					{
						int lightIndex = draw + i;

						// Get the projected extent in [0,1]
						float x, y, width, height;
						lights[ lightIndex ].GetProjectedExtent( m_sceneCamera, out x, out y, out width, out height );

						// Get the four vertices of rectangular mesh.
						Vector3 position0 = new Vector3( x, y, 0 );
						Vector3 position1 = new Vector3( x, y + height, 0 );
						Vector3 position2 = new Vector3( x + width, y, 0 );
						Vector3 position3 = new Vector3( x + width, y + height, 0 );

						// Get the view-space light position.
						Vector3 lightPosition = Vector3.Transform( lights[ lightIndex ].WorldPosition, m_sceneCamera.ViewMatrix );

						// Get the view-space direction vector
						Vector3 lightDirection = Vector3.TransformNormal( lights[ lightIndex ].WorldDirection, m_sceneCamera.ViewMatrix );

						// Set the vertices.
						vertices[ i * 4 + 0 ] = new SpotLightVertex(
							position0,
							lightPosition,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent,
							lights[ lightIndex ].AttenuationInnerAngle,
							lights[ lightIndex ].AttenuationOuterAngle,
							lights[ lightIndex ].AttenuationAngleExponent );
						vertices[ i * 4 + 1 ] = new SpotLightVertex(
							position1,
							lightPosition,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent,
							lights[ lightIndex ].AttenuationInnerAngle,
							lights[ lightIndex ].AttenuationOuterAngle,
							lights[ lightIndex ].AttenuationAngleExponent );
						vertices[ i * 4 + 2 ] = new SpotLightVertex(
							position2,
							lightPosition,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent,
							lights[ lightIndex ].AttenuationInnerAngle,
							lights[ lightIndex ].AttenuationOuterAngle,
							lights[ lightIndex ].AttenuationAngleExponent );
						vertices[ i * 4 + 3 ] = new SpotLightVertex(
							position3,
							lightPosition,
							lightDirection,
							lights[ lightIndex ].DiffuseColor,
							lights[ lightIndex ].SpecularPower,
							lights[ lightIndex ].AttenuationDistance,
							lights[ lightIndex ].AttenuationDistanceExponent,
							lights[ lightIndex ].AttenuationInnerAngle,
							lights[ lightIndex ].AttenuationOuterAngle,
							lights[ lightIndex ].AttenuationAngleExponent );

						// Set the indicies.
						indices[ i * 6 + 0 ] = ( ushort )( i * 4 + 0 );
						indices[ i * 6 + 1 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 2 ] = ( ushort )( i * 4 + 1 );
						indices[ i * 6 + 3 ] = ( ushort )( i * 4 + 2 );
						indices[ i * 6 + 4 ] = ( ushort )( i * 4 + 3 );
						indices[ i * 6 + 5 ] = ( ushort )( i * 4 + 1 );
					}

					// Fill buffers.
					Renderer.ResetBuffers();
					m_spotLightsVertexBuffer.SetData<SpotLightVertex>( vertices );
					m_spotLightsIndexBuffer.SetData<ushort>( indices );

					// Apply an effect pass.
					Renderer.ApplyEffect( m_effect, "DeferredLighting.SpotLightingPass" );

					// Let it rendered.
					Renderer.Render( m_spotLightsVertexBuffer, 0, m_spotLightsIndexBuffer, PrimitiveType.TriangleList, indices.Length, 0, lightsToDraw * 2 );
				}
			}
		}
	}
}