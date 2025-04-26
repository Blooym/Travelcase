using System;
using Dalamud.Game.Command;

namespace Travelcase.Command
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
        public CommandManager() => Travelcase.Commands.AddHandler(SettingsCommand, new CommandInfo(this.OnCommand)
        {
            HelpMessage = "Opens the Travelcase configuration window when no arguments are specified. '/travelcase toggle' to toggle the plugin, '/travelcase rp' to toggle roleplay mode.",
            ShowInHelp = true
        });

        /// <summary>
        ///     Dispose of the PluginCommandManager and its resources.
        /// </summary>
        public void Dispose() => Travelcase.Commands.RemoveHandler(SettingsCommand);

        /// <summary>
        ///     Event handler for when a command is issued by the user.
        /// </summary>
        /// <param name="command">The command that was issued.</param>
        /// <param name="args">The arguments that were passed with the command.</param>
        ///
        private void OnCommand(string command, string args)
        {
            var hasConfig = Travelcase.PluginConfiguration.CharacterConfigurations.TryGetValue(Travelcase.ClientState.LocalContentId, out var config);
            if (!hasConfig || config is null)
            {
                return;
            }

            switch (command)
            {
                case SettingsCommand when args == "toggle":
                    if (config != null)
                    { config.PluginEnabled = !config.PluginEnabled; Travelcase.PluginConfiguration.Save(); }
                    break;
                case SettingsCommand when args == "rp":
                    if (config != null)
                    { config.RoleplayOnly = !config.RoleplayOnly; Travelcase.PluginConfiguration.Save(); }
                    break;
                case SettingsCommand when args?.Length == 0:
                    Travelcase.WindowManager.ToggleConfigWindow();
                    break;
            }
        }
    }
}
