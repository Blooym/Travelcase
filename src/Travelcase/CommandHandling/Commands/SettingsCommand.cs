using Dalamud.Game.Command;
using Travelcase.Base;
using Travelcase.CommandHandling.Interfaces;
using Travelcase.Localization;
using Travelcase.UserInterface.Windows.Settings;

namespace Travelcase.CommandHandling.Commands
{
    internal sealed class SettingsCommand : IDalamudCommand
    {
        private const string CommandName = "/travelcase";

        /// <inheritdoc />
        public string Name { get; } = CommandName;

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute) { HelpMessage = TCommands.SettingsHelp };

        /// <inheritdoc />
        public CommandInfo.HandlerDelegate OnExecute => (command, args) =>
        {
            var config = Services.CharacterConfig.CurrentConfig;
            switch (command)
            {
                case CommandName when args == "toggle":
                    if (config != null)
                    { config.IsEnabled = !config.IsEnabled; config.Save(); }
                    break;
                case CommandName when args == "rp":
                    if (config != null)
                    { config.OnlyInRoleplayMode = !config.OnlyInRoleplayMode; config.Save(); }
                    break;
                case CommandName when args?.Length == 0:
                    Services.WindowingService.ToggleWindow<SettingsWindow>();
                    break;
            }
        };
    }
}