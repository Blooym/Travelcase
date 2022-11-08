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
        public bool IsEnabled { get; set; } = true;
        public bool OnlyInRoleplayMode { get; set; }
        public Dictionary<uint, Gearset> GearsetBindings { get; set; } = new();

        public class Gearset
        {
            public int Number { get; set; }
            public byte GlamourPlate { get; set; }
            public bool Enabled { get; set; }
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
                    PluginLog.Warning("CharacterConfiguration(Save): No ContentID found, not able to save configuration.");
                    return;
                }

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
        ///     Sets 'this' to the configuration for the current character.
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
                return NewCharacterConfiguration();
            }

            try
            {
                var configObj = JsonConvert.DeserializeObject<CharacterConfiguration>(File.ReadAllText(configPath));
                if (configObj == null)
                {
                    return NewCharacterConfiguration();
                }

                return configObj;
            }
            catch (Exception e)
            {
                PluginLog.Error($"CharacterConfiguration(Load): Failed to load character configuration from {configPath}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        ///     Create a new configuration for the current character.
        /// </summary>
        private static CharacterConfiguration NewCharacterConfiguration()
        {
            var newCharacter = new CharacterConfiguration();

            newCharacter.Save();
            return newCharacter;
        }
    }
}
