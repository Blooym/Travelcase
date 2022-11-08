namespace Travelcase.Base
{
    /// <summary>
    ///     A collection of constants used throughout the plugin.
    /// </summary>
    internal sealed class PluginConstants
    {
        /// <summary>
        ///    This is the name that will be shown in all UI elements, does not change InternalName.
        /// </summary>
        internal const string PluginName = "Travelcase";

        /// <summary>
        ///     The support button URL.
        /// </summary>
        internal const string DonateButtonUrl = "https://github.com/sponsors/BitsOfAByte";

        /// <summary>
        ///     The path to the plugin's resources folder with trailing slashes, relative to the plugin assembly location with trailing slashes.
        /// </summary>
        internal static readonly string PluginResourcesDir = $"{PluginService.PluginInterface.AssemblyLocation.DirectoryName}\\Resources\\";

        /// <summary>
        ///     The path to the plugin's localization folder with trailing slashes.
        /// </summary>
        internal static readonly string PluginlocalizationDir = PluginResourcesDir + "Localization\\";
    }
}
