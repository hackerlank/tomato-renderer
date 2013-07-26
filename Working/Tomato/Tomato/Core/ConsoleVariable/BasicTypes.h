#pragma once

namespace Tomato
{
	typedef 
		ConsoleVariableProxy<s32, ConsoleBasicTypeDescInt> 
		IntConsoleVariable;

	typedef 
		ConsoleVariableProxy<u32, ConsoleBasicTypeDescUInt> 
		UIntConsoleVariable;

	typedef 
		ConsoleVariableProxy<f32, ConsoleBasicTypeDescFloat> 
		FloatConsoleVariable;

	typedef 
		ConsoleVariableProxy<f64, ConsoleBasicTypeDescDouble> 
		DoubleConsoleVariable;

	typedef 
		ConsoleVariableProxy<bool, ConsoleBasicTypeDescBool> 
		BoolConsoleVariable;

	typedef 
		ConsoleVariableProxy<std::vector<s32>, ConsoleBasicTypeDescIntArray> 
		IntArrayConsoleVariable;

	typedef 
		ConsoleVariableProxy<std::vector<u32>, ConsoleBasicTypeDescUIntArray> 
		UIntArrayConsoleVariable;

	typedef 
		ConsoleVariableProxy<std::vector<f32>, ConsoleBasicTypeDescFloatArray> 
		FloatArrayConsoleVariable;

	typedef 
		ConsoleVariableProxy<std::vector<f64>, ConsoleBasicTypeDescDoubleArray> 
		DoubleArrayConsoleVariable;

	typedef 
		ConsoleVariableProxy<std::vector<bool>, ConsoleBasicTypeDescBoolArray> 
		BoolArrayConsoleVariable;

	typedef 
		ConsoleVariableProxy<String, ConsoleBasicTypeDescString> 
		StringConsoleVariable;

	typedef 
		ConsoleVariableProxy<Vector2, ConsoleBasicTypeDescVector2> 
		Vector2ConsoleVariable;

	typedef 
		ConsoleVariableProxy<Vector3, ConsoleBasicTypeDescVector3> 
		Vector3ConsoleVariable;

	typedef 
		ConsoleVariableProxy<Vector4, ConsoleBasicTypeDescVector4> 
		Vector4ConsoleVariable;

	typedef 
		ConsoleVariableProxy<Matrix4, ConsoleBasicTypeDescMatrix4> 
		Matrix4ConsoleVariable;
}
