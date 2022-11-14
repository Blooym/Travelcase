using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Logging;
using Travelcase.IPC.Interfaces;

namespace Travelcase.IPC
{
    /// <summary>
    ///     Controls all IPC providers and handles loading and unloading them.
    /// </summary>
    public sealed class IPCLoader : IDisposable
    {
        /// <summary>
        ///     All of the currently registered IPC providers alongside their ID.
        /// </summary>
        private readonly Dictionary<IPCProviders, IIPCProvider> ipcProviders = new();

        /// <summary>
        ///     Initializes the IPCLoader and loads all enabled IPC providers.
        /// </summary>
        public IPCLoader()
        {
            PluginLog.Debug("IPCLoader(Constructor): Beginning detection of IPC providers");

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIPCProvider))))
            {
                try
                {
                    var ipc = Activator.CreateInstance(type);

                    if (ipc is IIPCProvider provider)
                    {
                        provider.Enable();
                        this.ipcProviders.Add(provider.ID, provider);
                        PluginLog.Information($"IPCLoader(Constructor): Integration for {type.FullName} initialized.");
                    }
                }
                catch (Exception e) { PluginLog.Error($"IPCManager(Constructor): Failed to initialize {type.FullName} - {e.Message}"); }
            }
            PluginLog.Debug("IPCLoader(Constructor): Finished detecting IPC providers & initializing.");
        }

        /// <summary>
        ///      Disposes of the IPCLoader and all integrations.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("IPCLoader(Dispose): Disposing of all IPC providers");

            foreach (var ipc in this.ipcProviders.Values)
            {
                try
                {
                    ipc.Dispose();
                    PluginLog.Debug($"IPCLoader(Dispose): Disposed of IPC provider {ipc.ID}.");
                }
                catch (Exception e) { PluginLog.Error($"IPCLoader(Dispose): Failed to dispose of IPC provider {ipc.ID} - {e.Message}"); }
            }

            PluginLog.Debug("IPCLoader(Dispose): Successfully disposed.");
        }
    }
}
