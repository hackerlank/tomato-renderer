using System;
using System.Collections.Generic;
using System.Threading;

namespace Tomato
{
	[System.Diagnostics.DebuggerDisplay("{Message}")]
	public class EventMessageLog : EventLog
	{
		public string Message { get; protected set; }

		public EventMessageLog( string message )
			: base( null )
		{
			Message = message;
		}

		public EventMessageLog( object source, string message )
			: base( source )
		{
			Message = message;
		}

		public override string ToString()
		{
			return Message;
		}
	}
}