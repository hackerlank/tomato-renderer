#pragma once

namespace Tomato
{
	template < typename ValueType, typename TypeDescriptor >
	class ConsoleVariableProxy
	{
	public:
		ConsoleVariableProxy( const String& name )
		{
			Initialize( name, ValueType() );
		}

		ConsoleVariableProxy( const String& name, ConsoleManager& collection )
		{
			Initialize( name, ValueType(), collection );
		}

		ConsoleVariableProxy( const String& name, const ValueType& value )
		{ 
			Initialize( name, value );
		}

		ConsoleVariableProxy( const String& name, const ValueType& value, ConsoleManager& collection )
		{
			Initialize( name, value, collection );
		}

		ConsoleVariableProxy( const String& name, const ValueType& value, const ValueType& minValue, const ValueType& maxValue, ConsoleManager& collection )
		{
			Initialize( name, ValidateValue( value, minValue, maxValue ), collection );
			
			String minVariableName = StringFormatter( L"%s_min", name.GetBuffer() );
			ConsoleVariable< ValueType, TypeDescriptor >* minVariable = 
				new ConsoleVariable<ValueType, TypeDescriptor>( minVariableName, minValue, collection );
			minVariable->Release();

			String maxVariableName = StringFormatter( L"%s_max", name.GetBuffer() );
			ConsoleVariable< ValueType, TypeDescriptor >* maxVariable = 
				new ConsoleVariable<ValueType, TypeDescriptor>( maxVariableName, maxValue, collection );
			maxVariable->Release();
		}

		ConsoleVariableProxy( const String& name, const ValueType& value, const ValueType& minValue, const ValueType& maxValue, const ValueType& step, ConsoleManager& collection )
		{
			Initialize( name, ValidateValue( value, minValue, maxValue ), collection );
			
			String minVariableName = StringFormatter( L"%s_min", name.GetBuffer() );
			ConsoleVariable< ValueType, TypeDescriptor >* minVariable = 
				new ConsoleVariable<ValueType, TypeDescriptor>( minVariableName, minValue, collection );
			minVariable->Release();

			String maxVariableName = StringFormatter( L"%s_max", name.GetBuffer() );
			ConsoleVariable< ValueType, TypeDescriptor >* maxVariable = 
				new ConsoleVariable<ValueType, TypeDescriptor>( maxVariableName, maxValue, collection );
			maxVariable->Release();

			String stepVariableName = StringFormatter( L"%s_step", name.GetBuffer() );
			ConsoleVariable< ValueType, TypeDescriptor >* stepVariable = 
				new ConsoleVariable<ValueType, TypeDescriptor>( stepVariableName, step, collection );
			stepVariable->Release();
		}

		IConsoleVariable* GetInterface() const
		{
			return m_pVariable;
		}
	
