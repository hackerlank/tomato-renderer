using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security.Permissions;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	[PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
	internal class ConsoleGridFloatValueEditor : UITypeEditor
	{
		public ConsoleGridFloatValueEditor()
		{
		}

		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			// acceptable types
			if( value.GetType() != typeof( double )
				&& value.GetType() != typeof( float )
				&& value.GetType() != typeof( int )
				&& value.GetType() != typeof( uint ) )
				return value;

			IWindowsFormsEditorService editorService = ( IWindowsFormsEditorService )provider.GetService( typeof( IWindowsFormsEditorService ) );
			ConsoleGridItemDescriptor desc = context.PropertyDescriptor as ConsoleGridItemDescriptor;
			if( editorService != null && desc != null )
			{
				ConsoleGridItem item = desc.PropertyGrid.SelectedItem;

				float minValue = -100.0f;
				float maxValue = 100.0f;
				float stepValue = -1.0f;

				if( item.MinimumValue != null && item.MaximumValue != null )
				{
					if( item.ValueType == ConsoleVariableValueType.Vector2 )
					{
						if( desc.DisplayName.EndsWith( "X" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.MinimumValue ) ).X.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.MaximumValue ) ).X.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.SteppingValue ) ).X.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "Y" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.MinimumValue ) ).Y.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.MaximumValue ) ).Y.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector2 )( item.SteppingValue ) ).Y.Value );
							}
						}
					}
					else if( item.ValueType == ConsoleVariableValueType.Vector3 )
					{
						if( desc.DisplayName.EndsWith( "X" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MinimumValue ) ).X.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MaximumValue ) ).X.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.SteppingValue ) ).X.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "Y" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MinimumValue ) ).Y.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MaximumValue ) ).Y.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.SteppingValue ) ).Y.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "Z" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MinimumValue ) ).Z.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.MaximumValue ) ).Z.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector3 )( item.SteppingValue ) ).Z.Value );
							}
						}
					}
					else if( item.ValueType == ConsoleVariableValueType.Vector4 )
					{
						if( desc.DisplayName.EndsWith( "X" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MinimumValue ) ).X.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MaximumValue ) ).X.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.SteppingValue ) ).X.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "Y" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MinimumValue ) ).Y.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MaximumValue ) ).Y.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.SteppingValue ) ).Y.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "Z" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MinimumValue ) ).Z.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MaximumValue ) ).Z.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.SteppingValue ) ).Y.Value );
							}
						}
						else if( desc.DisplayName.EndsWith( "W" ) )
						{
							minValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MinimumValue ) ).W.Value );
							maxValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.MaximumValue ) ).W.Value );
							if( item.SteppingValue != null )
							{
								stepValue = Convert.ToSingle( ( ( ConsoleGridVector4 )( item.SteppingValue ) ).W.Value );
							}
						}
					}
					else
					{
						minValue = Convert.ToSingle( item.MinimumValue );
						maxValue = Convert.ToSingle( item.MaximumValue );
						if( item.SteppingValue != null )
						{
							stepValue = Convert.ToSingle( item.SteppingValue );
						}
					}

					ConsoleGridFloatValueEditorForm form = new ConsoleGridFloatValueEditorForm(
						desc.PropertyGrid,
						Convert.ToSingle( value ), minValue, maxValue, stepValue,
						( item.ValueType == ConsoleVariableValueType.Int || item.ValueType == ConsoleVariableValueType.UInt ) );
					form.Show();
				}

				return value;
			}

			return value;
		}

		public override bool GetPaintValueSupported( ITypeDescriptorContext context )
		{
			return false;
		}
	}


}

