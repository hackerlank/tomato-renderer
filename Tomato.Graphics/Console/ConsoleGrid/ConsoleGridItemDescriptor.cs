using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridItemDescriptor : PropertyDescriptor
	{
		private ConsoleGridItem m_item;

		private string m_category;
		private string m_description;

		private ConsoleGrid m_propertyGrid;

		public ConsoleVariableValueType ValueType { get { return m_item.ValueType; } }

		public object Value
		{
			get { return m_item.Value; }
			set { m_item.Value = value; }
		}

		public ConsoleGrid PropertyGrid
		{
			get { return m_propertyGrid; }
		}

		public override TypeConverter Converter
		{
			get
			{
				switch( m_item.ValueType )
				{
					case ConsoleVariableValueType.Int: return new ConsoleGridIntConverter();
					case ConsoleVariableValueType.IntArray: return new ConsoleGridIntArrayConverter();
					case ConsoleVariableValueType.UInt: return new ConsoleGridUIntConverter();
					case ConsoleVariableValueType.UIntArray: return new ConsoleGridUIntArrayConverter();
					case ConsoleVariableValueType.Float: return new ConsoleGridFloatConverter();
					case ConsoleVariableValueType.FloatArray: return new ConsoleGridFloatArrayConverter();
					case ConsoleVariableValueType.Double: return new ConsoleGridDoubleConverter();
					case ConsoleVariableValueType.DoubleArray: return new ConsoleGridDoubleArrayConverter();
					case ConsoleVariableValueType.Bool: return new ConsoleGridBoolConverter();
					case ConsoleVariableValueType.BoolArray: return new ConsoleGridBoolArrayConverter();
					case ConsoleVariableValueType.Vector2: return new ConsoleGridVector2Converter();
					case ConsoleVariableValueType.Vector3: return new ConsoleGridVector3Converter();
					case ConsoleVariableValueType.Vector4: return new ConsoleGridVector4Converter();
					case ConsoleVariableValueType.Matrix: return new ConsoleGridMatrixConverter();
					case ConsoleVariableValueType.String: return new ConsoleGridStringConverter();
				}

				return base.Converter;
			}
		}

		public override string Description
		{
			get
			{
				return m_description;
			}
		}

		public override string Category
		{
			get
			{
				return m_category;
			}
		}

		public override string DisplayName
		{
			get
			{
				return m_item.Name;
			}
		}

		public override bool SupportsChangeEvents
		{
			get
			{
				return base.SupportsChangeEvents;
			}
		}

		public ConsoleGridItemDescriptor( ConsoleGridItem item, Attribute[] attributes, ConsoleGrid propertyGrid )
			: base( item.Name, attributes )
		{
			m_item = item;

			m_category = item.Category;

			m_propertyGrid = propertyGrid;

			m_description = Enum.GetName( typeof( ConsoleVariableValueType ), m_item.ValueType );
		}

		public void SetCategory( string category )
		{
			m_category = category;
		}

		public override bool CanResetValue( object component )
		{
			return false;
		}

		public override Type ComponentType
		{
			get { return null; }
		}

		public override object GetValue( object component )
		{
			return m_item.Value;
		}

		public override bool IsReadOnly
		{
			get { return ( Attributes.Matches( ReadOnlyAttribute.Yes ) ); }
		}

		public override Type PropertyType
		{
			get { return m_item.Value.GetType(); }
		}

		public override void ResetValue( object component )
		{
			return;
		}

		public override void SetValue( object component, object value )
		{
			m_item.Value = value;
		}

		public override bool ShouldSerializeValue( object component )
		{
			return false;
		}
	}
}
