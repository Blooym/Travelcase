using Travelcase.CommandHandling;
using Travelcase.Managers;
using Travelcase.UserInterface;

namespace Travelcase.Base
{
    internal sealed class Services
    {
        internal static CommandManager CommandManager { get; private set; } = null!;
        internal static WindowingService WindowingService { get; private set; } = null!;
        internal static CharacterConfigManager CharacterConfig { get; private set; } = null!;
        internal static GearsetManager GearsetManager { get; private set; } = null!;

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize()
        {
            WindowingService = new WindowingService();
            CommandManager = new CommandManager();
            CharacterConfig = new CharacterConfigManager();
            GearsetManager = new GearsetManager();
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose()
        {
            WindowingService.Dispose();
            CommandManager.Dispose();
            CharacterConfig.Dispose();
            GearsetManager.Dispose();
        }
    }
}