using System;
using System.IO;
using CheapLoc;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using Travelcase.Base;

namespace Travelcase.UI.Windows.Settings
{
    public sealed class SettingsPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the current player config from the config manager
        /// </summary>
        internal static CharacterConfiguration? CurrentConfig => PluginService.CharacterConfig.CurrentConfig;

#if DEBUG
        internal FileDialogManager DialogManager = new();

        /// <summary>
        ///     Handles the directory select event and saves the location to that directory.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="path">The path to the directory.</param>
        public static void OnDirectoryPicked(bool success, string path)
        {
            if (!success)
            {
                return;
            }

            var directory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(path);
            Loc.ExportLocalizable();
            File.Copy(Path.Combine(path, "Travelcase_Localizable.json"), Path.Combine(path, "en.json"), true);
            Directory.SetCurrentDirectory(directory);
            PluginService.NotificationManager.AddNotification(new()
            {
                Content = "Localization exported successfully.",
                Title = PluginConstants.PluginName,
                Type = NotificationType.Success
            });
        }
#endif
    }
}
