using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Lumina.Excel.Sheets;
using Travelcase.Command;
using Travelcase.Configuration;
using Travelcase.UI;

namespace Travelcase
{
    internal sealed class Travelcase : IDalamudPlugin
    {
#pragma warning disable CS8618
        [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] public static ICommandManager Commands { get; private set; }
        [PluginService] public static IClientState ClientState { get; private set; }
        [PluginService] public static IDataManager DataManager { get; private set; }
        [PluginService] public static ICondition Condition { get; private set; }
        [PluginService] public static IFramework Framework { get; private set; }
        [PluginService] public static IPluginLog PluginLog { get; private set; }
        [PluginService] public static INotificationManager NotificationManager { get; private set; }
        public static CommandManager CommandManager { get; private set; }
        public static WindowManager WindowManager { get; private set; }
        public static PluginConfiguration PluginConfiguration { get; private set; }
        public static IEnumerable<TerritoryType> AllowedTerritories;
#pragma warning restore CS8618

        private const uint ROLEPLAY_ONLINE_STATUS_ID = 22;
        private static readonly uint[] AllowedTerritoryUse = [
             0, // Town
             1 ,// Open World
             2 ,// Inn
             13, // Housing Area
             19, // Chocobo Square
             23, // Gold Saucer
             30, // Free Company Garrison
             41, // Eureka
             45, // Masked Carnival
             46, // Ocean Fishing
             47, // Island Sanctuary
             48, // Bozja
             60, // Cosmic Exploration
        ];

        /// <summary>
        ///     The plugin's main entry point.
        /// </summary>
        public Travelcase()
        {
            AllowedTerritories = DataManager.Excel.GetSheet<TerritoryType>().Where(x => AllowedTerritoryUse.Contains(x.TerritoryIntendedUse.RowId) && !x.IsPvpZone);
            PluginConfiguration = PluginConfiguration.Load();
            WindowManager = new();
            CommandManager = new();
            ClientState.TerritoryChanged += this.OnTerritoryChanged;
        }
        /// <summary>
        ///     Disposes of the plugin's resources.
        /// </summary>
        public void Dispose()
        {
            ClientState.TerritoryChanged -= this.OnTerritoryChanged;
            CommandManager.Dispose();
            WindowManager.Dispose();
        }

        /// <summary>
        ///     Handles territory changes and gearset switching.
        /// </summary>
        private void OnTerritoryChanged(ushort territory)
        {
            const byte gearsetMaxValue = 100;
            const byte gearsetMinValue = 0;
            const byte glamourPlateMaxValue = 20;
            const byte glamourPlateMinValue = 0;

            if (!ClientState.IsLoggedIn)
            {
                return;
            }

            if (PluginConfiguration.CharacterConfigurations.TryGetValue(ClientState.LocalContentId, out var characterConfig) &&
                characterConfig.PluginEnabled &&
                (!characterConfig.RoleplayOnly || ClientState.LocalPlayer?.OnlineStatus.RowId == ROLEPLAY_ONLINE_STATUS_ID) &&
                characterConfig.ZoneGearsets.TryGetValue(territory, out var gearset) && gearset.Enabled)
            {
                PluginLog.Information("trigger");

                if (!AllowedTerritories.Any(t => t.RowId == territory))
                {
                    PluginLog.Warning($"Territory {territory} is not an allowed territoryID, skipping gearset change.");
                    return;
                }
                if (gearset.GlamourPlate is > glamourPlateMaxValue or < glamourPlateMinValue)
                {
                    PluginLog.Warning($"Glamour plate {gearset.GlamourPlate} is not a valid value, skipping gearset change and removing config");
                    characterConfig.ZoneGearsets.Remove(territory);
                    return;
                }
                if (gearset.GearsetNumber is > gearsetMaxValue or < gearsetMinValue)
                {
                    PluginLog.Warning($"Gearset number {gearset.GearsetNumber} is not a valid value, skipping gearset change and removing config");
                    characterConfig.ZoneGearsets.Remove(territory);
                    return;
                }
                new Task(() =>
                {
                    try
                    {
                        while (Condition[ConditionFlag.BetweenAreas]
                            || Condition[ConditionFlag.BetweenAreas51]
                            || Condition[ConditionFlag.Occupied]
                            || Condition[ConditionFlag.OccupiedInCutSceneEvent]
                            || Condition[ConditionFlag.Unconscious])
                        {
                            PluginLog.Debug("Unable to change gearset yet, waiting for conditions to clear.");
                            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                        }
                        this.ChangeGearset(gearset.GearsetNumber, gearset.GlamourPlate);
                    }
                    catch (Exception e) { PluginLog.Error(e, "An error occured whilst attempting to switch gearset"); }
                }).Start();
            }
        }

        /// <summary>
        ///     Changes the player's gearset.
        /// </summary>
        private unsafe bool ChangeGearset(int gearsetId, byte glamourId)
        {
            var gsModuleInstance = RaptureGearsetModule.Instance();
            PluginLog.Information($"Moved zones, changing gearset to {gearsetId}{(glamourId == 0 ? string.Empty : $" and glamour plate to {glamourId}")}.");
            if (!gsModuleInstance->IsValidGearset(gearsetId))
            {
                PluginLog.Warning($"Unknown or invalid gearset value: {gearsetId}, skipping gearset change.");
                return false;
            }
            if (gsModuleInstance->EquipGearset(gearsetId, glamourId) == -1)
            {
                PluginLog.Error($"Failed to change gearset to {gearsetId}, recieved -1 from RaptureGearsetModule::EquipGearset.");
            }
            return true;
        }
    }
}
