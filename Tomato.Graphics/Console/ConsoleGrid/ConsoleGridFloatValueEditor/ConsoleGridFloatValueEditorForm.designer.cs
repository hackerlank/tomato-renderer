namespace Tomato.Graphics.Console.ConsoleGrid
{
	partial class ConsoleGridFloatValueEditorForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			( ( System.ComponentModel.ISupportInitialize )( this.trackBar1 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// trackBar1
			// 
			this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackBar1.LargeChange = 10;
			this.trackBar1.Location = new System.Drawing.Point( 0, 0 );
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size( 181, 24 );
			this.trackBar1.TabIndex = 1;
			this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBar1.ValueChanged += new System.EventHandler( this.hScrollBar1_ValueChanged );
			// 
			// ConsoleGridFloatValueEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 181, 24 );
			this.Controls.Add( this.trackBar1 );
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConsoleGridFloatValueEditorForm";
			this.Opacity = 0.9D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Deactivate += new System.EventHandler( this.FloatValueEditorForm_Deactivate );
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.FloatValueEditorForm_KeyDown );
			( ( System.ComponentModel.ISupportInitialize )( this.trackBar1 ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		

		private System.Windows.Forms.TrackBar trackBar1;

	}
}