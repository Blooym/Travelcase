using System;
using Dalamud.Game.Command;
using Dalamud.Logging;
using Travelcase.Base;
using Travelcase.Localization;
using Travelcase.UI.Windows.Settings;

namespace Travelcase.Managers
{
    /// <summary>
    ///     Initializes and manages all commands and command-events for the plugin.
    /// </summary>
    public sealed class CommandManager : IDisposable
    {
        private const string SettingsCommand = "/travelcase";

        /// <summary>
        ///     Initializes the CommandManager and its resources.
        /// </summary>
        public CommandManager()
        {
            PluginLog.Debug("CommandManager(Constructor): Initializing...");

            PluginService.Commands.AddHandler(SettingsCommand, new CommandInfo(this.OnCommand) { HelpMessage = TCommands.SettingsHelp, ShowInHelp = true });

            PluginLog.Debug("CommandManager(Constructor): Initialization complete.");
        }

        /// <summary>
        ///     Dispose of the PluginCommandManager and its resources.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("CommandManager(Dispose): Disposing...");

            PluginService.Commands.RemoveHandler(SettingsCommand);

            PluginLog.Debug("CommandManager(Dispose): Successfully disposed.");
        }

        /// <summary>
        ///     Event handler for when a command is issued by the user.
        /// </summary>
        private void OnCommand(string command, string args)
        {
            var windowSystem = PluginService.WindowManager.WindowSystem;
            var config = PluginService.CharacterConfig.CurrentConfig;
            switch (command)
            {
                case SettingsCommand when args == "toggle":
                    config = PluginService.CharacterConfig?.CurrentConfig;
                    if (config != null)
                    { config.IsEnabled = !config.IsEnabled; config.SaveForCharacter(); }
                    break;
                case SettingsCommand when args == "rp":
                    config = PluginService.CharacterConfig?.CurrentConfig;
                    if (config != null)
                    { config.OnlyInRoleplayMode = !config.OnlyInRoleplayMode; config.SaveForCharacter(); }
                    break;
                case SettingsCommand when args == string.Empty:
                    if (windowSystem.GetWindow(TWindowNames.Settings) is SettingsWindow settingsWindow)
                    {
                        settingsWindow.IsOpen = !settingsWindow.IsOpen;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
