using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridItemCollection : List<ConsoleGridItem>, ICustomTypeDescriptor
	{
		private ConsoleGrid m_consoleGrid = null;

		public ConsoleGridItemCollection( ConsoleGrid consoleGrid )
		{
			m_consoleGrid = consoleGrid;
		}

		public void AddFromStream( MessageStream stream )
		{
			ConsoleVariable consoleVariable = ConsoleVariable.ReadFromStream( stream );
			Add( consoleVariable );
		}

		public void Add( ConsoleVariable consoleVariable )
		{
			// Parse the category name and item name.
			string[] nameTokens = consoleVariable.Name.Split( '|' );

			// Remove variables with the same signature.
			if( nameTokens.Length == 1 )
			{
				Remove( null, nameTokens[ 0 ] );
			}
			else if( nameTokens.Length == 2 )
			{
				Remove( nameTokens[ 0 ], nameTokens[ 1 ] );
			}
			else
			{
				throw new FormatException( consoleVariable.Name );
			}

			// Add.
			Add( new ConsoleGridItem( consoleVariable ) );
		}

		public void Remove( string category, string name )
		{
			RemoveAll( i => { return ( i.Category == category ) && ( i.Name == name ); } );
		}

		public ConsoleGridItem Find( string category, string name )
		{
			return ( from i in this
					 where ( i.Category == category ) && ( i.Name == name )
					 select i ).FirstOrDefault();
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes( this, true );
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName( this, true );
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName( this, true );
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter( this, true );
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent( this, true );
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty( this, true );
		}

		public object GetEditor( Type editorBaseType )
		{
			return TypeDescriptor.GetEditor( this, editorBaseType, true );
		}

		public EventDescriptorCollection GetEvents( Attribute[] attributes )
		{
			return TypeDescriptor.GetEvents( this, attributes, true );
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents( this, true );
		}

		public PropertyDescriptorCollection GetProperties( Attribute[] attributes )
		{
			PropertyDescriptor[] propertyDescriptors = new PropertyDescriptor[ Count ];
			for( int i = 0 ; i < Count ; i++ )
			{
				ConsoleGridItem item = this[ i ];

				List<Attribute> newAttributes = new List<Attribute>();
				newAttributes.AddRange( attributes );

				// For numerics types, add a FloatValueEditor attribute.
				if( ( item.ValueType == ConsoleVariableValueType.Float )
					|| ( item.ValueType == ConsoleVariableValueType.Double )
					|| ( item.ValueType == ConsoleVariableValueType.UInt )
					|| ( item.ValueType == ConsoleVariableValueType.Int ) )
				{
					newAttributes.Add( new EditorAttribute( typeof( ConsoleGridFloatValueEditor ), typeof( System.Drawing.Design.UITypeEditor ) ) );
				}

				propertyDescriptors[ i ] = new ConsoleGridItemDescriptor( item, newAttributes.ToArray(), m_consoleGrid );
			}

			return new PropertyDescriptorCollection( propertyDescriptors );
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return TypeDescriptor.GetProperties( this, true );
		}

		public object GetPropertyOwner( PropertyDescriptor propertyDescriptor )
		{
			return this;
		}
	}
}