		operator ValueType () const
		{
			return m_pVariable->GetValueT();
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator = ( const ValueType& value )
		{
			m_pVariable->SetValueT( value );
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator += ( const ValueType& value )
		{
			( *m_pVariable ) += value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator -= ( const ValueType& value )
		{
			( *m_pVariable ) -= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator *= ( const ValueType& value )
		{
			( *m_pVariable ) *= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator /= ( const ValueType& value )
		{
			( *m_pVariable ) /= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator %= ( const ValueType& value )
		{
			( *m_pVariable ) %= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator &= ( const ValueType& value )
		{
			( *m_pVariable ) &= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator <<= ( const ValueType& value )
		{
			( *m_pVariable ) <<= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator >>= ( const ValueType& value )
		{
			( *m_pVariable ) >>= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator ^= ( const ValueType& value )
		{
			( *m_pVariable ) ^= value;
			return *this;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator |= ( const ValueType& value )
		{
			( *m_pVariable ) |= value;
			return *this;
		}

		ValueType GetValue() const
		{
			return m_pVariable->GetValueT();
		}

		virtual ~ConsoleVariableProxy()
		{
			if( m_pVariable )
			{
				m_pVariable->Release();
				m_pVariable = NULL;
			}
		}

	private:
		ConsoleVariableProxy( const ConsoleVariableProxy<ValueType, TypeDescriptor>& copy )
			: m_pVariable( NULL )
		{
			*this = copy;
		}

		ConsoleVariableProxy<ValueType, TypeDescriptor>& operator = ( const ConsoleVariableProxy<ValueType, TypeDescriptor>& copy )
		{
			if( m_pVariable != copy.m_pVariable )
			{
				if( m_pVariable != NULL ) { m_pVariable->Release(); }

				m_pVariable = copy.m_pVariable;

				if( m_pVariable != NULL ) { m_pVariable->AddRef(); }
			}

			return *this;
		}

	private:
		void Initialize(
			const String& name, 
			const ValueType& value )
		{
			m_pVariable = new ConsoleVariable< ValueType, TypeDescriptor >( name, value );
		}

		void Initialize(
			const String& name, 
			const ValueType& value, 
			ConsoleManager& collection )
		{
			IConsoleVariable* pVariable = collection.GetVariable( name );
			if( pVariable )
			{
				ConsoleVariable< ValueType, TypeDescriptor >* pConv = 
					dynamic_cast<ConsoleVariable< ValueType, TypeDescriptor >*>( pVariable );

				if( pConv )
				{
					m_pVariable = pConv;
				}
				else
				{
					throw L"Type mismatch";
				}
			}
			else
			{
				m_pVariable = new ConsoleVariable<ValueType, TypeDescriptor>( name, value, collection );
			}
		}

	private:
		ValueType ValidateValue( const ValueType& value, const ValueType& minValue, const ValueType& maxValue )
		{
			if( minValue <= maxValue )
			{
				if( minValue <= value )
				{
					if( value <= maxValue ) { return value; }
					else { return maxValue; }
				}
				else { return minValue; }
			}
			else
			{
				if( maxValue <= value )
				{
					if( value <= minValue ) { return value; }
					else { return minValue; }
				}
				else { return maxValue; }
			}
		}

		f32 ValidateFloatValue( const f32& value, const f32& minValue, const f32& maxValue )
		{
			if( minValue <= maxValue )
			{
				if( minValue <= value )
				{
					if( value <= maxValue ) { return value; }
					else { return maxValue; }
				}
				else { return minValue; }
			}
			else
			{
				if( maxValue <= value )
				{
					if( value <= minValue ) { return value; }
					else { return minValue; }
				}
				else { return maxValue; }
			}
		}

	private:
		ConsoleVariable< ValueType, TypeDescriptor >* m_pVariable;
	};

	template<> Vector2 ConsoleVariableProxy<Vector2, ConsoleBasicTypeDescVector2>::ValidateValue( const Vector2& value, const Vector2& minValue, const Vector2& maxValue )
	{
		return Vector2(
			ValidateFloatValue( value.X, minValue.X, maxValue.X ),
			ValidateFloatValue( value.Y, minValue.Y, maxValue.Y ) );
	}

	template<> Vector3 ConsoleVariableProxy<Vector3, ConsoleBasicTypeDescVector3>::ValidateValue( const Vector3& value, const Vector3& minValue, const Vector3& maxValue )
	{
		return Vector3(
			ValidateFloatValue( value.X, minValue.X, maxValue.X ),
			ValidateFloatValue( value.Y, minValue.Y, maxValue.Y ),
			ValidateFloatValue( value.Z, minValue.Z, maxValue.Z ) );
	}

	template<> Vector4 ConsoleVariableProxy<Vector4, ConsoleBasicTypeDescVector4>::ValidateValue( const Vector4& value, const Vector4& minValue, const Vector4& maxValue )
	{
		return Vector4(
			ValidateFloatValue( value.X, minValue.X, maxValue.X ),
			ValidateFloatValue( value.Y, minValue.Y, maxValue.Y ),
			ValidateFloatValue( value.Z, minValue.Z, maxValue.Z ),
			ValidateFloatValue( value.W, minValue.W, maxValue.W ) );
	}
}