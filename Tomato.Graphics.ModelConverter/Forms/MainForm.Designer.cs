namespace Tomato.Graphics.ModelConverter
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 84, 12 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Source Model";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point( 14, 24 );
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size( 320, 21 );
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 340, 22 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 35, 23 );
			this.button1.TabIndex = 2;
			this.button1.Text = "...";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point( 340, 61 );
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size( 35, 23 );
			this.button2.TabIndex = 5;
			this.button2.Text = "...";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler( this.button2_Click );
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point( 14, 63 );
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size( 320, 21 );
			this.textBox2.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 12, 48 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 106, 12 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Destination Model";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point( 284, 90 );
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size( 91, 24 );
			this.button3.TabIndex = 6;
			this.button3.Text = "Convert";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler( this.button3_Click );
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "OBJ Files(*.obj)|*.obj";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "Tomato Renderer Model Files(*.trm)|*.trm";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 387, 126 );
			this.Controls.Add( this.button3 );
			this.Controls.Add( this.button2 );
			this.Controls.Add( this.textBox2 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.textBox1 );
			this.Controls.Add( this.label1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "ModelConverter";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}

