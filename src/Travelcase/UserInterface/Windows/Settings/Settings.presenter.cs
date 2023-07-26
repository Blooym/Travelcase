using System;
using Travelcase.Base;
using Travelcase.Configuration;

namespace Travelcase.UserInterface.Windows.Settings
{
    public sealed class SettingsPresenter : IDisposable
    {
        public void Dispose() { }

        /// <summary>
        ///     Pulls the current player config from the config manager
        /// </summary>
        internal static CharacterConfiguration? CurrentConfig => Services.CharacterConfig.CurrentConfig;
    }
}
