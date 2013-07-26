#pragma once

#include "IConsoleVariable.h"

namespace Tomato
{
	class TOMATO_API ConsoleManager
	{
	public:
		ConsoleManager();

		virtual ~ConsoleManager();

#ifndef _RELEASE
		void InitializeConsoleServer( s32 listeningPort );
		void UpdateConsoleServer();
#endif

		void SetConsoleVariableFilePath( const String& filePath );
		void SaveConsoleVariables();
		void LoadConsoleVariables();

		//void ProcessMessage( const MessageStream& commandMessage, MessageStream& outputReslut );

	public:
		class ICommandProcessor
		{
		public:
			//virtual bool OnCommand( const String& command, const String& parameters, MessageStream& outputStream ) = 0;
		};

		void RegisterCommandProcessor( ICommandProcessor* pProcessor );

		void UnregisterCommandProcessor( ICommandProcessor* pProcessor );

	public:
		void SetVariable( IConsoleVariable& variable, bool bAutoTypeRegistry = false );

		IConsoleVariable* GetVariable( const String& name ) const;

		void DeleteVariable( IConsoleVariable& variable );

		void RegisterTypeProxy( IConsoleVariable* pTypeProxy );

		String DetermineType( const String& name, const String& value );

		IConsoleVariable* SetInt( const String& name, const s32& value );
		IConsoleVariable* SetIntArray( const String& name, const std::vector<s32>& value );
		IConsoleVariable* SetUInt( const String& name, const u32& value );
		IConsoleVariable* SetUIntArray( const String& name, const std::vector<u32>& value );
		IConsoleVariable* SetBool( const String& name, const bool& value );
		IConsoleVariable* SetBoolArray( const String& name, const std::vector<bool>& value );
		IConsoleVariable* SetFloat( const String& name, const f32& value );
		IConsoleVariable* SetFloatArray( const String& name, const std::vector<f32>& value );
		IConsoleVariable* SetDouble( const String& name, const f64& value );
		IConsoleVariable* SetDoubleArray( const String& name, const std::vector<f64>& value );
		IConsoleVariable* SetString( const String& name, const String& value );
		IConsoleVariable* SetVector2( const String& name, const Vector2& value );
		IConsoleVariable* SetVector3( const String& name, const Vector3& value );
		IConsoleVariable* SetVector4( const String& name, const Vector4& value );
		IConsoleVariable* SetMatrix4( const String& name, const Matrix4& value );

		const s32& GetInt( const String& name ) const;
		const std::vector<s32>& GetIntArray( const String& name ) const;
		const u32& GetUInt( const String& name ) const;
		const std::vector<u32>& GetUIntArray( const String& name ) const;
		const bool& GetBool( const String& name ) const;
		const std::vector<bool>& GetBoolArray( const String& name ) const;
		const f32& GetFloat( const String& name ) const;
		const std::vector<f32>& GetFloatArray( const String& name ) const;
		const f64& GetDouble( const String& name ) const;
		const std::vector<f64>& GetDoubleArray( const String& name ) const;
		const String& GetString( const String& name) const;
		const Vector2& GetVector2( const String& name ) const;
		const Vector3& GetVector3( const String& name ) const;
		const Vector4& GetVector4( const String& name ) const;
		const Matrix4& GetMatrix4( const String& name ) const;

	private:
		class Impl;
		friend Impl;
		Impl* m_pImpl;

#ifndef _RELEASE
		enum
		{
			ConsoleServerPacketSize = 4096 * 4
		};

		//TcpSocketServer m_consoleServer;
		//MessageStream m_packetBuffer;

#endif
	};
}

#include "ConsoleVariable.h"
#include "BasicTypeDesc.h"
#include "ConsoleVariableProxy.h"
#include "BasicTypes.h" 
