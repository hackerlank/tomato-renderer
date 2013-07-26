using System.Collections.Generic;

namespace Tomato.Text
{
	public class StringTokenizer : IEnumerable<string>
	{
		private char[] m_separators;

		/// <summary>
		/// Gets default separator characters.
		/// </summary>
		public static char[] DefaultSeparators { get; private set; }

		/// <summary>
		/// Gets a source string.
		/// </summary>
		public string SourceString { get; private set; }

		static StringTokenizer()
		{
			DefaultSeparators = new char[] 
				{ 
					( char )( 0x0009 ), ( char )( 0x000A ), ( char )( 0x000B ), ( char )( 0x000C ), ( char )( 0x000D ), ( char )( 0x0020 ), ( char )( 0x0085 ), ( char )( 0x00A0 ),
					( char )( 0x1680 ), ( char )( 0x2000 ), ( char )( 0x2001 ), ( char )( 0x2002 ), ( char )( 0x2003 ), ( char )( 0x2004 ), ( char )( 0x2005 ), ( char )( 0x2006 ),
					( char )( 0x2007 ), ( char )( 0x2008 ), ( char )( 0x2009 ), ( char )( 0x200A ), ( char )( 0x200B ), ( char )( 0x2028 ), ( char )( 0x2029 ), ( char )( 0x2030 )
				};
		}

		/// <summary>
		/// Constructs string tokenizer using default separator characters.
		/// </summary>
		/// <param name="source"></param>
		public StringTokenizer( string source )
		{
			SourceString = source;

			m_separators = new char[ DefaultSeparators.Length ];
			DefaultSeparators.CopyTo( m_separators, 0 );
		}

		/// <summary>
		/// Constructs string tokenizer using specified separator characters.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="separators"></param>
		public StringTokenizer( string source, params char[] separators )
		{
			SourceString = source;

			m_separators = new char[ separators.Length ];
			separators.CopyTo( m_separators, 0 );
		}

		/// <summary>
		/// Enumerates string tokens.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<string> GetEnumerator()
		{
			int index = 0;
			while( index < SourceString.Length )
			{
				// Skip whitespaces
				if( SourceString.IndexOfAny( m_separators, index, 1 ) >= 0 )
				{
					index++;
					continue;
				}

				// Find next whitespace position
				int nextPosition = SourceString.IndexOfAny( m_separators, index + 1 );
				if( nextPosition < 0 )
				{
					// This is the last token.
					int startIndex = index;
					index = SourceString.Length;
					yield return SourceString.Substring( startIndex );
				}
				else
				{
					// Return next token.
					int startIndex = index;
					index = nextPosition + 1;
					yield return SourceString.Substring( startIndex, nextPosition - startIndex );
				}
			}
		}

		/// <summary>
		/// Enumerates string tokens.
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
