#include "TomatoPCH.h"

#include "ConsoleManager.h" 

#include <algorithm>

#include "ConsoleVariable.h"
#include "ConsoleMessageType.h"

namespace Tomato
{
	typedef 
		ConsoleVariable<s32, ConsoleBasicTypeDescInt> 
		S32ConsoleVariableInternal;

	typedef 
		ConsoleVariable<std::vector<s32>, ConsoleBasicTypeDescIntArray> 
		S32ArrayConsoleVariableInternal;

	typedef 
		ConsoleVariable<f32, ConsoleBasicTypeDescFloat> 
		F32ConsoleVariableInternal;

	typedef 
		ConsoleVariable<std::vector<f32>, ConsoleBasicTypeDescFloatArray> 
		F32ArrayConsoleVariableInternal;

	typedef 
		ConsoleVariable<f64, ConsoleBasicTypeDescDouble> 
		F64ConsoleVariableInternal;

	typedef 
		ConsoleVariable<std::vector<f64>, ConsoleBasicTypeDescDoubleArray> 
		F64ArrayConsoleVariableInternal;

	typedef 
		ConsoleVariable<u32, ConsoleBasicTypeDescUInt> 
		U32ConsoleVariableInternal;

	typedef 
		ConsoleVariable<std::vector<u32>, ConsoleBasicTypeDescUIntArray> 
		U32ArrayConsoleVariableInternal;

	typedef 
		ConsoleVariable<bool, ConsoleBasicTypeDescBool> 
		BoolConsoleVariableInternal;

	typedef 
		ConsoleVariable<std::vector<bool>, ConsoleBasicTypeDescBoolArray> 
		BoolArrayConsoleVariableInternal;

	typedef 
		ConsoleVariable<String, ConsoleBasicTypeDescString> 
		StringConsoleVariableInternal;

	typedef 
		ConsoleVariable<Vector2, ConsoleBasicTypeDescVector2> 
		Vector2ConsoleVariableInternal;

	typedef 
		ConsoleVariable<Vector3, ConsoleBasicTypeDescVector3>
		Vector3ConsoleVariableInternal;

	typedef 
		ConsoleVariable<Vector4, ConsoleBasicTypeDescVector4>
		Vector4ConsoleVariableInternal;

	typedef 
		ConsoleVariable<Matrix4, ConsoleBasicTypeDescMatrix4>
		Matrix4ConsoleVariableInternal;

	class ConsoleManager::Impl
	{
	public:

		class ConsoleVariableInfo
		{
		public:
			ConsoleVariableInfo(
				IConsoleVariable* pVariable,
				bool bStronglyTyped = true,
				bool bTypeProxy = false)
				: m_pVariable( pVariable )
				, m_bStronglyTyped( bStronglyTyped )
				, m_bTypeProxy( bTypeProxy )
			{ 
			}

		public:
			IConsoleVariable* m_pVariable;
			bool m_bStronglyTyped;
			bool m_bTypeProxy;
		};

		void Initialize();

		void Dispose();

		//void ProcessMessage( const MessageStream& commandMessage, MessageStream& outputResult );

		void SetVariable( IConsoleVariable* pVariable, bool bAutoTypeRegistry );

		IConsoleVariable* GetVariable( const String& name );

		void DeleteVariable( IConsoleVariable& variable );

		ConsoleVariableInfo* AddInternal(
			IConsoleVariable* pVariable, 
			bool bStronglyTyped = true, 
			bool bTypeProxy = false) ;

		ConsoleVariableInfo* Add(
			const String& name, 
			const String& type, 
			bool bStronglyTyped = true );

		ConsoleVariableInfo* Add(
			const String& name, 
			const String& type, 
			const String& value, 
			bool bStronglyTyped = true );

		template < typename T, typename TypeDesc>
		ConsoleVariableInfo* AddT(
			const String& name, 
			const T& value, 
			bool bStronglyTyped = true );

		void Remove( const String& name );

		void RemoveAll( bool bRemoveTypeProxies = false );

		ConsoleVariableInfo* Find( const String& name );

		template< typename T, typename TypeDesc>
		IConsoleVariable* SetValueT( const String& name, const T& value );

		template< typename T, typename TypeDesc>
		const T& GetValueT( const String& name );

		std::vector<String> ToStringVector( const String& string )
		{
			std::vector<String> stringVector;
			stringVector.push_back( string );
			return stringVector;
		}

		template<typename T> const T& GetBasicTypeProxyValue(); // no implementation provided
		template<> const s32& GetBasicTypeProxyValue() { return m_pIntProxy->GetValueT(); }
		template<> const std::vector<s32>& GetBasicTypeProxyValue() { return m_pIntArrayProxy->GetValueT(); }
		template<> const u32& GetBasicTypeProxyValue() { return m_pUIntProxy->GetValueT(); }
		template<> const std::vector<u32>& GetBasicTypeProxyValue() { return m_pUIntArrayProxy->GetValueT(); }
		template<> const bool& GetBasicTypeProxyValue() { return m_pBoolProxy->GetValueT(); }
		template<> const std::vector<bool>& GetBasicTypeProxyValue() { return m_pBoolArrayProxy->GetValueT(); }
		template<> const f32& GetBasicTypeProxyValue() { return m_pFloatProxy->GetValueT(); }
		template<> const std::vector<f32>& GetBasicTypeProxyValue() { return m_pFloatArrayProxy->GetValueT(); }
		template<> const f64& GetBasicTypeProxyValue() { return m_pDoubleProxy->GetValueT(); }
		template<> const std::vector<f64>& GetBasicTypeProxyValue() { return m_pDoubleArrayProxy->GetValueT(); }
		template<> const String& GetBasicTypeProxyValue() { return m_pStringProxy->GetValueT(); }
		template<> const Vector2& GetBasicTypeProxyValue() { return m_pVector2Proxy->GetValueT(); }
		template<> const Vector3& GetBasicTypeProxyValue() { return m_pVector3Proxy->GetValueT(); }
		template<> const Vector4& GetBasicTypeProxyValue() { return m_pVector4Proxy->GetValueT(); }
		template<> const Matrix4& GetBasicTypeProxyValue() { return m_pMatrix4Proxy->GetValueT(); }

		String DetermineType( const String& name, const String& value );

		bool IsNumber( const String& string );

		void RegisterTypeProxy( IConsoleVariable* pTypeProxy );

		IConsoleVariable* FindTypeProxy( const String& typeName );

		//void WriteToStream( IConsoleVariable* pVariable, MessageStream& outputStream );

