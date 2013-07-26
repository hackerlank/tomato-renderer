#pragma once

namespace Tomato
{
	struct TOMATO_API DataType
	{
		enum Type
		{
			None,

			Bool,
			BoolArray,
			Int,
			IntArray,			
			UInt,
			UIntArray,
			Float,
			FloatArray,
			Double,
			DoubleArray,
			String,
			StringArray,
			Vector2,
			Vector2Array,
			Vector3,
			Vector3Array,
			Vector4,
			Vector4Array,
			Matrix,
			MatrixArray,

			FORCEDWORD = 0x7FFFFFFF
		};
	};
}