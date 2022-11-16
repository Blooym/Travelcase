using CheapLoc;
using Dalamud.Logging;
using Dalamud.Plugin.Ipc;
using Travelcase.Base;
using Travelcase.IPC.Interfaces;

namespace Travelcase.IPC.Providers
{
    /// <summary>
    ///     Provider for Wotsit
    /// </summary>
    public sealed class WotsitIPC : IIPCProvider
    {
        public IPCProviders ID { get; } = IPCProviders.Wotsit;

        /// <summary>
        ///     The IconID that represents the plugin in Wotsit.
        /// </summary>
        private const uint WotsitIconID = 10;

        /// <summary>
        ///     Available label.
        /// </summary>
        private const string LabelProviderAvailable = "FA.Available";

        /// <summary>
        ///     Register label.
        /// </summary>
        private const string LabelProviderRegister = "FA.Register";

        /// <summary>
        ///     UnregisterAll label.
        /// </summary>
        private const string LabelProviderUnregisterAll = "FA.UnregisterAll";

        /// <summary>
        ///     Invoke label.
        /// </summary>
        private const string LabelProviderInvoke = "FA.Invoke";

        /// <summary>
        ///     Register CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, string, uint, string>? wotsitRegister;

        /// <summary>
        ///     Unregister CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, bool>? wotsitUnregister;

        /// <summary>
        ///     Available CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<bool>? wotsitAvailable;

        /// <summary>
        ///     Invoke CallGateSubscriber.
        /// </summary>
        private ICallGateSubscriber<string, bool>? wotsitInvoke;

        /// <summary>
        ///     Stored GUID for wotsitToggleEnabledIpc.
        /// </summary>
        private string? wotsitToggleEnabledIpc;

        /// <summary>
        ///     Stored GUID for wotsitToggleRPOnlyIpc.
        /// </summary>
        private string? wotsitToggleRPOnlyIpc;

        public void Enable()
        {
            try
            { this.Initialize(); }
            catch { /* Ignore */ }

            this.wotsitAvailable = PluginService.PluginInterface.GetIpcSubscriber<bool>(LabelProviderAvailable);
            this.wotsitAvailable.Subscribe(this.Initialize);
        }

        public void Dispose()
        {
            try
            {
                this.wotsitAvailable?.Unsubscribe(this.Initialize);
                this.wotsitInvoke?.Unsubscribe(this.HandleInvoke);
                this.wotsitUnregister?.InvokeFunc(PluginConstants.PluginName);
            }
            catch { /* Ignore */ }
        }

        /// <summary>
        ///     Initializes IPC for Wotsit.
        /// </summary>
        private void Initialize()
        {
            this.wotsitRegister = PluginService.PluginInterface.GetIpcSubscriber<string, string, uint, string>(LabelProviderRegister);
            this.wotsitUnregister = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderUnregisterAll);
            this.wotsitInvoke = PluginService.PluginInterface.GetIpcSubscriber<string, bool>(LabelProviderInvoke);

            this.wotsitInvoke.Subscribe(this.HandleInvoke);

            this.RegisterAll();
        }

        /// <summary>
        ///     Registers / Reloads the listings for this plugin.
        /// </summary>
        private void RegisterAll()
        {
            if (this.wotsitRegister == null)
            {
                return;
            }

            this.wotsitToggleEnabledIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCTogglePlugin, WotsitIconID);
            this.wotsitToggleRPOnlyIpc = this.wotsitRegister.InvokeFunc(PluginConstants.PluginName, WotsitTranslations.WotsitIPCToggleRP, WotsitIconID);
        }

        /// <summary>
        ///     Handles IPC invocations for Wotsit.
        /// </summary>
        /// <param name="guid">The GUID assigned to the method.</param>
        private void HandleInvoke(string guid)
        {
            var config = PluginService.CharacterConfig.CurrentConfig;
            PluginLog.Verbose($"WotsitIPC(HandleInvoke): Invoker GUID is {guid}");
            if (guid == this.wotsitToggleEnabledIpc)
            {
                if (config != null)
                {
                    config.IsEnabled = !config.IsEnabled;
                    config.Save();
                    PluginService.ToastGui.ShowQuest(config.IsEnabled ? WotsitTranslations.WotsitIPCEnabled : WotsitTranslations.WotsitIPCDisabled);
                }
            }
            else if (guid == this.wotsitToggleRPOnlyIpc)
            {
                if (config != null)
                {
                    config.OnlyInRoleplayMode = !config.OnlyInRoleplayMode;
                    config.Save();
                    PluginService.ToastGui.ShowQuest(config.OnlyInRoleplayMode ? WotsitTranslations.WotsitIPCRPEnabled : WotsitTranslations.WotsitIPCRPDisabled);
                }
            }
        }

        /// <summary>
        ///     Translations for Wotsit.
        /// </summary>
        private static class WotsitTranslations
        {
            public static string WotsitIPCTogglePlugin => string.Format(Loc.Localize("IPC.Wotsit.TogglePlugin", "Toggle {0}"), PluginConstants.PluginName);
            public static string WotsitIPCToggleRP => Loc.Localize("IPC.Wotsit.ToggleRP", "Toggle Roleplay-only mode");
            public static string WotsitIPCEnabled => string.Format(Loc.Localize("IPC.Wotsit.Enabled", "{0} is now enabled."), PluginConstants.PluginName);
            public static string WotsitIPCDisabled => string.Format(Loc.Localize("IPC.Wotsit.Disabled", "{0} is now disabled."), PluginConstants.PluginName);
            public static string WotsitIPCRPEnabled => Loc.Localize("IPC.Wotsit.RPEnabled", "Roleplay-only mode is now enabled.");
            public static string WotsitIPCRPDisabled => Loc.Localize("IPC.Wotsit.RPDisabled", "Roleplay-only mode is now disabled.");
        }
    }
}
