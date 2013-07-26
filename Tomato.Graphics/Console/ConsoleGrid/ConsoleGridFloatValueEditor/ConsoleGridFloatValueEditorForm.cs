using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal partial class ConsoleGridFloatValueEditorForm : Form
	{
		private ConsoleGrid m_propertyGrid;

		private const int m_controlMin = 0;
		private const int m_controlMax = 100;
		private const int m_controlFreq = 1;

		private float m_value;
		private float m_minValue;
		private float m_maxValue;
		private float m_stepValue;
		private bool m_isIntegerType;

		public ConsoleGridFloatValueEditorForm( ConsoleGrid propertyGrid, float value, float minValue, float maxValue, float stepValue, bool isIntegerType )
		{
			InitializeComponent();

			m_propertyGrid = propertyGrid;
			m_isIntegerType = isIntegerType;

			m_minValue = minValue;
			m_maxValue = maxValue;
			m_value = value;
			m_stepValue = stepValue;

			if( m_stepValue <= float.Epsilon )
			{
				m_stepValue = ( m_maxValue - m_minValue ) * 0.01f;
			}
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			Location = new Point(
				Cursor.Position.X + 7,
				Cursor.Position.Y - 25 );

			trackBar1.Minimum = m_controlMin;
			trackBar1.Maximum = m_controlMax;
			trackBar1.TickFrequency = m_controlFreq;
			trackBar1.SmallChange = m_controlFreq;
			trackBar1.LargeChange = m_controlFreq * 2;

			trackBar1.Value = ( int )(
				( float )( trackBar1.Maximum ) *
				( m_value - m_minValue ) /
				( m_maxValue - m_minValue ) );
		}

		protected override void OnLostFocus( EventArgs e )
		{
			base.OnLostFocus( e );

			Close();
		}

		private void hScrollBar1_ValueChanged( object sender, EventArgs e )
		{
			float valueStep = ( float )System.Math.Round( m_stepValue *
				( float )( m_controlMax - m_controlMin ) /
				( m_maxValue - m_minValue ) );
			trackBar1.Value = trackBar1.Minimum + ( int )(
				System.Math.Round( ( ( float )( trackBar1.Value - trackBar1.Minimum ) / valueStep ) ) *
				valueStep );

			m_value = m_minValue +
				( ( ( float )( trackBar1.Value ) * ( m_maxValue - m_minValue ) )
				/ ( float )( trackBar1.Maximum ) );

			if( m_isIntegerType )
			{
				m_value = ( float )System.Math.Round( m_value );
			}

			Text = "Value: " + m_value + " (Step: " + m_stepValue + ")";

			m_propertyGrid.ChangeSelectedItemValue( m_value );
		}

		private void FloatValueEditorForm_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				Close();
			}
		}

		private void FloatValueEditorForm_Deactivate( object sender, EventArgs e )
		{
			Close();
		}
	}
}