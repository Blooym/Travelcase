using Dalamud.Plugin;
using Sirensong;
using Travelcase.Base;

namespace Travelcase
{
    public sealed class TravelcasePlugin : IDalamudPlugin
    {
        /// <summary>
        ///     The plugin name.
        /// </summary>
        public string Name { get; } = Constants.PluginName;

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        /// <param name="pluginInterface"></param>
        public TravelcasePlugin(DalamudPluginInterface pluginInterface)
        {
            SirenCore.Initialize(pluginInterface, this.Name);
            pluginInterface.Create<DalamudInjections>();
            Services.Initialize();
        }

        /// <summary>
        ///     Disposes of the plugin.
        /// </summary>
        public void Dispose() => Services.Dispose();
    }
}