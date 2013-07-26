using System;
using System.Windows.Forms;

namespace SampleWinFormsApplication
{
	public class TransparentFlowLayoutPanel : FlowLayoutPanel
	{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 0x20;
				return createParams;
			}
		}

		public TransparentFlowLayoutPanel()
		{
			this.SetStyle( ControlStyles.Opaque, true );
		}
	}
}