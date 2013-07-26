using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	public class ConsoleGrid : UserControl
	{
		private PropertyGrid propertyGrid1 = null;
		private ConsoleVariableCollection m_consoleVariableCollection = null;

		internal ConsoleGridItemCollection Items { get; private set; }

		[Browsable( false )]
		public ConsoleVariableCollection ConsoleVariables 
		{
			get { return m_consoleVariableCollection; }
			set
			{
				if( m_consoleVariableCollection != value )
				{
					// Unbind the previous collection
					if( m_consoleVariableCollection != null )
					{
						m_consoleVariableCollection.ItemAdded -= new Action<ConsoleVariable>( OnConsoleVariablesItemAdded );
						m_consoleVariableCollection.ItemRemoved -= new Action<string>( OnConsoleVariablesItemRemoved );
						m_consoleVariableCollection.ItemsCleared -= new Action( OnConsoleVariablesItemsCleared );
					}

					// Set value.
					m_consoleVariableCollection = value;
					m_consoleVariableCollection.ItemAdded += new Action<ConsoleVariable>( OnConsoleVariablesItemAdded );
					m_consoleVariableCollection.ItemRemoved += new Action<string>( OnConsoleVariablesItemRemoved );
					m_consoleVariableCollection.ItemsCleared += new Action( OnConsoleVariablesItemsCleared );

					// Refill the console grid item collection.
					Items.Clear();
					if( m_consoleVariableCollection != null )
					{
						Items.AddRange( from c in m_consoleVariableCollection select new ConsoleGridItem( c ) );
					}

					// Rebind to the property grid control.
					propertyGrid1.SelectedObject = Items;
				}
			}
		}

		/// <summary>
		/// Raised when the value of a console grid's item has changed.
		/// </summary>
		public event Action<ConsoleGridItem> ItemValueChanged = null;

		/// <summary>
		/// Gets the currenly selected console grid item.
		/// </summary>
		[Browsable( false )]
		public ConsoleGridItem SelectedItem
		{
			get
			{
				if( ( propertyGrid1 != null )
					&& ( propertyGrid1.SelectedGridItem != null ) )
				{
					return GetConsoleGridItem( propertyGrid1.SelectedGridItem );
				}

				return null;
			}
		}

		public ConsoleGrid()
		{
			InitializeComponent();

			Items = new ConsoleGridItemCollection( this );
			ConsoleVariables = new ConsoleVariableCollection();
			propertyGrid1.SelectedObject = Items;
		}

		private void InitializeComponent()
		{
			propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			SuspendLayout();
			
			// propertyGrid1			
			propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			propertyGrid1.Location = new System.Drawing.Point( 0, 0 );
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.Size = new System.Drawing.Size( 207, 217 );
			propertyGrid1.TabIndex = 0;
			propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler( OnPropertyValueChanged );
			
			// ConsoleGrid
			Controls.Add( this.propertyGrid1 );
			Name = "ConsoleGrid";
			Size = new System.Drawing.Size( 207, 217 );
			ResumeLayout( false );
		}		

		internal void ChangeSelectedItemValue( object value )
		{
			ConsoleGridItemDescriptor desc = propertyGrid1.SelectedGridItem.PropertyDescriptor as ConsoleGridItemDescriptor;
			if( desc != null )
			{
				desc.Value = value;
				propertyGrid1.SelectedObject = Items;

				ConsoleGridItem consoleGridItem = GetConsoleGridItem( propertyGrid1.SelectedGridItem );
				if( consoleGridItem != null )
				{
					// Invoke ConsoleGridValueChanged event.
					if( ItemValueChanged != null )
					{
						ItemValueChanged( consoleGridItem );
					}
				}
			}
		}

		private void OnPropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			ConsoleGridItem consoleGridItem = GetConsoleGridItem( e.ChangedItem );
			if( consoleGridItem != null )
			{
				// Find the corresponding console variable and update its value.
				if( m_consoleVariableCollection != null )
				{
					string consoleVariableName;
					if( consoleGridItem.Category != null )
					{
						consoleVariableName = string.Format( "{0}|{1}", consoleGridItem.Category, consoleGridItem.Name );
					}
					else
					{
						consoleVariableName = consoleGridItem.Name;
					}

					var consoleVariable = ( from c in m_consoleVariableCollection
											where c.Name == consoleVariableName
											select c ).FirstOrDefault();
					if( consoleVariable != null )
					{
						// Convert to console variable value.
						consoleVariable.Value = ConsoleGridItem.GetConsoleVariableValue( consoleGridItem );
					}
				}

				// Invoke ConsoleGridValueChanged event.
				if( ItemValueChanged != null )
				{
					ItemValueChanged( consoleGridItem );
				}
			}
		}

		private ConsoleGridItem GetConsoleGridItem( GridItem gridItem )
		{
			// Find the top-most grid item.
			while( gridItem.Parent.GridItemType != GridItemType.Category )
			{
				gridItem = gridItem.Parent;
			}

			// Find the corresponding console grid item.
			return Items.Find( gridItem.PropertyDescriptor.Category, gridItem.PropertyDescriptor.Name );
		}

		private void OnConsoleVariablesItemAdded( ConsoleVariable consoleVariable )
		{
			Items.Add( new ConsoleGridItem( consoleVariable ) );
			propertyGrid1.SelectedObject = Items;
		}

		private void OnConsoleVariablesItemRemoved( string consoleVariableName )
		{
			string[] nameTokens = consoleVariableName.Split( '|' );

			// Remove variables with the same signature.
			if( nameTokens.Length == 1 )
			{
				Items.Remove( null, nameTokens[ 0 ] );
			}
			else if( nameTokens.Length == 2 )
			{
				Items.Remove( nameTokens[ 0 ], nameTokens[ 1 ] );
			}
			else
			{
				throw new FormatException( consoleVariableName );
			}

			propertyGrid1.SelectedObject = Items;
		}

		private void OnConsoleVariablesItemsCleared()
		{
			Items.Clear();
			propertyGrid1.SelectedObject = Items;
		}		
	}
}
