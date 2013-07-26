using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics.Content.Pipeline.Debugging
{
	/// <summary>
	/// To debug the XNA Framework Content Pipeline:
	///     1. Modify the constants below to match your project
	///     2. Set this project to be the Startup Project
	///     3. Start debugging
	/// </summary>
	class Program
	{
		private static string s_projectToDebug = @"..\..\..\SampleApplicationContent\SampleApplicationContent.contentproj";

		private const string SingleItem = @"Models\Sponza\Sponza.obj";

		private const GraphicsProfile XnaProfile = GraphicsProfile.HiDef;

		private const TargetPlatform XnaPlatform = TargetPlatform.Windows;

		private const LoggerVerbosity LoggingVerbosity = LoggerVerbosity.Normal;

		private const string Platform = @"x86";

		#region MSBuild hosting and execution

		/// <summary>
		/// This program hosts the MSBuild engine and builds the content project with parameters based
		/// on the constant values specified above.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if( !Path.IsPathRooted( s_projectToDebug ) )
			{
				s_projectToDebug = Path.GetFullPath( s_projectToDebug );
			}

			if( !File.Exists( s_projectToDebug ) )
			{
				throw new FileNotFoundException( String.Format( "The project file '{0}' does not exist. Set the ProjectToDebug field to the full path of the project you want to debug.", s_projectToDebug ), s_projectToDebug );
			}
			if( !String.IsNullOrEmpty( SingleItem ) && !File.Exists( Path.Combine( WorkingDirectory, SingleItem ) ) )
			{
				throw new FileNotFoundException( String.Format( "The project item '{0}' does not exist. Set the SingleItem field to the relative path of the content item you want to debug, or leave it empty to debug the whole project.", SingleItem ), SingleItem );
			}
			Environment.CurrentDirectory = WorkingDirectory;

			var globalProperties = new Dictionary<string, string>();

			globalProperties.Add( "Configuration", Configuration );
			globalProperties.Add( "Platform", Platform );
			globalProperties.Add( "XnaProfile", XnaProfile.ToString() );
			globalProperties.Add( "XNAContentPipelineTargetPlatform", XnaContentPipelineTargetPlatform );
			globalProperties.Add( "SingleItem", SingleItem );
			globalProperties.Add( "CustomAfterMicrosoftCommonTargets", DebuggingTargets );

			var project = ProjectCollection.GlobalProjectCollection.LoadProject( ProjectName, globalProperties, MSBuildVersion );
			bool succeeded = project.Build( "rebuild", Loggers );

			// To read the build output in the console window, place a breakpoint on the
			// Debug.WriteLine statement below.
			Debug.WriteLine( "Build " + ( succeeded ? "Succeeded." : "Failed." ) );
		}

		#region Additional, rarely-changing property values

		private const string Configuration = "Debug";
		private const string MSBuildVersion = "4.0";

		private static IEnumerable<ILogger> Loggers
		{
			get
			{
				return new ILogger[] { new ConsoleLogger( LoggingVerbosity ) };
			}
		}

		private static string WorkingDirectory
		{
			get { return Path.GetDirectoryName( Path.GetFullPath( s_projectToDebug ) ); }
		}

		private static string BuildToolDirectory
		{
			get
			{
				string startupExe = System.Reflection.Assembly.GetEntryAssembly().Location;
				return Path.GetDirectoryName( startupExe );
			}
		}

		private static string ProjectName
		{
			get { return Path.GetFileName( Path.GetFullPath( s_projectToDebug ) ); }
		}

		private static string XnaContentPipelineTargetPlatform
		{
			get
			{
				return XnaPlatform.ToString();
			}
		}

		public static string DebuggingTargets
		{
			get
			{
				if( String.IsNullOrEmpty( SingleItem ) )
				{
					return String.Empty;
				}

				string targetsPath = @"Targets\Debugging.targets";
				return Path.Combine( BuildToolDirectory, targetsPath );
			}
		}

		#endregion

		#endregion
	}
}
