#pragma once

// TOMATO_API
#ifdef TOMATO_DLL_EXPORT
	#define TOMATO_API __declspec(dllexport)
#else
	#ifdef TOMATO_DLL_IMPORT
		#define TOMATO_API __declspec(dllimport)
	#else
		#define TOMATO_API
	#endif
#endif

// DELETE_AND_SET_NULL
#ifndef DELETE_AND_SET_NULL
#define DELETE_AND_SET_NULL(p)	{ if( (p) != NULL ) { delete (p); (p) = NULL; } }
#endif

// DELETE_ARRAY_AND_SET_NULL
#ifndef DELETE_ARRAY_AND_SET_NULL
#define DELETE_ARRAY_AND_SET_NULL(p)	{ if( (p) != NULL ) { delete[] (p); (p) = NULL; } }
#endif

// WARNING macro
#define _QUOTE(x)		# x
#define QUOTE(x)		_QUOTE(x)
#define __FILE__LINE__ __FILE__ "(" QUOTE(__LINE__) ") : "
#define FILE_LINE message( __FILE__LINE__ )
#define WARNING(x)		message( __FILE__LINE__"warning C9999:  " #x )

// Primitive Types
//
//	Integer types: 
//		Signed: s8, s16, s32, s64
//		Unsigned: u8, u16, u32, u64
//
//	Character types: char, wchar
//
//	Floating-point types: f32, f64
//
//	Other types: bool, byte
//
namespace Tomato
{
#ifdef WIN32
	typedef char s8;
	typedef short s6;
	typedef int s32;
	typedef __int64 s64;

	typedef unsigned char u8;
	typedef unsigned short u16;
	typedef unsigned int u32;
	typedef unsigned __int64 u64;

	//typedef bool bool;
	//typedef char char;
	typedef wchar_t wchar;
	typedef unsigned char byte;

	typedef float f32;
	typedef double f64;
#else
	// other platforms
#endif
}
