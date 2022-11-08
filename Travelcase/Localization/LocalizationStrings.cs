using CheapLoc;
using Travelcase.Base;

namespace Travelcase.Localization
{
    /// <summary>
    ///     A collection translatable Command Help strings.
    /// </summary>
    internal sealed class TCommands
    {
        internal static string SettingsHelp => Loc.Localize("Commands.Settings.Help", "Opens the Travelcase configuration window when no arguments are specified, '/travelcase toggle' to toggle the plugin on/off, and '/travelcase rp' to toggle the plugin on/off in roleplay mode.");
    }

    /// <summary>
    ///     A collection of translatable window strings.
    /// </summary>
    internal sealed class TWindowNames
    {
        internal static string Settings => string.Format(Loc.Localize("Window.Settings", "{0} - Settings"), PluginConstants.PluginName);
    }

    /// <summary>
    ///     A collection of translatable settings strings.
    /// </summary>
    internal sealed class TSettings
    {
        internal static string Enable => string.Format(Loc.Localize("Settings.Enable", "Enable {0}"), PluginConstants.PluginName);
        internal static string OnlyInRoleplayMode => Loc.Localize("Settings.OnlyInRoleplayMode", "Only While Roleplaying");
        internal static string Search => Loc.Localize("Settings.Search", "Search");
        internal static string Zone => Loc.Localize("Settings.Zone", "Zone");
        internal static string Enabled => Loc.Localize("Settings.ZoneName", "Enabled");
        internal static string GearsetSlot => Loc.Localize("Settings.GearsetSlot", "Gearset Slot");
        internal static string GlamourPlate => Loc.Localize("Settings.GlamourSlot", "Glamour Plate");
        internal static string NoZoneFound => Loc.Localize("Settings.NoZoneFound", "No zones found matching your search query, or perhaps no zones were able to load.");
        internal static string LoginToUse => Loc.Localize("Settings.LoginToUse", "Please login to configure the plugin.");
        internal static string Donate => Loc.Localize("Settings.Donate", "Donate");
    }
}
