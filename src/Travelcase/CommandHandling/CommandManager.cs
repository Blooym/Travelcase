using System;
using Travelcase.Base;
using Travelcase.CommandHandling.Commands;
using Travelcase.CommandHandling.Interfaces;

namespace Travelcase.CommandHandling
{
    internal sealed class CommandManager : IDisposable
    {
        /// <summary>
        ///     All  commands to register with the <see cref="Dalamud.Game.Command.CommandManager" />, holds all references.
        /// </summary>
        private IDalamudCommand[] commands = { new SettingsCommand() };

        private bool disposedValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager" /> class.
        /// </summary>
        public CommandManager()
        {
            foreach (var command in this.commands)
            {
                DalamudInjections.Commands.AddHandler(command.Name, command.Command);
            }
        }

        /// <summary>
        ///     Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                foreach (var command in this.commands)
                {
                    DalamudInjections.Commands.RemoveHandler(command.Name);
                }
                this.commands = Array.Empty<IDalamudCommand>();

                this.disposedValue = true;
            }
        }
    }
}