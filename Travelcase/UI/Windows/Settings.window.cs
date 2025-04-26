using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using ImGuiNET;

namespace Travelcase.UI.Windows
{
    public sealed class SettingsWindow : Window
    {
        private static readonly Dictionary<uint, string> TerritoryList = Travelcase.AllowedTerritories
            .Where(t => !string.IsNullOrEmpty(t.PlaceName.Value.Name.ExtractText())
                )
            .OrderBy(x => x.PlaceName.Value.Name.ExtractText())
            .ToDictionary(
                t => t.RowId,
                t => t.PlaceName.Value.Name.ExtractText()
            );
        private string searchQuery = string.Empty;

        public SettingsWindow() : base(Travelcase.PluginInterface.Manifest.Name)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(850, 300),
                MaximumSize = new Vector2(1200, 1000)
            };
            this.Size = new Vector2(850, 450);
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.TitleBarButtons = [
    new()
                {
                    Icon = FontAwesomeIcon.Heart,
                    Click= (mouseButton) => Util.OpenLink("https://go.blooym.dev/donate"),
                    ShowTooltip = () => ImGui.SetTooltip("Support the developer"),
                },
                new() {
                    Icon = FontAwesomeIcon.Comment,
                    Click = (mouseButton) => Util.OpenLink("https://github.com/Blooym/Travelcase"),
                    ShowTooltip = () => ImGui.SetTooltip("Repository"),
                },
            ];
        }

        public override bool DrawConditions() => Travelcase.ClientState.IsLoggedIn;

        public override void Draw()
        {
            if (!Travelcase.PluginConfiguration.CharacterConfigurations.TryGetValue(Travelcase.ClientState.LocalContentId, out var config))
            {
                config = new();
                Travelcase.PluginConfiguration.CharacterConfigurations[Travelcase.ClientState.LocalContentId] = config;
            }

            // Top-level config options.
            if (ImGui.Checkbox($"Enable {Travelcase.PluginInterface.Manifest.Name}", ref config.PluginEnabled))
            {
                Travelcase.PluginConfiguration.Save();
            }
            ImGui.SameLine();
            ImGui.BeginDisabled(!config.PluginEnabled);
            if (ImGui.Checkbox("Only enable when roleplaying", ref config.RoleplayOnly))
            {
                Travelcase.PluginConfiguration.Save();
            }
            ImGui.EndDisabled();

            // Zone list.
            ImGui.SetNextItemWidth(-1);
            ImGui.InputTextWithHint("##Search", "Search...", ref this.searchQuery, 100);
            var filteredTerritories = TerritoryList.Where(x => x.Value.Contains(this.searchQuery, StringComparison.InvariantCultureIgnoreCase));
            if (filteredTerritories.Any())
            {
                ImGui.BeginDisabled(!config.PluginEnabled);
                if (ImGui.BeginTable("##SettingsTable", 4, ImGuiTableFlags.ScrollY))
                {
                    ImGui.TableSetupScrollFreeze(0, 1);
                    ImGui.TableSetupColumn("Zone");
                    ImGui.TableSetupColumn("Enabled", ImGuiTableColumnFlags.WidthFixed, 100);
                    ImGui.TableSetupColumn("Gearset Slot");
                    ImGui.TableSetupColumn("Glamour Plate Slot");
                    ImGui.TableHeadersRow();
                    foreach (var t in filteredTerritories)
                    {
                        var gearset = config.ZoneGearsets.GetValueOrDefault(t.Key, new());

                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        ImGui.Text(t.Value);
                        ImGui.TableNextColumn();
                        if (ImGui.Checkbox($"##{t.Key}", ref gearset.Enabled))
                        {
                            config.ZoneGearsets[t.Key] = gearset;
                            Travelcase.PluginConfiguration.Save();
                        }
                        ImGui.TableSetColumnIndex(2);
                        ImGui.BeginDisabled(!gearset.Enabled);

                        var slot = gearset.GearsetNumber + 1;
                        if (ImGui.InputInt($"##{t.Key}GearsetSlot", ref slot, 1, 1))
                        {
                            unsafe
                            {
                                slot = Math.Clamp(slot, 1, 100) - 1;
                                if (RaptureGearsetModule.Instance()->IsValidGearset(slot) && gearset.GearsetNumber != slot)
                                {
                                    gearset.GearsetNumber = slot;
                                    config.ZoneGearsets[t.Key] = gearset;
                                    Travelcase.PluginConfiguration.Save();
                                }
                            }
                        }
                        ImGui.TableSetColumnIndex(3);

                        var glamourPlate = (int)gearset.GlamourPlate;
                        if (ImGui.InputInt($"##{t.Key}GlamourPlateSlot", ref glamourPlate, 1, 1))
                        {
                            if (gearset.GlamourPlate != glamourPlate)
                            {
                                gearset.GlamourPlate = (byte)glamourPlate;
                                config.ZoneGearsets[t.Key] = gearset;
                                Travelcase.PluginConfiguration.Save();
                            }
                        }
                        ImGui.EndDisabled();
                    }
                    ImGui.EndTable();
                    ImGui.EndDisabled();
                }
            }
            else
            {
                ImGui.TextDisabled("No zones matching your search query");
            }
        }
    }
}
