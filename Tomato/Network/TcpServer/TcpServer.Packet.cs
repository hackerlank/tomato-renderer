using System;

namespace Tomato.Network
{
	partial class TcpServer
	{
		public enum PacketType : uint
		{
			ClientPacket,
			ManagementEvent
		}

		public interface IPacket
		{
			PacketType PacketType { get; }
		}

		public sealed class ClientPacket : IPacket
		{
			public PacketType PacketType { get { return PacketType.ClientPacket; } }

			public long ClientID { get; private set; }
			public byte[] RawData { get; private set; }

			public ClientPacket( long clientID, byte[] rawData )
			{
				ClientID = clientID;
				RawData = rawData;
			}
		}

		public enum ManagementEventType : uint
		{
			AbnormalClientDisconnection
		}

		public class ManagementEvent : IPacket
		{
			public PacketType PacketType { get { return PacketType.ManagementEvent; } }

			public ManagementEventType EventType { get; private set; }

			public ManagementEvent( ManagementEventType eventType )
			{
				EventType = eventType;
			}
		}

		public class AbnormalClientDisconnectionSystemEventPacket : ManagementEvent
		{
			public long ClientID { get; private set; }

			public AbnormalClientDisconnectionSystemEventPacket( long clientID )
				: base( ManagementEventType.AbnormalClientDisconnection )
			{
				ClientID = clientID;
			}
		}
	}
}
