#pragma once

namespace Tomato
{
	class IConsoleVariable
	{
	public:
		virtual u32 AddRef() = 0;

		virtual u32 Release() = 0;

		virtual String GetName() const = 0;

		virtual DataType::Type GetType() const = 0;

		virtual String GetTypeName() const = 0;

		virtual String ToString() const = 0;

		virtual void SetValue( const String& value ) = 0;

		virtual String GetValue() const = 0;

		virtual void ResetValue() = 0;

		virtual IConsoleVariable* Clone( const String& name ) const = 0;

		virtual IConsoleVariable* CloneType( const String& name ) const = 0;

		virtual void Dispose() = 0;

	protected: 
		virtual ~IConsoleVariable() = 0 { }
	};

}