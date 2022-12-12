namespace Travelcase.Utils
{
    public static class EnumUtil
    {
        /// <summary>
        ///     Converts a byte to an enum.
        /// </summary>
        public static T ToEnum<T>(this byte value) where T : struct => (T)(object)value;
    }
}
