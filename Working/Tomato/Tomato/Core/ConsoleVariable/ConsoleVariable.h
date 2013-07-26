#pragma once

namespace Tomato
{
	template < typename ValueType, typename TypeDescriptor >
	class ConsoleVariable : public IConsoleVariable
	{
	public:
		ConsoleVariable( const String& name )
			: m_name( name )
			, m_value()
			, m_referenceCount( 1 )
		{ }

		ConsoleVariable( const String& name, ConsoleManager& collection )
			: m_name( name )
			, m_value()
			, m_referenceCount( 1 )
		{
			collection.SetVariable( *this );
		}

		ConsoleVariable( const String& name, const ValueType& value )
			: m_name( name )
			, m_referenceCount( 1 )
			, m_value( value )
		{ 
			//SetValueT( value );
		}

		ConsoleVariable( const String& name, const ValueType& value, ConsoleManager& collection )
			: m_name( name )
			, m_referenceCount( 1 )
			, m_value( value )
		{
			//SetValueT( value );

			collection.SetVariable( *this );
		}

		virtual u32 AddRef()
		{
			return ++m_referenceCount;
		}

		virtual u32 Release()
		{
			--m_referenceCount;

			if( m_referenceCount == 0 )
			{
				delete this;
				return 0;
			}

			return m_referenceCount;
		}

		virtual String GetName() const
		{
			return m_name;
		}

		virtual DataType::Type GetType() const
		{
			return TypeDescriptor::GetType();
		}

		virtual String GetTypeName() const
		{
			return TypeDescriptor::GetTypeName();
		}

		virtual String ToString() const
		{
			return StringFormatter(L"%s|%s|%s",
				GetName().GetCString(),
				GetTypeName().GetCString(),
				GetValue().GetCString() ).GetString();		
		}

		virtual void Dispose()
		{
		}

		virtual String GetValue() const
		{
			return TypeDescriptor::ToString( m_value );
		}

		virtual void SetValue( const String& value )
		{
			SetValueT( TypeDescriptor::FromString( value ) );
		}
		
		virtual void ResetValue()
		{
			SetValueT( ValueType() );
		}

		const ValueType& GetValueT() const
		{
			return m_value;
		}

		void SetValueT( const ValueType& value )
		{
			m_value = value;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator = ( const ValueType& value )
		{
			m_value = value;
			return *this;
		}

		operator ValueType ()
		{
			return m_value;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator += ( const ValueType& value )
		{
			m_value += value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator -= ( const ValueType& value )
		{
			m_value -= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator *= ( const ValueType& value )
		{
			m_value *= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator /= ( const ValueType& value )
		{
			m_value /= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator %= ( const ValueType& value )
		{
			m_value %= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator &= ( const ValueType& value )
		{
			m_value &= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator <<= ( const ValueType& value )
		{
			m_value <<= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator >>= ( const ValueType& value )
		{
			m_value >>= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator ^= ( const ValueType& value )
		{
			m_value ^= value;
			return *this;
		}

		ConsoleVariable< ValueType, TypeDescriptor >& operator |= ( const ValueType& value )
		{
			m_value |= value;
			return *this;
		}

		virtual IConsoleVariable* Clone( const String& name ) const
		{
			ConsoleVariable< ValueType, TypeDescriptor >* pClone = 
				new ConsoleVariable< ValueType, TypeDescriptor >( name, m_value );
			return pClone;
		}

		virtual IConsoleVariable* CloneType( const String& name ) const
		{
			ConsoleVariable< ValueType, TypeDescriptor >* pClone = 
				new ConsoleVariable< ValueType, TypeDescriptor >( name );
			return pClone;
		}

	protected:
		virtual ~ConsoleVariable()
		{
		}

	private:
		String m_name;
		
		ValueType m_value;

		u32 m_referenceCount;
	};
}