		//void ProcessCommandList( const MessageStream& command, MessageStream& outputStream );
		//void ProcessCommandRemove( const MessageStream& command, MessageStream& outputStream );
		//void ProcessCommandReset( const MessageStream& command, MessageStream& outputStream );
		//void ProcessCommandClear( const MessageStream& command, MessageStream& outputStream );

		void RegisterCommandProcessor( ConsoleManager::ICommandProcessor* pProcessor );
		void UnregisterCommandProcessor( ConsoleManager::ICommandProcessor* pProcessor );

		bool IsValidVariableName( const String& name );

		void SetConsoleVariableFilePath( const String& filePath );
		void SaveConsoleVariables();
		void LoadConsoleVariables();
		void DeleteConsoleVariableFile();

		std::map<String, ConsoleVariableInfo*> m_variables;

		S32ConsoleVariableInternal* m_pIntProxy;
		S32ArrayConsoleVariableInternal* m_pIntArrayProxy;
		U32ConsoleVariableInternal* m_pUIntProxy;
		U32ArrayConsoleVariableInternal* m_pUIntArrayProxy;
		F32ConsoleVariableInternal* m_pFloatProxy;
		F32ArrayConsoleVariableInternal* m_pFloatArrayProxy;
		F64ConsoleVariableInternal* m_pDoubleProxy;
		F64ArrayConsoleVariableInternal* m_pDoubleArrayProxy;
		BoolConsoleVariableInternal* m_pBoolProxy;
		BoolArrayConsoleVariableInternal* m_pBoolArrayProxy;
		StringConsoleVariableInternal* m_pStringProxy;
		Vector2ConsoleVariableInternal* m_pVector2Proxy;
		Vector3ConsoleVariableInternal* m_pVector3Proxy;
		Vector4ConsoleVariableInternal* m_pVector4Proxy;
		Matrix4ConsoleVariableInternal* m_pMatrix4Proxy;

		std::vector<ConsoleManager::ICommandProcessor*> m_CommandProcessors;

		String m_consoleVariableFilePath;
	};

	ConsoleManager::ConsoleManager()
		: m_pImpl(NULL)
#ifndef _RELEASE
		//, m_packetBuffer( 8192 )
#endif
	{
		m_pImpl = new Impl;
		m_pImpl->Initialize();
	}

	ConsoleManager::~ConsoleManager()
	{ 		
#ifndef _RELEASE
		//m_consoleServer.Shutdown();
#endif
		m_pImpl->Dispose();
		delete m_pImpl;
		m_pImpl = NULL;
	}

	void ConsoleManager::SetConsoleVariableFilePath( const String& filePath )
	{
		m_pImpl->SetConsoleVariableFilePath( filePath );
	}

	void ConsoleManager::SaveConsoleVariables()
	{
		m_pImpl->SaveConsoleVariables();
	}

	void ConsoleManager::LoadConsoleVariables()
	{
		m_pImpl->LoadConsoleVariables();
	}

#ifndef _RELEASE
	void ConsoleManager::InitializeConsoleServer( s32 /*listeningPort*/ )
	{
		//m_consoleServer.Initialize( listeningPort, 16, ConsoleServerPacketSize, ConsoleServerPacketSize );
		//m_consoleServer.Listen( 200 );
		//m_consoleServer.Start();
	}

	void ConsoleManager::UpdateConsoleServer()
	{
#if 0
		s32 clientID = 0;

		m_packetBuffer.Reset( true );
		if( m_consoleServer.Recv( clientID, m_packetBuffer ) )
		{	
			if( m_packetBuffer.GetPayloadLength() > 0 )
			{	
				m_packetBuffer.ReadHeaderU16(); // Çì´õ

				MessageStream outputResult;			
				ProcessMessage( m_packetBuffer, outputResult );

				if( outputResult.GetLength() > sizeof( u32 ) )
				{
					m_consoleServer.Send( clientID, reinterpret_cast<const char*>( outputResult.GetBuffer() ), outputResult.GetLength() );
				}
			}			
		}
#endif
	}
#endif // _RELEASE

#if 0
	void ConsoleManager::ProcessMessage( const MessageStream& commandMessage, MessageStream& outputResult )
	{
		m_pImpl->ProcessMessage( commandMessage, outputResult );
	}
#endif

	void ConsoleManager::RegisterCommandProcessor( ICommandProcessor* pProcessor )
	{
		m_pImpl->RegisterCommandProcessor( pProcessor );
	}

	void ConsoleManager::UnregisterCommandProcessor( ICommandProcessor* pProcessor )
	{
		m_pImpl->UnregisterCommandProcessor( pProcessor );
	}

	IConsoleVariable* ConsoleManager::GetVariable( const String& name ) const
	{
		return m_pImpl->GetVariable( name );
	}

	void ConsoleManager::SetVariable( IConsoleVariable& variable, bool bAutoTypeRegistry )
	{
		m_pImpl->SetVariable( &variable, bAutoTypeRegistry );
	}

	void ConsoleManager::DeleteVariable( IConsoleVariable& variable )
	{
		m_pImpl->DeleteVariable( variable );
	}

	void ConsoleManager::RegisterTypeProxy( IConsoleVariable* pTypeProxy )
	{
		m_pImpl->RegisterTypeProxy( pTypeProxy );
	}

	String ConsoleManager::DetermineType( const String& name, const String& value )
	{
		return m_pImpl->DetermineType( name, value );
	}

	IConsoleVariable* ConsoleManager::SetInt( const String& name, const s32& value ) 
	{ return m_pImpl->SetValueT<s32, ConsoleBasicTypeDescInt>( name, value ); }

	IConsoleVariable* ConsoleManager::SetUInt( const String& name, const u32& value ) 
	{ return m_pImpl->SetValueT<u32, ConsoleBasicTypeDescUInt>( name, value ); }

	IConsoleVariable* ConsoleManager::SetBool( const String& name, const bool& value ) 
	{ return m_pImpl->SetValueT<bool, ConsoleBasicTypeDescBool>( name, value ); }

	IConsoleVariable* ConsoleManager::SetFloat( const String& name, const f32& value ) 
	{ return m_pImpl->SetValueT<f32, ConsoleBasicTypeDescFloat>( name, value ); }

	IConsoleVariable* ConsoleManager::SetDouble( const String& name, const f64& value ) 
	{ return m_pImpl->SetValueT<f64, ConsoleBasicTypeDescDouble>( name, value ); }

