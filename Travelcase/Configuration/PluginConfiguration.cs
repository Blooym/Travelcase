using System.Collections.Generic;
using Dalamud.Configuration;

namespace Travelcase.Configuration
{
    internal sealed class PluginConfiguration : IPluginConfiguration
    {
        public int Version { get; set; }
        public Dictionary<ulong, CharacterConfiguration> CharacterConfigurations = [];

        public void Save() => Travelcase.PluginInterface.SavePluginConfig(this);
        public static PluginConfiguration Load() => Travelcase.PluginInterface.GetPluginConfig() as PluginConfiguration ?? new();
    }

    internal sealed class CharacterConfiguration
    {
        public int Version { get; set; }
        public bool PluginEnabled = true;
        public bool RoleplayOnly;
        public Dictionary<uint, Gearset> ZoneGearsets = [];
    }

    internal sealed class Gearset
    {
        public int GearsetNumber;
        public byte GlamourPlate;
        public bool Enabled;
    }
}
