using System;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Travelcase.Base;
using Travelcase.Utils;

namespace Travelcase.Managers
{
    /// <summary>
    ///     Manages automatic gearset switching based on conditions with protections in place.
    /// </summary>
    public class GearsetManager : IDisposable
    {
        /// <summary>
        ///     Initializes the GearsetManager and its resources.
        /// </summary>
        public GearsetManager()
        {
            PluginLog.Debug("GearsetManager(Initialize): Initializing gearset manager.");

            PluginService.ClientState.TerritoryChanged += this.OnTerritoryChanged;

            PluginLog.Debug("GearsetManager(Initialize): Successfully initialized gearset manager.");
        }

        /// <summary>
        //      Disposes of the GearsetManager and associated resources.
        /// </summary>
        public void Dispose()
        {
            PluginLog.Debug("GearsetManager(Dispose): Disposing of gearset manager.");

            PluginService.ClientState.TerritoryChanged -= this.OnTerritoryChanged;
            GC.SuppressFinalize(this);

            PluginLog.Debug("GearsetManager(Dispose): Successfully disposed of gearset manager.");
        }

        /// <summary>
        ///     The player's stored territory, used to prevent gearset switching more than 1 time per territory.
        /// </summary>
        private ushort storedTerritory;

        /// <summary>
        ///     Handles territory changes and gearset switching.
        /// </summary>
        public void OnTerritoryChanged(object? _, ushort territory)
        {
            var config = PluginService.CharacterConfig.CurrentConfig;
            if (this.storedTerritory != territory && config != null && config.IsEnabled)
            {
                if (config.OnlyInRoleplayMode && PluginService.ClientState.LocalPlayer?.OnlineStatus.Id != 22)
                {
                    PluginLog.Log($"{PluginService.ClientState.LocalPlayer?.OnlineStatus.Id}");
                    PluginLog.Debug("GearsetManager(OnTerritoryChanged): Player is not in roleplay mode, skipping gearset change.");
                    return;
                }

                PluginLog.Debug($"GearsetManager(OnTerritoryChange): Territory changed to {territory}, and config is enabled, attmepting to apply gearset.");
                this.storedTerritory = territory;

                new Task(() =>
                {
                    try
                    {
                        if (config.GearsetBindings.TryGetValue(territory, out var gearset))
                        {
                            if (!gearset.Enabled)
                            {
                                return;
                            }

                            if (!DataUtil.GetAllowedZones()?.Any(x => x.RowId == territory) ?? true)
                            {
                                PluginLog.Warning($"GearsetManager(OnTerritoryChange): Territory {territory} is not allowed, skipping gearset change.");
                                return;
                            }

                            if (gearset.GlamourPlate is > 19 or < 0)
                            {
                                PluginLog.Warning($"GearsetManager(OnTerritoryChange): Glamour plate {gearset.GlamourPlate} is not a valid value, skipping gearset change.");
                                return;
                            }

                            if (gearset.Number is > 100 or < 0)
                            {
                                PluginLog.Warning($"GearsetManager(OnTerritoryChange): Gearset number {gearset.Number} is not a valid value, skipping gearset change.");
                                return;
                            }

                            while (PluginService.Condition[ConditionFlag.BetweenAreas]
                                || PluginService.Condition[ConditionFlag.BetweenAreas51]
                                || PluginService.Condition[ConditionFlag.OccupiedInCutSceneEvent]
                                || PluginService.Condition[ConditionFlag.Unconscious])
                            {
                                PluginLog.Debug($"GearsetManager(OnTerritoryChange): Unable to change gearset yet, waiting for conditions to clear.");
                                Task.Delay(1000).Wait();
                            }

                            this.ChangeGearset(gearset.Number, gearset.GlamourPlate);
                        }
                    }
                    catch (Exception e) { PluginLog.Error(e.ToString()); }
                }).Start();
            }
        }

        /// <summary>
        ///     Changes the player's gearset.
        /// </summary>
        private unsafe bool ChangeGearset(int gearsetId, byte glamourId)
        {
            var gsModuleInstance = RaptureGearsetModule.Instance();
            PluginLog.Information($"GearsetManager(OnTerritoryChange): Attempting to change to gearset to {gearsetId}.");


            if (gsModuleInstance->IsValidGearset(gearsetId) == 0)
            {
                PluginLog.Warning($"GearsetManager(OnTerritoryChange): Unknown or invalid gearset ID {gearsetId}, skipping gearset change.");
                return false;
            }

            if (gsModuleInstance->EquipGearset(gearsetId, glamourId) == -1)
            {
                PluginLog.Error($"GearsetManager(OnTerritoryChange): Failed to change gearset to {gearsetId}, recieved -1 from RaptureGearsetModule::EquipGearset.");
            }

            return true;
        }
    }
}
