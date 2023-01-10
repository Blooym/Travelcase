using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.IoC;
using Dalamud.Plugin;
using Travelcase.Managers;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class PluginService
    {
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; } = null!;
        [PluginService] internal static ClientState ClientState { get; private set; } = null!;
        [PluginService] internal static DataManager DataManager { get; private set; } = null!;
        [PluginService] internal static Condition Condition { get; private set; } = null!;
        [PluginService] internal static Framework Framework { get; private set; } = null!;

        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static CharacterConfigManager CharacterConfig { get; private set; } = null!;
        internal static ResourceManager Resources { get; private set; } = null!;
        internal static GearsetManager GearsetManager { get; private set; } = null!;

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize()
        {
            Resources = new ResourceManager();
            WindowManager = new WindowManager();
            CommandManager = new CommandManager();
            CharacterConfig = new CharacterConfigManager();
            GearsetManager = new GearsetManager();
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            WindowManager.Dispose();
            CommandManager.Dispose();
            CharacterConfig.Dispose();
            GearsetManager.Dispose();
            Resources.Dispose();
        }
    }
}
