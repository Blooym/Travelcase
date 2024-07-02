using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Travelcase.Managers;

namespace Travelcase.Base
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class PluginService
    {
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ICommandManager Commands { get; private set; } = null!;
        [PluginService] internal static IClientState ClientState { get; private set; } = null!;
        [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
        [PluginService] internal static ICondition Condition { get; private set; } = null!;
        [PluginService] internal static IFramework Framework { get; private set; } = null!;
        [PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;
        [PluginService] internal static INotificationManager NotificationManager { get; private set; } = null!;

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
