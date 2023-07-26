using Dalamud.Game.Command;

namespace Travelcase.CommandHandling.Interfaces
{
    /// <summary>
    ///     Represents a command.
    /// </summary>
    internal interface IDalamudCommand
    {
        /// <summary>
        ///     The name of the command (including the /)
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The command info.
        /// </summary>
        CommandInfo Command { get; }

        /// <summary>
        ///     The command's execution handler.
        /// </summary>
        CommandInfo.HandlerDelegate OnExecute { get; }
    }
}