using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Gui.Toast;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using Travelcase.IPC;
using Travelcase.Managers;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
#pragma warning disable CS8618 // Injection is handled by the Dalamud Plugin Framework here.
    internal sealed class PluginService
    {
        [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static Dalamud.Game.Command.CommandManager Commands { get; private set; }
        [PluginService] internal static ClientState ClientState { get; private set; }
        [PluginService] internal static DataManager DataManager { get; private set; }
        [PluginService] internal static Condition Condition { get; private set; }
        [PluginService] internal static ToastGui ToastGui { get; private set; }

        internal static CommandManager CommandManager { get; private set; }
        internal static WindowManager WindowManager { get; private set; }
        internal static CharacterConfigManager CharacterConfig { get; private set; }
        internal static ResourceManager Resources { get; private set; }
        internal static GearsetManager GearsetManager { get; private set; }
        internal static IPCLoader IPC { get; private set; }

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
            IPC = new IPCLoader();

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
            IPC.Dispose();

            PluginLog.Debug("PluginService(Initialize): Successfully disposed of plugin services.");
        }
    }
}
