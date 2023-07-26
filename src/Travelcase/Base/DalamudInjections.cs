using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class DalamudInjections
    {
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; } = null!;
        [PluginService] internal static ClientState ClientState { get; private set; } = null!;
        [PluginService] internal static DataManager DataManager { get; private set; } = null!;
        [PluginService] internal static Condition Condition { get; private set; } = null!;
        [PluginService] internal static Framework Framework { get; private set; } = null!;
    }
}
