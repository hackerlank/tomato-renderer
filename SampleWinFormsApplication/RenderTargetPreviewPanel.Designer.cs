﻿namespace SampleWinFormsApplication
{
	partial class RenderTargetPreviewPanel
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			( ( System.ComponentModel.ISupportInitialize )( this.pictureBox1 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 3, 0 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 52, 14 );
			this.label1.TabIndex = 0;
			this.label1.Text = "(name)";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point( 3, 17 );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size( 227, 121 );
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseEnter += new System.EventHandler( this.pictureBox1_MouseEnter );
			this.pictureBox1.MouseLeave += new System.EventHandler( this.pictureBox1_MouseLeave );
			// 
			// RenderTargetPreviewPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 14F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.pictureBox1 );
			this.Font = new System.Drawing.Font( "Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.Name = "RenderTargetPreviewPanel";
			this.Size = new System.Drawing.Size( 233, 141 );
			( ( System.ComponentModel.ISupportInitialize )( this.pictureBox1 ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