	IConsoleVariable* ConsoleManager::SetIntArray( const String& name, const std::vector<s32>& value ) 
	{ return m_pImpl->SetValueT<std::vector<s32>, ConsoleBasicTypeDescIntArray>( name, value ); }

	IConsoleVariable* ConsoleManager::SetUIntArray( const String& name, const std::vector<u32>& value ) 
	{ return m_pImpl->SetValueT<std::vector<u32>, ConsoleBasicTypeDescUIntArray>( name, value ); }

	IConsoleVariable* ConsoleManager::SetBoolArray( const String& name, const std::vector<bool>& value ) 
	{ return m_pImpl->SetValueT<std::vector<bool>, ConsoleBasicTypeDescBoolArray>( name, value ); }

	IConsoleVariable* ConsoleManager::SetFloatArray( const String& name, const std::vector<f32>& value ) 
	{ return m_pImpl->SetValueT<std::vector<f32>, ConsoleBasicTypeDescFloatArray>( name, value ); }

	IConsoleVariable* ConsoleManager::SetDoubleArray( const String& name, const std::vector<f64>& value )
	{ return m_pImpl->SetValueT<std::vector<f64>, ConsoleBasicTypeDescDoubleArray>( name, value ); }

	IConsoleVariable* ConsoleManager::SetString( const String& name, const String& value ) 
	{ return m_pImpl->SetValueT<String, ConsoleBasicTypeDescString>( name, value ); }

	IConsoleVariable* ConsoleManager::SetVector2( const String& name, const Vector2& value ) 
	{ return m_pImpl->SetValueT<Vector2, ConsoleBasicTypeDescVector2>( name, value ); }

	IConsoleVariable* ConsoleManager::SetVector3( const String& name, const Vector3& value ) 
	{ return m_pImpl->SetValueT<Vector3, ConsoleBasicTypeDescVector3>( name, value ); }

	IConsoleVariable* ConsoleManager::SetVector4( const String& name, const Vector4& value ) 
	{ return m_pImpl->SetValueT<Vector4, ConsoleBasicTypeDescVector4>( name, value ); }

	IConsoleVariable* ConsoleManager::SetMatrix4( const String& name, const Matrix4& value ) 
	{ return m_pImpl->SetValueT<Matrix4, ConsoleBasicTypeDescMatrix4>( name, value ); }

	const s32& ConsoleManager::GetInt( const String& name ) const 
	{ return m_pImpl->GetValueT<s32, ConsoleBasicTypeDescInt>( name ); }

	const u32& ConsoleManager::GetUInt( const String& name ) const 
	{ return m_pImpl->GetValueT<u32, ConsoleBasicTypeDescUInt>( name ); }

	const bool& ConsoleManager::GetBool( const String& name ) const 
	{ return m_pImpl->GetValueT<bool, ConsoleBasicTypeDescBool>( name ); }

	const f32& ConsoleManager::GetFloat( const String& name ) const 
	{ return m_pImpl->GetValueT<f32, ConsoleBasicTypeDescFloat>( name ); }

	const f64& ConsoleManager::GetDouble( const String& name ) const 
	{ return m_pImpl->GetValueT<f64, ConsoleBasicTypeDescDouble>( name ); }

	const std::vector<s32>& ConsoleManager::GetIntArray( const String& name ) const 
	{ return m_pImpl->GetValueT<std::vector<s32>, ConsoleBasicTypeDescIntArray>( name ); }

	const std::vector<u32>& ConsoleManager::GetUIntArray( const String& name ) const 
	{ return m_pImpl->GetValueT<std::vector<u32>, ConsoleBasicTypeDescUIntArray>( name ); }

	const std::vector<bool>& ConsoleManager::GetBoolArray( const String& name ) const 
	{ return m_pImpl->GetValueT<std::vector<bool>, ConsoleBasicTypeDescBoolArray>( name ); }

	const std::vector<f32>& ConsoleManager::GetFloatArray( const String& name ) const 
	{ return m_pImpl->GetValueT<std::vector<f32>, ConsoleBasicTypeDescFloatArray>( name ); }

	const std::vector<f64>& ConsoleManager::GetDoubleArray( const String& name ) const 
	{ return m_pImpl->GetValueT<std::vector<f64>, ConsoleBasicTypeDescDoubleArray>( name ); }

	const String& ConsoleManager::GetString( const String& name) const 
	{ return m_pImpl->GetValueT<String, ConsoleBasicTypeDescString>( name ); }

	const Vector2& ConsoleManager::GetVector2( const String& name ) const 
	{ return m_pImpl->GetValueT<Vector2, ConsoleBasicTypeDescVector2>( name ); }	

	const Vector3& ConsoleManager::GetVector3( const String& name ) const 
	{ return m_pImpl->GetValueT<Vector3, ConsoleBasicTypeDescVector3>( name ); }

	const Vector4& ConsoleManager::GetVector4( const String& name ) const 
	{ return m_pImpl->GetValueT<Vector4, ConsoleBasicTypeDescVector4>( name ); }

	const Matrix4& ConsoleManager::GetMatrix4( const String& name ) const 
	{ return m_pImpl->GetValueT<Matrix4, ConsoleBasicTypeDescMatrix4>( name ); }

	void ConsoleManager::Impl::Initialize()
	{
		m_pIntProxy = new S32ConsoleVariableInternal( L"" );
		m_pIntArrayProxy = new S32ArrayConsoleVariableInternal( L"" );
		m_pUIntProxy = new U32ConsoleVariableInternal( L"" );
		m_pUIntArrayProxy = new U32ArrayConsoleVariableInternal( L"" );
		m_pBoolProxy = new BoolConsoleVariableInternal( L"" );
		m_pBoolArrayProxy = new BoolArrayConsoleVariableInternal( L"" );
		m_pFloatProxy = new F32ConsoleVariableInternal( L"" );
		m_pFloatArrayProxy = new F32ArrayConsoleVariableInternal( L"" );
		m_pDoubleProxy = new F64ConsoleVariableInternal( L"" );
		m_pDoubleArrayProxy = new F64ArrayConsoleVariableInternal( L"" );
		m_pStringProxy = new StringConsoleVariableInternal( L"" );
		m_pVector2Proxy = new Vector2ConsoleVariableInternal( L"" );
		m_pVector3Proxy = new Vector3ConsoleVariableInternal( L"" );
		m_pVector4Proxy = new Vector4ConsoleVariableInternal( L"" );
		m_pMatrix4Proxy = new Matrix4ConsoleVariableInternal( L"" );

		RegisterTypeProxy( m_pIntProxy );
		RegisterTypeProxy( m_pIntArrayProxy );
		RegisterTypeProxy( m_pUIntProxy );
		RegisterTypeProxy( m_pUIntArrayProxy );
		RegisterTypeProxy( m_pBoolProxy );
		RegisterTypeProxy( m_pBoolArrayProxy );
		RegisterTypeProxy( m_pFloatProxy );
		RegisterTypeProxy( m_pFloatArrayProxy );
		RegisterTypeProxy( m_pDoubleProxy );
		RegisterTypeProxy( m_pDoubleArrayProxy );
		RegisterTypeProxy( m_pStringProxy );
		RegisterTypeProxy( m_pVector2Proxy );
		RegisterTypeProxy( m_pVector3Proxy );
		RegisterTypeProxy( m_pVector4Proxy );
		RegisterTypeProxy( m_pMatrix4Proxy );
	}

