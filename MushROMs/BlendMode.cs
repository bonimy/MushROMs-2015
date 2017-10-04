
namespace MushROMs
{
    /// <summary>
    /// Specifies constants defining color blend modes.
    /// </summary>
    public enum BlendMode
    {
        /// <summary>
        /// Multiply the layers.
        /// </summary>
        Multiply,
        /// <summary>
        /// Invert both layers, multiply them, then invert the result.
        /// </summary>
        Screen,
        /// <summary>
        /// Combine <see cref="Multiply"/> and <see cref="Screen"/>.
        /// </summary>
        Overlay,
        /// <summary>
        /// <see cref="Screen"/> with layers swapped.
        /// </summary>
        HardLight,
        /// <summary>
        /// A softer version of <see cref="HardLight"/>.
        /// </summary>
        SoftLight,
        /// <summary>
        /// Divide bottom layer by the inverted top layer.
        /// </summary>
        ColorDodge,
        /// <summary>
        /// Sum the layers.
        /// </summary>
        LinearDodge,
        /// <summary>
        /// Same as <see cref="ColorDodge"/>, but blending with white does not change the image.
        /// </summary>
        ColorBurn,
        /// <summary>
        /// Invert each layer, add them, then invert the result.
        /// </summary>
        LinearBurn,
        /// <summary>
        /// Combines <see cref="ColorDodge"/> and <see cref="ColorBurn"/>
        /// (rescaled so that neutral colors become middle gray).
        /// </summary>
        VividLight,
        /// <summary>
        /// Combines <see cref="LinearDodge"/> and <see cref="LinearBurn"/>
        /// (rescaled so that neutral colors become middle gray).
        /// </summary>
        LinearLight,
        /// <summary>
        /// Subtracts the larger color component with the smaller one.
        /// </summary>
        Difference,
        /// <summary>
        /// Retains the smallest components of each layer.
        /// </summary>
        Darken,
        /// <summary>
        /// Retains the largest components of each layer.
        /// </summary>
        Lighten
    }
}
