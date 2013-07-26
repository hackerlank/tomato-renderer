using System;
using System.IO;
using System.Xml;

namespace Tomato.Xml
{
	public static class XmlSerializer
	{
		public static XmlNode ToXmlNode<T>( T target )
		{
			try
			{
				using( StringWriter stringWriter = new StringWriter() )
				{
					using( XmlTextWriter xmlWriter = new XmlTextWriter( stringWriter ) )
					{
						System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer( typeof( T ) );
						xmlSerializer.Serialize( xmlWriter, target );

						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml( stringWriter.ToString() );

						return xmlDocument;
					}
				}
			}
			catch(Exception) { }

			return null;
		}

		public static T FromXmlNode<T>( XmlNode xmlNode )
		{
			try
			{
				using( StringReader stringReader = new StringReader( xmlNode.InnerText ) )
				{
					using( XmlTextReader xmlReader = new XmlTextReader( stringReader ) )
					{
						System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer( typeof( T ) );
						return (T)( xmlSerializer.Deserialize( xmlReader ) );
					}
				}
				
			}
			catch(Exception) { }

			return default(T);
		}
	}

}