	void ConsoleManager::Impl::Dispose()
	{
		RemoveAll(true);

		m_pIntProxy->Release();
		m_pIntArrayProxy->Release();
		m_pUIntProxy->Release();
		m_pUIntArrayProxy->Release();
		m_pBoolProxy->Release();
		m_pBoolArrayProxy->Release();
		m_pFloatProxy->Release();
		m_pFloatArrayProxy->Release();
		m_pDoubleProxy->Release();
		m_pDoubleArrayProxy->Release();
		m_pStringProxy->Release();
		m_pVector2Proxy->Release();
		m_pVector3Proxy->Release();
		m_pVector4Proxy->Release();
		m_pMatrix4Proxy->Release();
	}

	void ConsoleManager::Impl::SetVariable( IConsoleVariable* pVariable, bool bAutoTypeRegistry )
	{
		if( pVariable )
		{
			if( bAutoTypeRegistry )
			{
				IConsoleVariable *pProxy = FindTypeProxy( pVariable->GetTypeName() );
				if( !pProxy )
				{
					RegisterTypeProxy( pVariable );
				}
			}

			pVariable->AddRef();
			AddInternal( pVariable );
		}
	}

	IConsoleVariable* ConsoleManager::Impl::GetVariable( const String& name )
	{
		ConsoleVariableInfo* pVariableInfo = Find( name );

		if( pVariableInfo ) 
		{ 
			pVariableInfo->m_pVariable->AddRef();
			return pVariableInfo->m_pVariable; 
		}
		else 
		{ 
			return static_cast<IConsoleVariable*>( 0 ); 
		}
	}

	void ConsoleManager::Impl::DeleteVariable( IConsoleVariable& variable )
	{
		String name = variable.GetName();
		ConsoleVariableInfo* pVariableInfo = Find( name );

		if( pVariableInfo )
		{
			if( pVariableInfo->m_pVariable == &variable )
			{
				Remove( name );
			}
		}
	}

