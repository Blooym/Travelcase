using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Logging;
using Newtonsoft.Json;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to and determines the per-character Plugin configuration.
    /// </summary>
    internal sealed class CharacterConfiguration
    {
        /// <summary>
        ///     The current configuration version, incremented on breaking changes.
        /// </summary>
        public int Version { get; set; }
        public string? CharacterName { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool OnlyInRoleplayMode { get; set; }
        public Dictionary<uint, Gearset> GearsetBindings { get; set; } = new();

        public class Gearset
        {
            public string Name { get; set; } = string.Empty;
            public int GearsetNumber { get; set; }
            public byte GlamourPlate { get; set; }
            public bool Enabled { get; set; }
            public DateTime GeneratedOn { get; set; } = DateTime.Now;
        }

        private static string GetConfigDir() => PluginService.PluginInterface.GetPluginConfigDirectory();
        private static string UserConfigPath(ulong contentId) => Path.Combine(GetConfigDir(), $"{contentId}.json");

        /// <summary>
        ///     Saves the current configuration (and any modifications) as a file for this character.
        /// </summary>
        public void Save()
        {
            try
            {
                var contentId = PluginService.ClientState.LocalContentId;
                if (contentId == 0)
                {
                    PluginLog.Warning("CharacterConfiguration(Save): No ContentID found, not able to save user configuration.");
                    return;
                }

                this.CharacterName = PluginService.ClientState.LocalPlayer?.Name.TextValue;

                Directory.CreateDirectory(GetConfigDir());
                var configObj = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(UserConfigPath(contentId), configObj);
            }
            catch (Exception e)
            {
                PluginLog.Error($"CharacterConfiguration(Save): Failed to save character configuration to file: {e}");
            }
        }

        /// <summary>
        ///     Saves the current configuration (and any modifications) as a file for the ContentID provided.
        /// </summary>
        /// <param name="contentId">The ContentID to save the configuration for.</param>
        public void Save(ulong contentId)
        {
            try
            {
                this.CharacterName = PluginService.ClientState.LocalPlayer?.Name.TextValue;

                Directory.CreateDirectory(GetConfigDir());
                var configObj = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(UserConfigPath(contentId), configObj);
            }
            catch (Exception e)
            {
                PluginLog.Error($"CharacterConfiguration(Save): Failed to save character configuration to file: {e}");
            }
        }

        /// <summary>
        ///     Returns the configuration for the current character.
        /// </summary>
        public static CharacterConfiguration? Load()
        {
            var contentID = PluginService.ClientState.LocalContentId;
            if (contentID == 0)
            {
                PluginLog.Warning("CharacterConfiguration(Load): No ContentID found, not able to load configuration.");
                return null;
            }

            var configPath = UserConfigPath(contentID);
            if (!File.Exists(configPath))
            {
                return new();
            }

            try
            {
                var configObj = JsonConvert.DeserializeObject<CharacterConfiguration>(File.ReadAllText(configPath));
                return configObj ?? (CharacterConfiguration?)(new());
            }
            catch (Exception e)
            {
                PluginLog.Error($"CharacterConfiguration(Load): Failed to load character configuration from {configPath}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Loads the configuration for the ContentID provided.
        /// </summary>
        /// <param name="contentId"></param>
        public static CharacterConfiguration? Load(ulong contentId)
        {
            var configPath = UserConfigPath(contentId);
            if (!File.Exists(configPath))
            {
                return new();
            }

            try
            {
                return JsonConvert.DeserializeObject<CharacterConfiguration>(File.ReadAllText(configPath));
            }
            catch (Exception e)
            {
                PluginLog.Error($"CharacterConfiguration(Load): Failed to load character configuration from {configPath}: {e.Message}");
                return null;
            }
        }
    }
}
