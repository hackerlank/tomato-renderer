using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	/// <summary>
	/// Represents transformation.
	/// A single transformation consists of scaling/rotation/translation (SRT) components.
	/// </summary>
	public struct Transformation : IEquatable<Transformation>
	{
		// Translation vector.
		private Vector3 m_translation;

		// Scaling
		private float m_scaling;

		// rotation quaternion
		private Quaternion m_rotation;

		// Transformation matrix
		private Matrix m_transformationMatrix;

		// identity transformation
		private static Transformation s_identity;

		/// <summary>
		/// Gets or sets scaling value.
		/// </summary>
		public float Scaling
		{
			get { return m_scaling; }
			set
			{
				// Validate value.
				ValidateScalingValue( ref value );

				if( m_scaling != value )
				{
					m_scaling = value;

					// Rebuild transformation matrix.
					GetAsMatrix( out m_transformationMatrix );
				}
			}
		}

		/// <summary>
		/// Gets or sets translation value.
		/// </summary>
		public Vector3 Translation
		{
			get { return m_translation; }
			set
			{
				if( m_translation != value )
				{
					m_translation = value;

					// Rebuild transformation matrix.
					GetAsMatrix( out m_transformationMatrix );
				}
			}
		}

		/// <summary>
		/// Gets or sets rotation value.
		/// </summary>
		public Quaternion Rotation
		{
			get { return m_rotation; }
			set
			{
				// Validate value.
				ValidateRotationValue( ref value );

				if( m_rotation != value )
				{
					m_rotation = value;

					// Rebuild transformation matrix.
					GetAsMatrix( out m_transformationMatrix );
				}
			}
		}

		public Matrix AsMatrix
		{
			get { return m_transformationMatrix; }
		}

		/// <summary>
		/// Gets identity transformation.
		/// Translation = ( 0, 0, 0 )
		/// Scaling = 1
		/// Rotation = Quaternion.Identity
		/// </summary>
		public static Transformation Identity { get { return s_identity; } }

		static Transformation()
		{
			s_identity = new Transformation(
				1.0f,
				Quaternion.Identity,
				Vector3.Zero );
		}

		public Transformation(
			float scaling,
			Quaternion rotation,
			Vector3 translation )
		{
			// Validate values.
			ValidateScalingValue( ref scaling );
			ValidateRotationValue( ref rotation );

			// Set memebers
			m_translation = translation;
			m_scaling = scaling;
			m_rotation = rotation;

			// Transformation matrix
			GetAsMatrix( m_scaling, ref m_rotation, ref m_translation, out m_transformationMatrix );
		}

		/// <summary>
		/// Constructs a transformation from a matrix.
		/// </summary>
		/// <param name="matrix"></param>
		public Transformation( Matrix matrix )
			: this( ref matrix )
		{
		}

		/// <summary>
		/// Constructs a transformation from a matrix.
		/// </summary>
		/// <param name="matrix"></param>
		public Transformation( ref Matrix matrix )
		{
			Vector3 scaling;
			if( !matrix.Decompose( out scaling, out m_rotation, out m_translation ) )
			{
				throw new InvalidCastException( "Failed to create transformation from matrix." );
			}

			// Make sure that scaling is uniform.
#if false
			if( !Tomato.Math.MathHelper.CompareFloat( scaling.X, scaling.Y )
				|| !Tomato.Math.MathHelper.CompareFloat( scaling.X, scaling.Z ) )
			{
				throw new InvalidOperationException( "Transformation class does not support non-uniform scaling." );
			}
#endif
			m_scaling = scaling.X;

			// Transformation matrix
			m_transformationMatrix = matrix;
		}

		/// <summary>
		/// Transforms a vector point.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3 Transform( Vector3 p )
		{
			Vector3 result;
			Vector3.Transform( ref p, ref m_transformationMatrix, out result );
			return result;
		}

		/// <summary>
		/// Transforms a vector point.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="result"></param>
		public void Transform( ref Vector3 p, out Vector3 result )
		{
			Vector3.Transform( ref p, ref m_transformationMatrix, out result );
		}

		/// <summary>
		/// Transforms a vector.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector4 Transform( Vector4 p )
		{
			Vector4 result;
			Vector4.Transform( ref p, ref m_transformationMatrix, out result );
			return result;
		}

		/// <summary>
		/// Transforms a vector.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="result"></param>
		public void Transform( ref Vector4 p, out Vector4 result )
		{
			Vector4.Transform( ref p, ref m_transformationMatrix, out result );
		}

		/// <summary>
		/// Transforms a vector normal.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3 TransformNormal( Vector3 p )
		{
			Vector3 result;
			Vector3.TransformNormal( ref p, ref m_transformationMatrix, out result );
			return result;
		}

		/// <summary>
		/// Transforms a vector normal.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="result"></param>
		public void TransformNormal( ref Vector3 p, out Vector3 result )
		{
			Vector3.TransformNormal( ref p, ref m_transformationMatrix, out result );
		}

		/// <summary>
		/// Multiplies a transformations by another transformation.
		/// </summary>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <returns></returns>
		public static Transformation operator *( Transformation t1, Transformation t2 )
		{
			Matrix result;
			Matrix.Multiply( ref t1.m_transformationMatrix, ref t2.m_transformationMatrix, out result );

			return new Transformation( ref result );
		}

		private void GetAsMatrix( out Matrix matrix )
		{
			GetAsMatrix( m_scaling, ref m_rotation, ref m_translation, out matrix );
		}

		private static void GetAsMatrix( float scaling, ref Quaternion rotation, ref Vector3 translation, out Matrix matrix )
		{
			Matrix scalingMatrix;
			Matrix.CreateScale( scaling, out scalingMatrix );

			Matrix rotationMatrix;
			Matrix.CreateFromQuaternion( ref rotation, out rotationMatrix );

			Matrix translationMatrix;
			Matrix.CreateTranslation( ref translation, out translationMatrix );

			// Compose matrix in Scaling * Rotation * Translation order.
			Matrix.Multiply( ref scalingMatrix, ref rotationMatrix, out matrix );
			Matrix.Multiply( ref matrix, ref translationMatrix, out matrix );
		}

		private static void ValidateScalingValue( ref float scaling )
		{
			if( scaling < 0.0f )
			{
#if DEBUG
				throw new ArgumentException( "Scaling value must be zero or positive number." );
#else
				scaling = 0.0f;
#endif
			}
		}

		private static void ValidateRotationValue( ref Quaternion rotation )
		{
			if( !MathHelper.CompareFloat( rotation.LengthSquared(), 1.0f ) )
			{
#if DEBUG
				throw new ArgumentException( "Rotation value must be a unit quaternion." );
#else
				rotation = Quaternion.Identity;
#endif
			}
		}

		public bool Equals( Transformation other )
		{
			return ( m_scaling == other.m_scaling )
				&& ( m_rotation == other.m_rotation )
				&& ( m_translation == other.m_translation );
		}

		public override bool Equals( object obj )
		{
			if( obj is Transformation )
			{
				return Equals( ( Transformation )obj );
			}

			return false;
		}

		public override int GetHashCode()
		{
			return m_transformationMatrix.GetHashCode();
		}

		public void WriteToStream( Stream stream )
		{
			WriteToStream( new BinaryWriter( stream ) );
		}

		public void WriteToStream( BinaryWriter writer )
		{
			writer.Write( m_scaling );

			writer.Write( m_rotation.X );
			writer.Write( m_rotation.Y );
			writer.Write( m_rotation.Z );
			writer.Write( m_rotation.W );

			writer.Write( m_translation.X );
			writer.Write( m_translation.Y );
			writer.Write( m_translation.Z );
		}

		public static Transformation ReadFromStream( Stream stream )
		{
			return ReadFromStream( new BinaryReader( stream ) );
		}

		public static Transformation ReadFromStream( BinaryReader reader )
		{
			float scaling = reader.ReadSingle();

			Quaternion rotation = Quaternion.Identity;
			rotation.X = reader.ReadSingle();
			rotation.Y = reader.ReadSingle();
			rotation.Z = reader.ReadSingle();
			rotation.W = reader.ReadSingle();

			Vector3 translation = Vector3.Zero;
			translation.X = reader.ReadSingle();
			translation.Y = reader.ReadSingle();
			translation.Z = reader.ReadSingle();

			return new Transformation( scaling, rotation, translation );
		}

		public static void ReadFromStream( Stream stream, out Transformation transformation )
		{
			ReadFromStream( new BinaryReader( stream ), out transformation );
		}

		public static void ReadFromStream( BinaryReader reader, out Transformation transformation )
		{
			float scaling = reader.ReadSingle();

			Quaternion rotation = Quaternion.Identity;
			rotation.X = reader.ReadSingle();
			rotation.Y = reader.ReadSingle();
			rotation.Z = reader.ReadSingle();
			rotation.W = reader.ReadSingle();

			Vector3 translation = Vector3.Zero;
			translation.X = reader.ReadSingle();
			translation.Y = reader.ReadSingle();
			translation.Z = reader.ReadSingle();

			transformation = new Transformation( scaling, rotation, translation );
		}
	}
}
