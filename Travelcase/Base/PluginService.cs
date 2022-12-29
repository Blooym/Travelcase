using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using Travelcase.Managers;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class PluginService
    {
#pragma warning disable CS8618, RCS1170 // Injection is handled by the Dalamud Plugin Framework here.
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
        [PluginService] internal static ClientState ClientState { get; private set; }
        [PluginService] internal static DataManager DataManager { get; private set; }
        [PluginService] internal static Condition Condition { get; private set; }
        [PluginService] internal static Framework Framework { get; private set; }

        internal static CommandManager CommandManager { get; private set; }
        internal static WindowManager WindowManager { get; private set; }
        internal static CharacterConfigManager CharacterConfig { get; private set; }
        internal static ResourceManager Resources { get; private set; }
        internal static GearsetManager GearsetManager { get; private set; }
#pragma warning restore CS8618, RCS1170

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

            PluginLog.Debug("PluginService(Initialize): Successfully initialized plugin services.");
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

            PluginLog.Debug("PluginService(Initialize): Successfully disposed of plugin services.");
        }
    }
}
