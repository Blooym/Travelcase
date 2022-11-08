using System;
using Dalamud.Logging;
using Travelcase.Base;

namespace Travelcase.Managers
{
    /// <summary>
    ///     CharacterConfigManager is responsible for managing the per-character configuration.
    /// </summary>
    internal class CharacterConfigManager : IDisposable
    {
        /// <summary>
        ///     The current configuration loaded for the character.
        /// </summary>
        internal CharacterConfiguration? CurrentConfig { get; private set; }

        /// <summary>
        ///     Initializes the CharacterConfigManager and loads the configuration for the current character.
        /// </summary>
        internal CharacterConfigManager()
        {
            PluginService.ClientState.Login += this.OnLogin;
            PluginService.ClientState.Logout += this.OnLogout;

            if (PluginService.ClientState?.LocalPlayer != null)
            {
                PluginLog.Debug("CharacterConfigManager(Initialize): Player is already logged in, loading config.");
                this.CurrentConfig = LoadConfig();
            }
        }

        /// <summary>
        ///     Disposes of the CharacterConfigManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginService.ClientState.Login -= this.OnLogin;
            PluginService.ClientState.Logout -= this.OnLogout;
            this.CurrentConfig = null;
        }

        /// <summary>
        ///     Handles the login event and loads the configuration for the current character.
        /// </summary>
        private void OnLogin(object? e, EventArgs args)
        {
            PluginLog.Debug("CharacterConfigManager(OnLogin): Player logged in, loading their config.");
            this.CurrentConfig = LoadConfig();
        }

        private static CharacterConfiguration? LoadConfig()
        {
            PluginLog.Debug("CharacterConfigManager(LoadConfig): Loading configuration");
            return CharacterConfiguration.Load() ?? null;
        }

        /// <summary>
        ///     Handles the logout event and unloads the configuration for the current character.
        /// </summary>
        private void OnLogout(object? e, EventArgs args)
        {
            PluginLog.Debug("CharacterConfigManager(OnLogout): Player logged out, unloading their config.");
            this.CurrentConfig = null;
        }
    }
}
