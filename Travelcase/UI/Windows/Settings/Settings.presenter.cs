using System;
using System.Collections.Generic;
using System.IO;
using CheapLoc;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Internal.Notifications;
using Lumina.Excel.GeneratedSheets;
using Travelcase.Base;
using Travelcase.Utils;

namespace Travelcase.UI.Windows.Settings
{
    public sealed class SettingsPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the allowed zones from DataUtil
        /// </summary>
        public static IEnumerable<TerritoryType>? AllowedZones => DataUtil.GetAllowedZones();

        /// <summary>
        ///     Pulls the current player config from the config manager
        /// </summary>
        internal static CharacterConfiguration? CurrentConfig => PluginService.CharacterConfig.CurrentConfig;

#if DEBUG
        internal FileDialogManager DialogManager = new();

        /// <summary>
        ///     Handles the directory select event and saves the location to that directory.
        /// </summary>
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
            PluginService.PluginInterface.UiBuilder.AddNotification("Localization exported successfully.", PluginConstants.PluginName, NotificationType.Success);
        }
#endif
    }
}