	void ConsoleManager::Impl::RegisterTypeProxy( IConsoleVariable* pTypeProxy ) 
	{
		if( pTypeProxy )
		{
			String proxyName = String( L"@" ) + pTypeProxy->GetTypeName();
			AddInternal( pTypeProxy->Clone(proxyName), true, true );
		}
	}

#if 0
	void ConsoleManager::Impl::ProcessMessage( const MessageStream& commandMessage, MessageStream& outputResult )
	{
		ConsoleMessageType::Type command = static_cast<ConsoleMessageType::Type>( commandMessage.ReadU16() );
		switch ( command )
		{
		case ConsoleMessageType::List:
			{
				ProcessCommandList( commandMessage, outputResult );
			}
			break;

		case ConsoleMessageType::Remove:
			{
				ProcessCommandRemove( commandMessage, outputResult );
			}
			break;

		case ConsoleMessageType::Reset:
			{
				ProcessCommandReset( commandMessage, outputResult );
			}
			break;

		case ConsoleMessageType::Clear:
			{
				ProcessCommandClear( commandMessage, outputResult );
			}
			break;

		case ConsoleMessageType::SaveFile:
			{
				SaveConsoleVariables();
			}
			break;

		case ConsoleMessageType::DeleteFile:
			{
				DeleteConsoleVariableFile();
			}
			break;

		case ConsoleMessageType::Get:
			{
				String variableName = commandMessage.ReadString();

				outputResult.WriteU16( ConsoleMessageType::Get );

				IConsoleVariable* pVariable = NULL;

				if( m_pConsoleVariableSet != NULL )
				{
					ConsoleVariableSet* pCurrentSet = m_pConsoleVariableSet->GetTail();

					while( pCurrentSet != NULL )
					{
						pVariable = pCurrentSet->GetVariable( variableName );

						if( pVariable != NULL )
						{
							break;
						}

						pCurrentSet = pCurrentSet->GetParent();
					}
				}

				if( pVariable == NULL )
				{
					ConsoleVariableInfo* pVariableInfo = Find( variableName );
					if( pVariableInfo != NULL )
					{
						pVariable = pVariableInfo->m_pVariable;
					}
				}			

				if( pVariable != NULL )
				{
					outputResult.WriteBool( true );
					WriteToStream( pVariable, outputResult );
				}
				else
				{
					outputResult.WriteBool( false );
					outputResult.WriteString( variableName );
				}				
			}
			break;

		case ConsoleMessageType::Set:
			{
				String variableName = commandMessage.ReadString();
				DataType::Type valueType = static_cast<DataType::Type>( commandMessage.ReadU8() );

				outputResult.WriteU16( ConsoleMessageType::Set );
				outputResult.WriteString( variableName );

				IConsoleVariable* pVariable = NULL;

				if( m_pConsoleVariableSet != NULL )
				{
					ConsoleVariableSet* pCurrentSet = m_pConsoleVariableSet->GetTail();

					while( pCurrentSet != NULL )
					{
						pVariable = pCurrentSet->GetVariable( variableName );

						if( pVariable != NULL )
						{
							break;
						}

						pCurrentSet = pCurrentSet->GetParent();
					}
				}

				if( pVariable == NULL )
				{
					ConsoleVariableInfo* pVariableInfo = Find( variableName );
					if( pVariableInfo != NULL )
					{
						pVariable = pVariableInfo->m_pVariable;
					}
				}
				

				if( pVariable != NULL )
				{
					Assert( pVariable->GetType() == valueType );

					switch( valueType )
					{
					case DataType::Bool:
						{
							bool value = commandMessage.ReadBool();

							static_cast<BoolConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::BoolArray:
						{
							size_t valueCount = commandMessage.ReadU32();
							std::vector<bool> values;
							values.resize( valueCount );
							for( size_t i = 0 ; i < valueCount ; ++i )
							{
								values[ i ] = commandMessage.ReadBool();
							}

							static_cast<BoolArrayConsoleVariableInternal*>( pVariable )->SetValueT( values );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Int:
						{
							s32 value = commandMessage.ReadS32();

							static_cast<S32ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::IntArray:
						{
							size_t valueCount = commandMessage.ReadU32();
							std::vector<s32> values;
							values.resize( valueCount );
							for( size_t i = 0 ; i < valueCount ; ++i )
							{
								values[i] = commandMessage.ReadS32();
							}

							static_cast<S32ArrayConsoleVariableInternal*>( pVariable )->SetValueT( values );

							outputResult.WriteBool( true );

						}
						break;

					case DataType::UInt:
						{
							u32 value = commandMessage.ReadU32();
							
							static_cast<U32ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::UIntArray:
						{
							size_t valueCount = commandMessage.ReadU32();
							std::vector<u32> values;
							values.resize( valueCount );
							for( size_t i = 0 ; i < valueCount ; ++i )
							{
								values[i] = commandMessage.ReadU32();
							}

							static_cast<U32ArrayConsoleVariableInternal*>( pVariable )->SetValueT( values );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Float:
						{
							f32 value = commandMessage.ReadF32();
							
							static_cast<F32ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::FloatArray:
						{
							size_t valueCount = commandMessage.ReadU32();
							std::vector< f32 > values;
							values.resize( valueCount );
							for( size_t i = 0 ; i < valueCount ; ++i )
							{
								values[i] = commandMessage.ReadF32();
							}

							static_cast<F32ArrayConsoleVariableInternal*>( pVariable )->SetValueT( values );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Double:
						{
							f64 value = commandMessage.ReadF64();
							
							static_cast<F64ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::DoubleArray:
						{
							size_t valueCount = commandMessage.ReadU32();
							std::vector<f64> values;
							values.resize(valueCount);
							for( size_t i = 0 ; i < valueCount ; ++i )
							{
								values[i] = commandMessage.ReadF64();
							}

							static_cast<F64ArrayConsoleVariableInternal*>( pVariable )->SetValueT( values );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::String:
						{
							String value = commandMessage.ReadString();

							static_cast<StringConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::StringArray:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
						break;

					case DataType::Vector2:
						{
							Vector2 value;
							value.X = commandMessage.ReadF32();
							value.Y = commandMessage.ReadF32();

							static_cast<Vector2ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Vector2Array:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
						break;
					case DataType::Vector3:
						{
							Vector3 value;
							value.X = commandMessage.ReadF32();
							value.Y = commandMessage.ReadF32();
							value.Z = commandMessage.ReadF32();

							static_cast<Vector3ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Vector3Array:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
						break;

					case DataType::Vector4:
						{
							Vector4 value;
							value.X = commandMessage.ReadF32();
							value.Y = commandMessage.ReadF32();
							value.Z = commandMessage.ReadF32();
							value.W = commandMessage.ReadF32();

							static_cast<Vector4ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;

					case DataType::Vector4Array:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
						break;

					case DataType::Matrix:
						{
							Matrix4 value;
							value.Row1.X = commandMessage.ReadF32();
							value.Row1.Y = commandMessage.ReadF32();
							value.Row1.Z = commandMessage.ReadF32();
							value.Row1.W = commandMessage.ReadF32();

							value.Row2.X = commandMessage.ReadF32();
							value.Row2.Y = commandMessage.ReadF32();
							value.Row2.Z = commandMessage.ReadF32();
							value.Row2.W = commandMessage.ReadF32();

							value.Row3.X = commandMessage.ReadF32();
							value.Row3.Y = commandMessage.ReadF32();
							value.Row3.Z = commandMessage.ReadF32();
							value.Row3.W = commandMessage.ReadF32();

							value.Row4.X = commandMessage.ReadF32();
							value.Row4.Y = commandMessage.ReadF32();
							value.Row4.Z = commandMessage.ReadF32();
							value.Row4.W = commandMessage.ReadF32();

							static_cast<Matrix4ConsoleVariableInternal*>( pVariable )->SetValueT( value );

							outputResult.WriteBool( true );
						}
						break;
					case DataType::MatrixArray:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
						break;

					default:
						{
							Assert( false );
							outputResult.WriteBool( false );
						}
					}
				}
				else
				{
					outputResult.WriteBool( false );
				}
			}
			break;

		case ConsoleMessageType::UserCommand:
			{
				s64 userCommandID = commandMessage.ReadS64();
				String inputString = commandMessage.ReadString();
				inputString.Trim();

				String commandStr = "";
				String parameterStr = "";

				s32 index = inputString.Find( L" " );
				if( index >= 0 )
				{
					commandStr = inputString.SubString( 0, index );
					parameterStr = inputString.SubString( index + 1 );
				}
				else
				{
					commandStr = inputString;
				}

				outputResult.WriteU16( ConsoleMessageType::UserCommand );
				outputResult.WriteS64( userCommandID );
				for( u32 i = 0; i < m_CommandProcessors.size(); ++i )
				{
					MessageStream outputStream;
					if( m_CommandProcessors[ i ]->OnCommand( commandStr, parameterStr, outputStream ) )
					{
						outputResult.WriteBool( true );
						outputResult.Write( outputStream.GetPayload(), outputStream.GetPayloadLength() );

						return;
					}
				}

				outputResult.WriteBool( false );
			}
			break;
		}
	}

	void ConsoleManager::Impl::WriteToStream( IConsoleVariable* pVariable, MessageStream& outputStream ) 
	{
		if( pVariable == NULL )
		{
			return;
		}

		String name = pVariable->GetName();
		outputStream.WriteString( name );

		DataType::Type type = pVariable->GetType();
		outputStream.WriteU8( static_cast<u8>( type ) );

		switch( type )
		{
		case DataType::Bool:
			{
				outputStream.WriteBool( static_cast<BoolConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::BoolArray:
			{
				const std::vector<bool>& values = static_cast<BoolArrayConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteU32( values.size() );
				for( size_t i = 0 ; i < values.size() ; ++i )
				{
					outputStream.WriteBool( values[i] );
				}
			}
			break;

		case DataType::Int:
			{
				outputStream.WriteS32( static_cast<S32ConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::IntArray:
			{
				const std::vector<s32>& values = static_cast<S32ArrayConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteU32( values.size() );
				for( size_t i = 0 ; i < values.size() ; ++i )
				{
					outputStream.WriteS32( values[i] );
				}
			}
			break;

		case DataType::UInt:
			{
				outputStream.WriteU32( static_cast<U32ConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::UIntArray:
			{
				const std::vector< u32 >& values = static_cast<U32ArrayConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteU32( values.size() );
				for( size_t i = 0 ; i < values.size() ; ++i )
				{
					outputStream.WriteU32( values[i] );
				}
			}
			break;

		case DataType::Float:
			{
				outputStream.WriteF32( static_cast<F32ConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::FloatArray:
			{
				const std::vector< f32 >& values = static_cast<F32ArrayConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteU32( values.size() );
				for( size_t i = 0 ; i < values.size() ; ++i )
				{
					outputStream.WriteF32( values[i] );
				}
			}
			break;

		case DataType::Double:
			{
				outputStream.WriteF64( static_cast<F64ConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::DoubleArray:
			{
				const std::vector< f64 > values = static_cast<F64ArrayConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteU32( values.size() );
				for( size_t i= 0 ; i < values.size() ; ++i )
				{
					outputStream.WriteF64( values[i] );
				}
			}
			break;

		case DataType::String:
			{
				outputStream.WriteString( static_cast<StringConsoleVariableInternal*>( pVariable )->GetValueT() );
			}
			break;

		case DataType::StringArray:
			{
				Assert( false );
			}
			break;
		case DataType::Vector2:
			{
				const Vector2& value = static_cast<Vector2ConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteF32( value.X );
				outputStream.WriteF32( value.Y );
			}
			break;

		case DataType::Vector2Array:
			{
				Assert( false );
			}
			break;

		case DataType::Vector3:
			{
				const Vector3& value = static_cast<Vector3ConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteF32( value.X );
				outputStream.WriteF32( value.Y );
				outputStream.WriteF32( value.Z );
			}
			break;

		case DataType::Vector3Array:
			{
				Assert( false );
			}
			break;

		case DataType::Vector4:
			{
				const Vector4& value = static_cast<Vector4ConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteF32( value.X );
				outputStream.WriteF32( value.Y );
				outputStream.WriteF32( value.Z );
				outputStream.WriteF32( value.W );
			}
			break;

		case DataType::Vector4Array:
			{
				Assert( false );
			}
			break;

		case DataType::Matrix:
			{
				const Matrix4& value = static_cast<Matrix4ConsoleVariableInternal*>( pVariable )->GetValueT();
				outputStream.WriteF32( value.Row1.X );
				outputStream.WriteF32( value.Row1.Y );
				outputStream.WriteF32( value.Row1.Z );
				outputStream.WriteF32( value.Row1.W );

				outputStream.WriteF32( value.Row2.X );
				outputStream.WriteF32( value.Row2.Y );
				outputStream.WriteF32( value.Row2.Z );
				outputStream.WriteF32( value.Row2.W );

				outputStream.WriteF32( value.Row3.X );
				outputStream.WriteF32( value.Row3.Y );
				outputStream.WriteF32( value.Row3.Z );
				outputStream.WriteF32( value.Row3.W );

				outputStream.WriteF32( value.Row4.X );
				outputStream.WriteF32( value.Row4.Y );
				outputStream.WriteF32( value.Row4.Z );
				outputStream.WriteF32( value.Row4.W );
			}
			break;

		case DataType::MatrixArray:
			{
				Assert( false );
			}
			break;

		default:
			Assert( false );
			break;
		}
	}

	void ConsoleManager::Impl::ProcessCommandList( const MessageStream& command, MessageStream& outputStream )
	{
		bool bTypeProxy = command.ReadBool();

		outputStream.WriteU16( ConsoleMessageType::List );
		outputStream.WriteBool( bTypeProxy );

		ConsoleVariableSet::ValueMapType result;

		if( m_pConsoleVariableSet != NULL 
			&& !bTypeProxy )
		{
			m_pConsoleVariableSet->EnumerateAll( result );
		}
		
		{
			std::map<String, ConsoleVariableInfo*>::const_iterator it = m_variables.begin();
			std::map<String, ConsoleVariableInfo*>::const_iterator endit = m_variables.end();
			while( it != endit )
			{
				if( it->second->m_bTypeProxy == bTypeProxy )
				{
					if( result.count( it->second->m_pVariable->GetName() ) <= 0 )
					{
						result.insert( std::make_pair( it->second->m_pVariable->GetName(), it->second->m_pVariable ) );
					}					
				}

				++it;
			}
		}		

		outputStream.WriteU32( result.size() );

		{
			ConsoleVariableSet::ValueMapType::iterator it = result.begin();
			ConsoleVariableSet::ValueMapType::iterator endit = result.end();

			while( it != endit )
			{
				IConsoleVariable* pVariable = it->second;
				WriteToStream( pVariable, outputStream );

				++it;
			}
		}
	}

	void ConsoleManager::Impl::ProcessCommandClear( const MessageStream& /*command*/, MessageStream& outputStream )
	{
		RemoveAll( false );

		outputStream.WriteU16( ConsoleMessageType::Clear );
		outputStream.WriteBool( true );
	}

	void ConsoleManager::Impl::ProcessCommandRemove( const MessageStream& command, MessageStream& outputStream )
	{
		String variableName = command.ReadString();
		ConsoleVariableInfo* pVariableInfo = Find( variableName );

		outputStream.WriteU16( ConsoleMessageType::Remove );
		outputStream.WriteString( variableName );
		
		if( pVariableInfo != NULL )
		{
			Remove( variableName );
		}

		outputStream.WriteBool( pVariableInfo != NULL );
	}

	void ConsoleManager::Impl::ProcessCommandReset( const MessageStream& command, MessageStream& outputStream )
	{
		String variableName = command.ReadString();
		ConsoleVariableInfo* pVariableInfo = Find( variableName );

		outputStream.WriteU16( ConsoleMessageType::Reset );
		outputStream.WriteString( variableName );

		if( pVariableInfo != NULL )
		{
			pVariableInfo->m_pVariable->ResetValue();
		}

		outputStream.WriteBool( pVariableInfo != NULL );
	}
#endif

	void ConsoleManager::Impl::RegisterCommandProcessor( ConsoleManager::ICommandProcessor* pProcessor )
	{
		Assert( pProcessor != NULL );

		if( std::find( m_CommandProcessors.begin(), m_CommandProcessors.end(), pProcessor ) == m_CommandProcessors.end() )
		{
			m_CommandProcessors.push_back( pProcessor );
		}
	}

	void ConsoleManager::Impl::UnregisterCommandProcessor( ConsoleManager::ICommandProcessor* pProcessor )
	{
		Assert( pProcessor != NULL );

		std::vector<ConsoleManager::ICommandProcessor*>::iterator it = std::find( m_CommandProcessors.begin(), m_CommandProcessors.end(), pProcessor );
		if( it != m_CommandProcessors.end() )
		{
			m_CommandProcessors.erase( it );
		}
	}

	bool ConsoleManager::Impl::IsValidVariableName( const String& name )
	{
		if( name.GetLength() == 0 ) { return false; }

		if( name[0] >= L'0' && name[0] <= L'9' ) { return false; }

		u32 length = name.GetLength();
		for( u32 i = 0; i<length; ++i )
		{
			if( !(( name[ i ] >= L'0' && name[ i ] <= L'9' ) 
				|| ( name[ i ] >= L'a' && name[ i ] <= L'z' ) 
				|| ( name[ i ] >= L'A' && name[ i ] <= L'Z' ) 
				|| ( name[ i ] == L'_' ) 
				|| ( name[ i ] == L'@' ) ) ) 
			{
				return false;
			}
		}

		return true;
	}

	ConsoleManager::Impl::ConsoleVariableInfo* 
		ConsoleManager::Impl::AddInternal(
		IConsoleVariable* pVariable, 
		bool bStronglyTyped, 
		bool bTypeProxy )
	{
		ConsoleVariableInfo* pVariableInfo = Find( pVariable->GetName() );
		if( pVariableInfo ) 
		{
			if( pVariable == pVariableInfo->m_pVariable )
			{
				return pVariableInfo;
			}
			else
			{
				Remove( pVariable->GetName() ); 
			}
		}

		ConsoleVariableInfo* pNewVariableInfo = new ConsoleVariableInfo(pVariable, bStronglyTyped, bTypeProxy );
		m_variables[ pVariable->GetName() ] = pNewVariableInfo;

		return pNewVariableInfo;
	}

	ConsoleManager::Impl::ConsoleVariableInfo* 
		ConsoleManager::Impl::Add(
		const String& name, 
		const String& type, 
		bool bStronglyTyped )
	{
		IConsoleVariable* pProxy = FindTypeProxy( type );
		if( pProxy )
		{
			IConsoleVariable* pNewVariable = pProxy->CloneType( name );
			return AddInternal( pNewVariable, bStronglyTyped );
		}
		else
		{
			return static_cast<ConsoleVariableInfo*>(0);			
		}
	}

	ConsoleManager::Impl::ConsoleVariableInfo* 
		ConsoleManager::Impl::Add(
		const String& name, 
		const String& type, 
		const String& value, 
		bool bStronglyTyped )
	{
		ConsoleVariableInfo* pVariableInfo = Add( name, type, bStronglyTyped );
		if( pVariableInfo )
		{
			pVariableInfo->m_pVariable->SetValue( value );
		}

		return pVariableInfo;
	}

	template <typename T, typename TypeDesc>
	ConsoleManager::Impl::ConsoleVariableInfo* 
		ConsoleManager::Impl::AddT(
		const String& name, 
		const T& value, 
		bool bStronglyTyped )
	{
		ConsoleVariable<T, TypeDesc>* pVariable = 
			new ConsoleVariable<T, TypeDesc>( name );
		pVariable->SetValueT( value );
		return AddInternal( pVariable, bStronglyTyped);
	}

	void ConsoleManager::Impl::Remove( const String& name )
	{
		ConsoleVariableInfo* pVariableInfo = Find( name );
		if( pVariableInfo )
		{
			pVariableInfo->m_pVariable->Release();

			delete pVariableInfo;
			m_variables.erase( name );
		}
	}

	void ConsoleManager::Impl::RemoveAll( bool bRemoveTypeProxies )
	{
		if( bRemoveTypeProxies )
		{
			// clear all
			std::map<String, ConsoleVariableInfo*>::iterator it;
			for( it = m_variables.begin() ; it != m_variables.end() ; ++it )
			{
				if( it->second )
				{
					it->second->m_pVariable->Release();

					delete it->second;
				}
			}
			m_variables.clear();
		}
		else
		{
			std::vector<String> removeList;
			std::map<String, ConsoleVariableInfo*>::iterator it;
			for( it = m_variables.begin() ; it != m_variables.end() ; ++it )
			{
				if( it->second && (it->second->m_bTypeProxy == false) )
				{
					removeList.push_back( it->first );
				}
			}

			std::vector<String>::const_iterator itRemoveList;
			for( itRemoveList = removeList.begin() ; itRemoveList != removeList.end() ; ++itRemoveList )
			{
				Remove( *itRemoveList );
			}
		}
	}

	ConsoleManager::Impl::ConsoleVariableInfo* ConsoleManager::Impl::Find( const String& name )
	{
		std::map<String, ConsoleVariableInfo*>::const_iterator it = m_variables.find( name );

		if( it != m_variables.end() ) { return it->second; }
		else { return static_cast<ConsoleVariableInfo*>(0); }
	}

	template< typename T, typename TypeDesc >
	IConsoleVariable* ConsoleManager::Impl::SetValueT( const String& name, const T& value )
	{
		ConsoleVariableInfo* pVariableInfo = Find( name );
		if( pVariableInfo )
		{
			ConsoleVariable<T, TypeDesc>* pConv = 
				dynamic_cast<ConsoleVariable<T, TypeDesc>*>( pVariableInfo->m_pVariable );

			if( pConv )
			{
				pConv->SetValueT( value );
				return pVariableInfo->m_pVariable;
			}
			else
			{
				Remove( name );
			}
		}

		pVariableInfo = AddT<T, TypeDesc>( name, value );
		return pVariableInfo->m_pVariable;
	}

	template< typename T, typename TypeDesc >
	const T& ConsoleManager::Impl::GetValueT( const String& name )
	{
		ConsoleVariableInfo* pVariableInfo = Find( name );
		if( pVariableInfo )
		{
			ConsoleVariable<T, TypeDesc>* pConv = 
				dynamic_cast<ConsoleVariable<T, TypeDesc>*>( pVariableInfo->m_pVariable );

			if( pConv )
			{
				return pConv->GetValueT();
			}
		}

		return GetBasicTypeProxyValue<T>();
	}

	//String ConsoleManager::Impl::DetermineType( const String& name, const String& value )
	String ConsoleManager::Impl::DetermineType( const String&, const String& value )
	{
		String value0 = value;
		value0.Trim();
		u32 length = value0.GetLength();

		bool parenthesisEnclosed = ( (value0[0] == L'(') && (value0[ length-1] == L')') );

		if( ( value0[0] == L'\"' ) && ( value0[ length-1] == L'\"' ) )
		{
			return ConsoleBasicTypeDescString::GetTypeName();
		}

		StringTokenizer tokenizer( value0 );
		tokenizer.AddPunctuator( L' ' );
		tokenizer.AddPunctuator( L'\t' );
		tokenizer.AddPunctuator( L',' );
		tokenizer.AddPunctuator( L'(' );
		tokenizer.AddPunctuator( L')' );

		s32 tokenCount = tokenizer.GetTokenCount();
		std::vector<String> tokens;
		for( s32 i = 0 ; i < tokenCount ; ++i )
		{
			tokens.push_back( tokenizer.GetNext() );
		}

		if( tokenCount == 0 )
		{
			return ConsoleBasicTypeDescString::GetTypeName();
		}
		else if( tokenCount == 1 )
		{
			if( IsNumber(tokens[0]) )
			{
				if( tokens[0].IndexOf( L"." ) < 0 )
				{
					return ConsoleBasicTypeDescInt::GetTypeName();
				}
				else
				{
					return ConsoleBasicTypeDescFloat::GetTypeName();
				}
			}
			else
			{
				if( (tokens[0].EqualTo( L"true", false ) ) 
					|| (tokens[0].EqualTo( L"false", false ) ) )
				{
					// true or false
					return ConsoleBasicTypeDescBool::GetTypeName(); 
				}
				else
				{
					// other string
					return ConsoleBasicTypeDescString::GetTypeName();
				}
			}
		}
		else 
		{
			if( parenthesisEnclosed && ( tokenCount == 2 ) 
				&& IsNumber( tokens[0] ) && IsNumber( tokens[1] ) )
			{
				// Vector2
				return ConsoleBasicTypeDescVector2::GetTypeName();
			}
			else if( parenthesisEnclosed && ( tokenCount == 3 ) 
				&& IsNumber( tokens[0] ) && IsNumber( tokens[1] ) && IsNumber( tokens[2] ) )
			{
				// Vector3
				return ConsoleBasicTypeDescVector3::GetTypeName();
			}
			else if( parenthesisEnclosed && ( tokenCount == 4 ) 
				&& IsNumber( tokens[0] ) && IsNumber( tokens[1] ) 
				&& IsNumber( tokens[2] ) && IsNumber( tokens[3] ) )
			{
				// Vector4
				return ConsoleBasicTypeDescVector4::GetTypeName();
			}
			else
			{
				bool isIntArray = true;
				bool isBoolArray = true;
				bool isFloatArray = true;

				for( s32 i = 0 ; i < tokenCount ; ++i )
				{
					if( !IsNumber( tokens[ i ] ) )
					{
						isIntArray = false;
						isFloatArray = false;

						if( !tokens[ i ].EqualTo( L"true", false ) && !tokens[ i ].EqualTo( L"false", false ) )
						{
							isBoolArray = false;
						}
					}
					else
					{
						isBoolArray = false;

						if( tokens[ i ].IndexOf( L"." ) < 0 )
						{
							isFloatArray = false;
						}
					}
				}

				if( isIntArray )
				{
					return ConsoleBasicTypeDescIntArray::GetTypeName();
				}
				else if( isBoolArray )
				{
					return ConsoleBasicTypeDescBoolArray::GetTypeName();
				}
				else if( isFloatArray )
				{
					return ConsoleBasicTypeDescFloatArray::GetTypeName();
				}
				else
				{
					return ConsoleBasicTypeDescString::GetTypeName();
				}
			}
		}

		//return ConsoleBasicTypeDescString::GetTypeName();
	}

	bool ConsoleManager::Impl::IsNumber( const String& string )
	{
		u32 length = string.GetLength();
		s32 dotCount = 0;

		for( u32 i = 0 ; i < length ; ++i )
		{
			if( string[ i ] == L'.' ) { ++dotCount; }

			if( !(( string[ i ] == L'.' ) || ( string[ i ] >= L'0' && string[ i ] <= L'9' )) )
			{
				return false;
			}
		}

		return ( dotCount < 2 );
	}

	IConsoleVariable* ConsoleManager::Impl::FindTypeProxy( const String& typeName )
	{
		ConsoleVariableInfo* pVariableInfo = Find( String( L"@" ) + typeName );

		if( pVariableInfo && pVariableInfo->m_bTypeProxy )
		{
			return pVariableInfo->m_pVariable;
		}
		else
		{
			return static_cast<IConsoleVariable*>(0);
		}
	}

	void ConsoleManager::Impl::SetConsoleVariableFilePath( const String& filePath )
	{
		m_consoleVariableFilePath = filePath;
	}

	void ConsoleManager::Impl::SaveConsoleVariables()
	{
#if 0
		FileStream fileStream( m_consoleVariableFilePath, EAccess::Write, ECreation::CreateAlways, false );
		TextWriter textWriter( fileStream, Encoding::ANSI );
			
		std::map<String, ConsoleVariableInfo*>::const_iterator it;
		for( it = m_variables.begin(); it != m_variables.end(); ++it )
		{
			if( it->second->m_bTypeProxy )
			{				
				continue;
			}
			
			IConsoleVariable* variable = it->second->m_pVariable;			
			String name = variable->GetName();
			if( name.IsEmpty() ) { continue; }
			String type = variable->GetTypeName();
			String value = variable->GetValue();

			String line( StringFormatter( L"%s|%s|%s", name.GetBuffer(), type.GetBuffer(), value.GetBuffer() ) );
			textWriter.WriteLine( line );		
		}	
#endif
	}

	void ConsoleManager::Impl::LoadConsoleVariables()
	{
#if 0
		FileStream fileStream( m_consoleVariableFilePath, EAccess::Read, ECreation::OpenExisting, false );
				
		String variable;
		String name;
		String type;
		String value;

		TextReader textReader( fileStream, Encoding::ANSI );
		while( textReader.ReadLine( variable ) )
		{
			StringTokenizer propertyTokenizer( variable );
			propertyTokenizer.AddPunctuator( L'|' );
			propertyTokenizer.GetNext( name );
			propertyTokenizer.GetNext( type );
			propertyTokenizer.GetNext( value );

			Add( name, type, value, true );
		}		
#endif
	}

	void ConsoleManager::Impl::DeleteConsoleVariableFile()
	{
		::DeleteFile( m_consoleVariableFilePath.GetCString() );
	}
}