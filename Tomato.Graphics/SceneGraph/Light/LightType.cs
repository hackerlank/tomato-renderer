namespace Tomato.Graphics
{
	/// <summary>
	/// Defines attenuation type of the light.
	/// </summary>
	public enum LightType
	{
		/// <summary>
		/// Infinite type of light does not attenuate at all.
		/// To simulate directional light, Infinite type should be used.
		/// </summary>
		Directional,

		/// <summary>
		/// Light does attenuate over the distance from the object.
		/// </summary>
		Point,

		/// <summary>
		/// Light does attenuate over the distance from the object and the angle from the direction.
		/// </summary>
		Spot
	}
}