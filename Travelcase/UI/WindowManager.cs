using System;
using System.Linq;
using Dalamud.Interface.Windowing;
using Travelcase.UI.Windows;

namespace Travelcase.UI
{
    internal sealed class WindowManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        ///     All windows to add to the windowing system, holds all references.
        /// </summary>
        private readonly Window[] windows = [new SettingsWindow()];

        /// <summary>
        ///     The windowing system.
        /// </summary>
        private readonly WindowSystem windowingSystem;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowManager" /> class.
        /// </summary>
        public WindowManager()
        {
            this.windowingSystem = new WindowSystem(Travelcase.PluginInterface.Manifest.InternalName);
            foreach (var window in this.windows)
            {
                this.windowingSystem.AddWindow(window);
            }
            Travelcase.PluginInterface.UiBuilder.Draw += this.windowingSystem.Draw;
            Travelcase.ClientState.Login += this.OnLogin;
            Travelcase.ClientState.Logout += this.OnLogout;
            if (Travelcase.ClientState.IsLoggedIn)
            {
                Travelcase.PluginInterface.UiBuilder.OpenConfigUi += this.ToggleConfigWindow;
            }
        }

        /// <summary>
        ///     Disposes of the window manager.
        /// </summary>
        public void Dispose()
        {
            if (this.disposedValue)
            {
                ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.windowingSystem));
                return;
            }
            Travelcase.ClientState.Login -= this.OnLogin;
            Travelcase.ClientState.Logout -= this.OnLogout;
            Travelcase.PluginInterface.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
            Travelcase.PluginInterface.UiBuilder.Draw -= this.windowingSystem.Draw;
            this.windowingSystem.RemoveAllWindows();
            foreach (var disposable in this.windows.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
            this.disposedValue = true;
        }

        /// <summary>
        ///     Toggles the open state of the configuration window.
        /// </summary>
        public void ToggleConfigWindow()
        {
            ObjectDisposedException.ThrowIf(this.disposedValue, nameof(this.windowingSystem));
            this.windows.FirstOrDefault(window => window is SettingsWindow)?.Toggle();
        }

        private void OnLogin() => Travelcase.PluginInterface.UiBuilder.OpenConfigUi += this.ToggleConfigWindow;
        private void OnLogout(int type, int code) => Travelcase.PluginInterface.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
    }
}
