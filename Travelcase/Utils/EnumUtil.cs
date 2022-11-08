namespace Travelcase.Utils
{
    public static class EnumUtil
    {
        public static T ToEnum<T>(this byte value) where T : struct => (T)(object)value;
    }
}
