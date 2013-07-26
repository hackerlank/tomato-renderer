using System;
using System.Collections.Generic;
using System.Threading;

namespace Tomato
{
	public sealed class EventLogger : IEnumerable<EventLog>
	{
		private int m_maximumLogCount;

		private LinkedList<EventLog> m_eventLogs = null;

		public bool IsActive { get; set; }

		public bool IsDebugOutputEnabled { get; set; }

		public EventLogger()
			: this( 1024 )
		{ }

		public EventLogger( int maximumLogCount )
		{
			m_maximumLogCount = maximumLogCount;

			m_eventLogs = new LinkedList<EventLog>();

			// By default, event logger is inactive.
			IsActive = false;

			IsDebugOutputEnabled = false;
		}

		public void AddEventLog( string message )
		{
			AddEventLog( null, message );
		}

		public void AddEventLog( object source, string message )
		{
			if( IsActive )
			{
				if( IsDebugOutputEnabled )
				{
					System.Diagnostics.Debug.WriteLine( message );
				}

				m_eventLogs.AddLast( new EventMessageLog( source, message ) );

				if( ( m_maximumLogCount > 0 )
					&& ( m_eventLogs.Count > m_maximumLogCount ) )
				{
					m_eventLogs.RemoveFirst();
				}
			}
		}

		public void ClearEventLogs()
		{
			if( IsActive )
			{
				m_eventLogs.Clear();
			}
		}

		public IEnumerator<EventLog> GetEnumerator()
		{
			foreach( EventLog log in m_eventLogs )
			{
				yield return log;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}