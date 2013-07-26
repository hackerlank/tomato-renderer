using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Tomato.Graphics
{
	partial class Renderer
	{
		public RenderTarget2D CreateRenderTarget( string name, SurfaceFormat surfaceFormat )
		{
			return CreateRenderTarget( name, 1.0f, false, surfaceFormat, DepthFormat.None );
		}

		public RenderTarget2D CreateRenderTarget( string name, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			return CreateRenderTarget( name, 1.0f, false, surfaceFormat, depthFormat );
		}

		public RenderTarget2D CreateRenderTarget( string name, int width, int height, SurfaceFormat surfaceFormat )
		{
			return CreateRenderTarget( name, width, height, false, surfaceFormat, DepthFormat.None );
		}

		public RenderTarget2D CreateRenderTarget( string name, int width, int height, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			return CreateRenderTarget( name, width, height, false, surfaceFormat, depthFormat );
		}

		public RenderTarget2D CreateRenderTarget( string name, int width, int height, bool bMipMap, SurfaceFormat surfaceFormat )
		{
			return CreateRenderTarget( name, width, height, 0, bMipMap, surfaceFormat, DepthFormat.None );
		}

		public RenderTarget2D CreateRenderTarget( string name, int width, int height, bool bMipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			return CreateRenderTarget( name, width, height, 0, bMipMap, surfaceFormat, depthFormat );
		}

		public RenderTarget2D CreateRenderTarget( string name, float scaleToDefaultBackBuffer, SurfaceFormat surfaceFormat )
		{
			return CreateRenderTarget( name, scaleToDefaultBackBuffer, false, surfaceFormat );
		}

		public RenderTarget2D CreateRenderTarget( string name, float scaleToDefaultBackBuffer, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			return CreateRenderTarget( name, scaleToDefaultBackBuffer, false, surfaceFormat, depthFormat );
		}

		public RenderTarget2D CreateRenderTarget( string name, float scaleToDefaultBackBuffer, bool bMipMap, SurfaceFormat surfaceFormat )
		{
			return CreateRenderTarget( name, scaleToDefaultBackBuffer, bMipMap, surfaceFormat, DepthFormat.None );
		}

		public RenderTarget2D CreateRenderTarget( string name, float scaleToDefaultBackBuffer, bool bMipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			int width, height;
			ComputeRelativeRenderTargetSize( scaleToDefaultBackBuffer, out width, out height );

			return CreateRenderTarget( name, width, height, scaleToDefaultBackBuffer, bMipMap, surfaceFormat, depthFormat );
		}

		private RenderTarget2D CreateRenderTarget( string name, int width, int height, float sizingFactor, bool bMipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat )
		{
			bool bRenderTargetRecreated = false;

			// Check whether render-target of the same name does exist.
			RenderTarget2D existingRenderTarget;
			if( m_renderTargets.TryGetValue( name, out existingRenderTarget ) )
			{
				// Dispose and set flag for event.
				existingRenderTarget.Dispose();
				bRenderTargetRecreated = true;
			}

			RenderTarget2D renderTarget = new RenderTarget2D( m_graphicsDevice, width, height, bMipMap, surfaceFormat, depthFormat, 0, RenderTargetUsage.PlatformContents );
#if DEBUG
			if( renderTarget.DepthStencilFormat != depthFormat )
			{
				Debug.WriteLine( "Warning: Render-target [{0}] was created with DepthFormat of {1}.", name, Enum.GetName( typeof( DepthFormat ), renderTarget.DepthStencilFormat ) );
			}
			if( renderTarget.Format != surfaceFormat )
			{
				Debug.WriteLine( "Warning: Render-target [{0}] was created with SurfaceFormat of {1}.", name, Enum.GetName( typeof( SurfaceFormat ), renderTarget.Format ) );
			}
#endif
			
			// Set resource name.
			renderTarget.Name = name;

			// Update cache.
			m_renderTargets[ name ] = renderTarget;
			
			// Set sizing factor.
			m_renderTargetSizingFactors[ name ] = sizingFactor;

			// Raise RenderTargetRecreated event.
			if( bRenderTargetRecreated )
			{
				if( RenderTargetRecreated != null )
				{
					RenderTargetRecreated( name );
				}
			}

			return renderTarget;
		}

		public RenderTarget2D LoadRenderTarget( string name )
		{
			RenderTarget2D renderTarget;
			if( m_renderTargets.TryGetValue( name, out renderTarget ) )
			{
				return renderTarget;
			}

			return null;
		}

		public void SetToDefaultRenderTarget()
		{
			m_graphicsDevice.SetRenderTarget( null );
		}

		public void SetRenderTarget( string renderTargetName )
		{
			RenderTarget2D renderTarget = LoadRenderTarget( renderTargetName );
			if( renderTarget == null )
			{
				throw new InvalidOperationException( string.Format( "Cannot find a render-target with name of {0}.", renderTargetName ) );
			}

			// Recreate the render-target if necessary.
			if( !IsRenderTargetValid( renderTarget ) )
			{
				renderTarget = RecreateRenderTarget( renderTarget );
			}

			m_graphicsDevice.SetRenderTarget( renderTarget );
		}

		public void SetRenderTargets( params string[] renderTargetNames )
		{
			if( renderTargetNames.Length == 0 )
			{
				throw new InvalidOperationException( "The number of render-targets cannot be zero." );
			}
			else if( renderTargetNames.Length > MaximumRenderTargetCount )
			{
				throw new InvalidOperationException( "The number of render-targets cannot be more than " + MaximumRenderTargetCount + "." );
			}

			// Count valid render-target names.
			int renderTargetCount = renderTargetNames.Length;
			for( int i = 0 ; i < renderTargetNames.Length ; ++i )
			{
				if( string.IsNullOrWhiteSpace( renderTargetNames[ i ] ) )
				{
					renderTargetCount = i;
					break;
				}
			}

			// Get the number of render-target names.
			RenderTargetBinding[] bindings = new RenderTargetBinding[ renderTargetCount ];
			for( int i = 0 ; i < renderTargetCount ; ++i )
			{
				RenderTarget2D renderTarget = LoadRenderTarget( renderTargetNames[ i ] );
				if( renderTarget == null )
				{
					throw new InvalidOperationException( string.Format( "Cannot find a render-target with name of {0}.", renderTargetNames[ i ] ) );
				}

				// Recreate the render-target if necessary.
				if( !IsRenderTargetValid( renderTarget ) )
				{
					renderTarget = RecreateRenderTarget( renderTarget );
				}

				bindings[ i ] = new RenderTargetBinding( renderTarget );
			}

			m_graphicsDevice.SetRenderTargets( bindings );
		}

		public void GetCurrentRenderTargetDimension( out int width, out int height )
		{
			RenderTargetBinding[] renderTargetBindings = m_graphicsDevice.GetRenderTargets();
			if( ( renderTargetBindings != null )
				&& ( renderTargetBindings.Length > 0 ) )
			{
				// Get the first render-target and get its size.
				RenderTarget2D renderTarget0 = renderTargetBindings[ 0 ].RenderTarget as RenderTarget2D;
				if( renderTarget0 == null )
				{
					throw new InvalidOperationException( "The first render-target is null." );
				}
				width = renderTarget0.Width;
				height = renderTarget0.Height;
			}
			else
			{
				// Get the output render-target dimension.
				width = OutputRenderTargetWidth;
				height = OutputRenderTargetHeight;
			}
		}

		/// <summary>
		/// Reads the render-target data.
		/// Type parameter T must have the same bit depth as the format of the render-target.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="renderTargetName"></param>
		/// <returns></returns>
		public T[] ReadRenderTargetData<T>( string renderTargetName ) where T : struct
		{
			RenderTarget2D renderTarget = LoadRenderTarget( renderTargetName );
			if( renderTarget != null )
			{
				T[] data = new T[ renderTarget.Width * renderTarget.Height ];
				renderTarget.GetData<T>( data );

				return data;
			}

			return null;
		}

		/// <summary>
		/// Reads the render-target data and convert them to an array of Color.
		/// </summary>
		/// <param name="renderTargetName"></param>
		/// <returns></returns>
		public Color[] ReadRenderTarget( string renderTargetName )
		{
			RenderTarget2D renderTarget = LoadRenderTarget( renderTargetName );
			if( renderTarget != null )
			{
				switch( renderTarget.Format )
				{
					case SurfaceFormat.Single:
						{
							float[] data = new float[ renderTarget.Width * renderTarget.Height ];
							renderTarget.GetData<float>( data );

							Color[] result = new Color[ renderTarget.Width * renderTarget.Width ];
							for( int i = 0 ; i < data.Length ; ++i )
							{
								result[ i ] = new Color( data[ i ], 0, 0, 0 );
							}
							return result;
						}

					case SurfaceFormat.Color:
						{
							Color[] data = new Color[ renderTarget.Width * renderTarget.Height ];
							renderTarget.GetData<Color>( data );
							return data;
						}

					case SurfaceFormat.HdrBlendable:
					case SurfaceFormat.HalfVector4:
						{
							HalfVector4[] data = new HalfVector4[ renderTarget.Width * renderTarget.Height ];
							renderTarget.GetData<HalfVector4>( data );

							Color[] result = new Color[ renderTarget.Width * renderTarget.Width ];
							for( int i = 0 ; i < data.Length ; ++i )
							{
								Vector4 converted = data[ i ].ToVector4();
								result[ i ] = new Color( converted.X, converted.Y, converted.Z, converted.W );
							}
							return result;
						}
				}
			}

			return null;
		}

		// Test the render-target is still valid or not.
		private bool IsRenderTargetValid( RenderTarget2D renderTarget )
		{
			// If the content is lost, it's no more valid.
			if( renderTarget.IsContentLost )
			{
				return false;
			}

			// Check if the viewport dimension has changed if the render-target has a relative sizing factor.
			float sizingFactor = m_renderTargetSizingFactors[ renderTarget.Name ];
			if( sizingFactor != 0.0f )
			{
				int validWidth, validHeight;
				ComputeRelativeRenderTargetSize( sizingFactor, out validWidth, out validHeight );

				if( ( validWidth != renderTarget.Width )
					|| ( validHeight != renderTarget.Height ) )
				{
					return false;
				}
			}

			return true;
		}

		// Compute the size of the render-target using a relative sizing factor.
		private void ComputeRelativeRenderTargetSize( float sizingFactor, out int width, out int height )
		{
			width = ( int )( System.Math.Floor( ( float )OutputRenderTargetWidth * sizingFactor ) );
			height = ( int )( System.Math.Floor( ( float )OutputRenderTargetHeight * sizingFactor ) );
		}

		private RenderTarget2D RecreateRenderTarget( RenderTarget2D previousRenderTarget )
		{
			RenderTarget2D newRenderTarget = null;

			float sizingFactor = m_renderTargetSizingFactors[ previousRenderTarget.Name ];
			if( sizingFactor != 0.0f )
			{
				Debug.WriteLine( "Info: Recreating render-target [{0}] with the sizing factor of [{1}].", previousRenderTarget.Name, sizingFactor );

				newRenderTarget = CreateRenderTarget(
					previousRenderTarget.Name,
					sizingFactor,
					previousRenderTarget.LevelCount > 1,
					previousRenderTarget.Format,
					previousRenderTarget.DepthStencilFormat );
			}
			else
			{
				Debug.WriteLine(
					"Info: Recreating render-target [{0}] with the same dimension of [{1}x{2}].",
					previousRenderTarget.Name,
					previousRenderTarget.Width,
					previousRenderTarget.Height );

				// Recreate render-target with the same dimension.
				newRenderTarget = CreateRenderTarget(
					previousRenderTarget.Name,
					previousRenderTarget.Width,
					previousRenderTarget.Height,
					previousRenderTarget.LevelCount > 1,
					previousRenderTarget.Format,
					previousRenderTarget.DepthStencilFormat );
			}

			// Dispose the lost render-target.
			if( !previousRenderTarget.IsDisposed )
			{
				previousRenderTarget.Dispose();
			}

			return newRenderTarget;
		}
	}
}