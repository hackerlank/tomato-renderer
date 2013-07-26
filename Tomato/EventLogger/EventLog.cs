using System;
using System.Collections.Generic;
using System.Threading;

namespace Tomato
{
	public class EventLog
	{
		public object Source { get; protected set; }

		public EventLog( object source )
		{
			Source = source;
		}
	}
}