using Travelcase.Base;

namespace Travelcase.Localization
{
    /// <summary>
    ///     A collection translatable Command Help strings.
    /// </summary>
    internal sealed class TCommands
    {
        internal static string SettingsHelp => "Opens the Travelcase configuration window when no arguments are specified. '/travelcase toggle' to toggle the plugin, '/travelcase rp' to toggle roleplay mode.";
    }

    /// <summary>
    ///     A collection of translatable window strings.
    /// </summary>
    internal sealed class TWindowNames
    {
        internal static string Settings => string.Format("{0} - Settings", Constants.PluginName);
    }

    /// <summary>
    ///     A collection of translatable settings strings.
    /// </summary>
    internal sealed class TSettings
    {
        internal static string Enable => string.Format("Enable {0}", Constants.PluginName);
        internal static string OnlyInRoleplayMode => "Only While Roleplaying";
        internal static string Search => "Search";
        internal static string Zone => "Zone";
        internal static string Enabled => "Enabled";
        internal static string GearsetSlot => "Gearset Slot";
        internal static string GlamourPlate => "Glamour Plate";
        internal static string NoZoneFound => "No zones found matching your search query, or perhaps no zones were able to load.";
        internal static string LoginToUse => "Please login to configure the plugin.";
        internal static string Donate => "Donate";
    }
}
