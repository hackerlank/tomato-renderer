#pragma once

namespace Tomato
{
	template<typename StringType, typename CharType>
	class StringTokenizerT
	{
		typedef std::set<CharType> PunctuatorListType;

	public:
		StringTokenizerT( const StringType& text )
			: m_text( text )
			, m_textLength( text.GetLength() )
			, m_position( 0 )
			, m_punctuators()
		{
		}

		virtual ~StringTokenizerT()
		{
		}

		void AddPunctuator( CharType punctuator )
		{
#ifdef _DEBUG
			Assert( !IsPunctuator( punctuator ) );
#endif

			m_punctuators.insert( punctuator );
		}

		bool GetNext( StringType& token )
		{
			s32 nextTokenIndex = GetNextTokenIndex();
			if( nextTokenIndex < 0 )
			{
				return false;
			}

			s32 tokenLength = GetTokenLength( nextTokenIndex );

			m_position = nextTokenIndex + tokenLength;

			token = m_text.SubString( nextTokenIndex, tokenLength );

			return true;
		}

		bool HasMoreTokens() const
		{
			return ( GetNextTokenIndex() >= 0 );
		}

		StringType GetNext()
		{
			StringType token;

			if( !GetNext( token ) )
			{
#ifdef _DEBUG
				Assert( false );
#endif
			}

			return token;
		}

		s32 GetTokenCount() const
		{
			s32 tokenCount = 0;
			bool bPunctuator = false;

			if( m_position < m_textLength )
			{
				if( IsPunctuator( m_text[ m_position ] ) )
				{
					bPunctuator = true;
				}
				else
				{
					bPunctuator = false;
				}
			}
			else
			{
				return 0;
			}

			for( s32 i = m_position + 1 ; i < m_textLength ; ++i )
			{
				if( IsPunctuator( m_text[ i ] ) )
				{
					if( !bPunctuator )
					{
						tokenCount++;
					}

					bPunctuator = true;
				}
				else
				{
					bPunctuator = false;
				}
			}

			if( !bPunctuator )
			{
				tokenCount++;
			}

			return tokenCount;
		}

	protected:
		bool IsPunctuator( CharType c ) const
		{
			return ( m_punctuators.find( c ) != m_punctuators.end() );
		}

		s32 GetNextTokenIndex() const
		{
			return GetNextTokenIndex( m_position );
		}

		s32 GetNextTokenIndex( s32 startIndex ) const
		{
			for( s32 i = startIndex ; i < m_textLength ; ++i )
			{
				if( !IsPunctuator( m_text[ i ] ) )
				{
					return i;
				}
			}

			return -1;
		}

		s32 GetTokenLength( s32 tokenIndex ) const
		{
			Assert( !IsPunctuator( m_text[ tokenIndex ] ) );

			s32 length = 0;
			for( s32 i = tokenIndex ; i < m_textLength ; ++i )
			{
				if( !IsPunctuator( m_text[ i ] ) )
				{
					length++;
				}
				else
				{
					break;
				}
			}

			return length;
		}

	private:
		StringType m_text;
		s32 m_textLength;

		s32 m_position;

		PunctuatorListType m_punctuators;
	};

}