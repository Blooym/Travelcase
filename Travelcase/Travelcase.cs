using Dalamud.IoC;
using Dalamud.Plugin;
using Travelcase.Base;

namespace Travelcase
{
    internal sealed class TravelcasePlugin : IDalamudPlugin
    {
        /// <summary>
        ///     The plugin name, fetched from PluginConstants.
        /// </summary>
        public string Name => PluginConstants.PluginName;

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        /// <param name="pluginInterface"></param>
        public TravelcasePlugin([RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<PluginService>();
            PluginService.Initialize();
        }

        public void Dispose() => PluginService.Dispose();
    }
}
