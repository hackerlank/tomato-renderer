using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Tomato.Graphics;

namespace SampleWinFormsApplication
{
	public partial class MainForm : Form
	{
		private const string SampleSceneFilePath = @".\Scene.trs";

		private LightPrePassRenderer m_renderer = null;

		private SceneDescription m_scene = null;
		private SceneObject m_sceneRoot = null;

		private Camera m_sceneCamera = null;
		private FlyCameraController m_cameraController = null;

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm() );
		}

		public MainForm()
		{
			InitializeComponent();

			xnaPanel1.Initializing += new Action( xnaPanel1_Initializing );
			xnaPanel1.Drawing += new Action( xnaPanel1_Drawing );
			xnaPanel1.Resize += new EventHandler( xnaPanel1_Resize );
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			UpdateRenderPassTree( treeView1, m_renderer );
			UpdateSceneGraphTree( treeView2, m_sceneRoot );
		}

		private void UpdateSceneGraphTree( TreeView treeView, SceneObject rootObject )
		{
			treeView.SuspendLayout();
			treeView.Nodes.Clear();

			TreeNode rootNode = CreateSceneGraphTreeNode( rootObject );
			treeView.Nodes.Add( rootNode );

			rootNode.Expand();
			treeView.ResumeLayout();
		}

		private TreeNode CreateSceneGraphTreeNode( SceneObject sceneObject )
		{
			TreeNode treeNode = new TreeNode();

			// Set the node name.
			treeNode.Text =
				string.Format( "[{0}] {1}",
				Enum.GetName( typeof( SceneObjectType ), sceneObject.ObjectType ),
				string.IsNullOrWhiteSpace( sceneObject.Name ) ? "" : sceneObject.Name );

			// Set the tag object.
			treeNode.Tag = sceneObject;

			// If it's a Node object, add children tree nodes.
			Node node = sceneObject as Node;
			if( node != null )
			{
				foreach( SceneObject child in node.Children )
				{
					treeNode.Nodes.Add( CreateSceneGraphTreeNode( child ) );
				}
			}
			else
			{
				// If it's a Mesh object, add sub-mesh tree nodes.
				Mesh mesh = sceneObject as Mesh;
				if( mesh != null )
				{
					foreach( SubMesh subMesh in mesh.GetSubMeshes() )
					{
						TreeNode subMeshNode = new TreeNode( "[SubMesh]" );
						subMeshNode.Tag = subMesh;
						treeNode.Nodes.Add( subMeshNode );
					}
				}
			}
			
			return treeNode;
		}

		private void UpdateRenderPassTree( TreeView treeView, Renderer renderer )
		{
			treeView.SuspendLayout();
			treeView.Nodes.Clear();

			// Create the root node.
			TreeNode rootNode = new TreeNode( "Renderer" );
			rootNode.Tag = renderer;
			treeView.Nodes.Add( rootNode );

			// Add the current render-technique.
			TreeNode renderTechniqueNode = new TreeNode( renderer.RenderTechnique.Name );
			renderTechniqueNode.Tag = renderer.RenderTechnique;
			rootNode.Nodes.Add( renderTechniqueNode );

			// Add render-stages.
			foreach( var renderStage in renderer.RenderTechnique.GetRenderStages() )
			{
				TreeNode renderStageNode = new TreeNode( renderStage.Name );
				renderStageNode.Tag = renderStage;
				renderTechniqueNode.Nodes.Add( renderStageNode );

				// Add render-passes.
				foreach( var renderPass in renderStage.GetRenderPasses() )
				{
					TreeNode renderPassNode = new TreeNode( renderPass.Name );
					renderPassNode.Tag = renderPass;
					renderStageNode.Nodes.Add( renderPassNode );
				}
			}

			rootNode.ExpandAll();
			treeView.ResumeLayout();
		}

		private void xnaPanel1_Initializing()
		{
			InitializeScene();

			// Load scene objects from scene description.
			m_sceneRoot = SceneObject.CreateFromSceneDescription( xnaPanel1.Content, m_scene );

			// Set up camera.
			m_sceneCamera = new Camera();

			// Create renderer.
			m_renderer = new LightPrePassRenderer( xnaPanel1.GraphicsDevice, xnaPanel1.Content, m_sceneCamera );
			m_renderer.OutputRenderTargetWidth = xnaPanel1.ClientSize.Width;
			m_renderer.OutputRenderTargetHeight = xnaPanel1.ClientSize.Height;

			// Set up camera controller.
			m_cameraController = new FlyCameraController( m_sceneCamera, xnaPanel1 );
			m_cameraController.MovementSpeed = 20.0f;
			m_cameraController.RotationSpeed = 0.1f;
			m_cameraController.SetCameraProjectionParameters( MathHelper.PiOver4, m_renderer.OutputRenderTargetAspectRatio, 0.1f, 1000 );

			// Add scene objects.
			m_renderer.AddScene( m_sceneRoot );
		}

		private void xnaPanel1_Drawing()
		{
			// Update camera controller.
			m_cameraController.Update();

			// Update camera.
			m_sceneCamera.Update( new UpdateContext(), false );

			// Update the scene objects.
			m_sceneRoot.Update( new UpdateContext(), false );

			// Draw scene.
			m_renderer.DoFrame();
		}

		private void xnaPanel1_Resize( object sender, EventArgs e )
		{
			m_renderer.OutputRenderTargetWidth = xnaPanel1.ClientSize.Width;
			m_renderer.OutputRenderTargetHeight = xnaPanel1.ClientSize.Height;

			if( m_cameraController != null )
			{
				m_cameraController.SetCameraProjectionParameters( MathHelper.PiOver4, m_renderer.OutputRenderTargetAspectRatio, 0.1f, 1000 );
			}
		}

		protected override void OnFormClosing( FormClosingEventArgs e )
		{
			// Save current scenes.
			m_scene.WriteToFile( SampleSceneFilePath );

			base.OnFormClosing( e );			
		}

		private void InitializeScene()
		{
			// Load saved scene.
			if( File.Exists( SampleSceneFilePath ) )
			{
				m_scene = SceneDescription.ReadFromFile( SampleSceneFilePath );
			}
			else
			{
				// Create a new scene.
				m_scene = new SceneDescription();
				m_scene.Name = "Sample Scene";

				// Add some models.
				m_scene.Add( new SceneDescriptionModelEntry( "Sponza", "Models/Sponza/Sponza", Transformation.Identity ) );

				m_scene.Add( 
					new SceneDescriptionModelEntry(
						"StreetSign", 
						"Models/street-sign",
						new Transformation( 1.0f, Quaternion.Identity, new Vector3( 0, 5, 0 ) ) ) );

				Transformation tankTransformation = new Transformation( 1.0f, Quaternion.Identity, new Vector3( -5, 0, 10 ) );
				m_scene.Add( new SceneDescriptionModelEntry( "Tank", "Models/tank", ref tankTransformation ) );

				Transformation robot1Transformation = new Transformation( 1.0f, Quaternion.Identity, new Vector3( 5, 3, 10 ) );
				m_scene.Add( new SceneDescriptionModelEntry( "Robot1", "Models/RobotGame/Yager", ref robot1Transformation ) );

				// Add randomly distributed point lights.
				{
					// Point lights
					Random random = new Random();
					for( int i = 0 ; i < 200 ; ++i )
					{
						Vector3 position = new Vector3(
							( float )random.NextDouble() * 200.0f - 100.0f,
							( float )random.NextDouble() * 150.0f,
							( float )random.NextDouble() * 100.0f - 50.0f );
						Vector3 diffuseColor = new Vector3(
							( float )random.NextDouble(),
							( float )random.NextDouble(),
							( float )random.NextDouble() );
						float specularPower = ( float )random.NextDouble() * 10.0f;
						float attenuationDistance = 10.0f + ( float )random.NextDouble() * 190.0f;
						float attenuationDistanceExponent = 1.0f + ( float )random.NextDouble() * 99.0f;

						m_scene.Add( SceneDescriptionLightEntry.CreatePointLight( 
							string.Format( "PointLight{0}", i ), position, diffuseColor, specularPower, attenuationDistance, attenuationDistanceExponent ) );
					}
				}

				// Add a direction light.
				m_scene.Add( SceneDescriptionLightEntry.CreateDirectionalLight( "SkyLight", new Vector3( 0, -1, 0 ), new Vector3( 0.5f, 0.5f, 0.5f ), 1.0f ) );
			}
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
			Text = string.Format( "Tomato Renderer Sample (FPS: {0:F2} / TRIS: {1:0,000})", m_renderer.FPS, m_renderer.TrianglesPerFrame );
		}

		private void treeView1_AfterSelect( object sender, TreeViewEventArgs e )
		{
			propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;
		}

		private void treeView2_AfterSelect( object sender, TreeViewEventArgs e )
		{
			propertyGrid2.SelectedObject = treeView2.SelectedNode.Tag;
		}

		private void xnaPanel1_MouseDown( object sender, MouseEventArgs e )
		{
			xnaPanel1.Invoke( new MethodInvoker( () => { xnaPanel1.Select(); } ) );
		}

		private void button1_Click( object sender, EventArgs e )
		{
			TakeSnapshots( flowLayoutPanel1 );
		}

		private void TakeSnapshots( FlowLayoutPanel flowLayoutPanel )
		{
			flowLayoutPanel.Visible = false;

			Cursor currentCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			flowLayoutPanel.SuspendLayout();
			flowLayoutPanel.Controls.Clear();

			foreach( string renderTargetName in m_renderer.RenderTargets )
			{
				bool bSuccessful = false;

				Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget = m_renderer.LoadRenderTarget( renderTargetName );
				if( renderTarget != null )
				{
					// Add PictureBox control.
					RenderTargetPreviewPanel previewPanel = new RenderTargetPreviewPanel( m_renderer, renderTarget );
					flowLayoutPanel.Controls.Add( previewPanel );
				}

				if( !bSuccessful )
				{
				}
			}

			flowLayoutPanel.Visible = true;
			flowLayoutPanel.ResumeLayout();

			Cursor.Current = currentCursor;
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}
	}
}
