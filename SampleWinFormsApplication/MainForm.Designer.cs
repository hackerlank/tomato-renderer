namespace SampleWinFormsApplication
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
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.treeView2 = new System.Windows.Forms.TreeView();
			this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.xnaPanel1 = new Tomato.Graphics.WinForms.XnaPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.snapshotViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer2 ) ).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer3 ) ).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.tabPage3.SuspendLayout();
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer1 ) ).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point( 0, 0 );
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size( 201, 427 );
			this.propertyGrid1.TabIndex = 4;
			this.propertyGrid1.ToolbarVisible = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tabPage1 );
			this.tabControl1.Controls.Add( this.tabPage2 );
			this.tabControl1.Controls.Add( this.tabPage3 );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 460, 466 );
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add( this.splitContainer2 );
			this.tabPage1.Location = new System.Drawing.Point( 4, 23 );
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding( 6 );
			this.tabPage1.Size = new System.Drawing.Size( 452, 439 );
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Render Pass";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point( 6, 6 );
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add( this.treeView1 );
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add( this.propertyGrid1 );
			this.splitContainer2.Size = new System.Drawing.Size( 440, 427 );
			this.splitContainer2.SplitterDistance = 234;
			this.splitContainer2.SplitterWidth = 5;
			this.splitContainer2.TabIndex = 6;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Indent = 10;
			this.treeView1.Location = new System.Drawing.Point( 0, 0 );
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size( 234, 427 );
			this.treeView1.TabIndex = 5;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.treeView1_AfterSelect );
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add( this.splitContainer3 );
			this.tabPage2.Location = new System.Drawing.Point( 4, 23 );
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage2.Size = new System.Drawing.Size( 452, 439 );
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Scene Graph";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point( 3, 3 );
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add( this.treeView2 );
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add( this.propertyGrid2 );
			this.splitContainer3.Size = new System.Drawing.Size( 446, 433 );
			this.splitContainer3.SplitterDistance = 237;
			this.splitContainer3.SplitterWidth = 5;
			this.splitContainer3.TabIndex = 7;
			// 
			// treeView2
			// 
			this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView2.Indent = 10;
			this.treeView2.Location = new System.Drawing.Point( 0, 0 );
			this.treeView2.Name = "treeView2";
			this.treeView2.Size = new System.Drawing.Size( 237, 433 );
			this.treeView2.TabIndex = 5;
			this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.treeView2_AfterSelect );
			// 
			// propertyGrid2
			// 
			this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid2.Location = new System.Drawing.Point( 0, 0 );
			this.propertyGrid2.Name = "propertyGrid2";
			this.propertyGrid2.Size = new System.Drawing.Size( 204, 433 );
			this.propertyGrid2.TabIndex = 4;
			this.propertyGrid2.ToolbarVisible = false;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add( this.flowLayoutPanel1 );
			this.tabPage3.Controls.Add( this.button1 );
			this.tabPage3.Location = new System.Drawing.Point( 4, 23 );
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage3.Size = new System.Drawing.Size( 452, 439 );
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Snapshot";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Location = new System.Drawing.Point( 7, 40 );
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size( 437, 100 );
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 7, 7 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 86, 27 );
			this.button1.TabIndex = 0;
			this.button1.Text = "Shoot";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.xnaPanel1 );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.tabControl1 );
			this.splitContainer1.Size = new System.Drawing.Size( 985, 466 );
			this.splitContainer1.SplitterDistance = 520;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 7;
			// 
			// xnaPanel1
			// 
			this.xnaPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xnaPanel1.Location = new System.Drawing.Point( 0, 0 );
			this.xnaPanel1.Name = "xnaPanel1";
			this.xnaPanel1.Size = new System.Drawing.Size( 520, 466 );
			this.xnaPanel1.TabIndex = 0;
			this.xnaPanel1.Text = "xnaPanel1";
			this.xnaPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler( this.xnaPanel1_MouseDown );
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding( 7, 2, 0, 2 );
			this.menuStrip1.Size = new System.Drawing.Size( 985, 24 );
			this.menuStrip1.TabIndex = 8;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// FileToolStripMenuItem
			// 
			this.FileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem} );
			this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
			this.FileToolStripMenuItem.Size = new System.Drawing.Size( 37, 20 );
			this.FileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 93, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.snapshotViewerToolStripMenuItem} );
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size( 47, 20 );
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// snapshotViewerToolStripMenuItem
			// 
			this.snapshotViewerToolStripMenuItem.Name = "snapshotViewerToolStripMenuItem";
			this.snapshotViewerToolStripMenuItem.Size = new System.Drawing.Size( 164, 22 );
			this.snapshotViewerToolStripMenuItem.Text = "&Snapshot Viewer";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem} );
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size( 116, 22 );
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2} );
			this.statusStrip1.Location = new System.Drawing.Point( 0, 490 );
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding( 1, 0, 16, 0 );
			this.statusStrip1.Size = new System.Drawing.Size( 985, 22 );
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size( 99, 17 );
			this.toolStripStatusLabel1.Text = "Tomato Renderer";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size( 202, 17 );
			this.toolStripStatusLabel2.Text = "Windows Forms Sample Application";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 8F, 14F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 985, 512 );
			this.Controls.Add( this.splitContainer1 );
			this.Controls.Add( this.menuStrip1 );
			this.Controls.Add( this.statusStrip1 );
			this.Font = new System.Drawing.Font( "Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Tomato Renderer WinForms Sample";
			this.Load += new System.EventHandler( this.MainForm_Load );
			this.tabControl1.ResumeLayout( false );
			this.tabPage1.ResumeLayout( false );
			this.splitContainer2.Panel1.ResumeLayout( false );
			this.splitContainer2.Panel2.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer2 ) ).EndInit();
			this.splitContainer2.ResumeLayout( false );
			this.tabPage2.ResumeLayout( false );
			this.splitContainer3.Panel1.ResumeLayout( false );
			this.splitContainer3.Panel2.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer3 ) ).EndInit();
			this.splitContainer3.ResumeLayout( false );
			this.tabPage3.ResumeLayout( false );
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize )( this.splitContainer1 ) ).EndInit();
			this.splitContainer1.ResumeLayout( false );
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout( false );
			this.statusStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private Tomato.Graphics.WinForms.XnaPanel xnaPanel1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.TreeView treeView2;
		private System.Windows.Forms.PropertyGrid propertyGrid2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem snapshotViewerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

	}
}

