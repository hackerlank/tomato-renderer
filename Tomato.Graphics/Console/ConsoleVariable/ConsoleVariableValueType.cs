namespace Tomato.Graphics.Console
{
	/// <summary>
	/// Defines the type of console variable's value.
	/// </summary>
    public enum ConsoleVariableValueType
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

        Invalid = 0x7FFFFFFF
    }
}
