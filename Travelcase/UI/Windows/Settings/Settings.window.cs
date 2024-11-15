using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using ImGuiNET;
using Travelcase.Base;
using Travelcase.Localization;
using Travelcase.Utils;

namespace Travelcase.UI.Windows.Settings
{
    public sealed class SettingsWindow : Window, IDisposable
    {
        internal SettingsPresenter Presenter;
        public SettingsWindow() : base(TWindowNames.Settings)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(850, 300),
                MaximumSize = new Vector2(1200, 1000)
            };
            this.Size = new Vector2(850, 450);
            this.SizeCondition = ImGuiCond.FirstUseEver;
            this.Presenter = new SettingsPresenter();
        }
        public void Dispose() => this.Presenter.Dispose();

        private string searchQuery = string.Empty;

        public override void Draw()
        {
            var config = SettingsPresenter.CurrentConfig;
            if (config != null)
            {
                var globalEnabled = config.IsEnabled;
                if (ImGui.Checkbox(TSettings.Enable, ref globalEnabled))
                {
                    switch (globalEnabled)
                    {
                        case true:
                            config.IsEnabled = true;
                            config.Save();
                            break;
                        case false:
                            config.IsEnabled = false;
                            config.Save();
                            break;
                        default:
                    }
                }
                ImGui.SameLine();

                ImGui.BeginDisabled(!globalEnabled);
                var onlyInRoleplayMode = config.OnlyInRoleplayMode;
                if (ImGui.Checkbox(TSettings.OnlyInRoleplayMode, ref onlyInRoleplayMode))
                {
                    switch (onlyInRoleplayMode)
                    {
                        case true:
                            config.OnlyInRoleplayMode = true;
                            config.Save();
                            break;
                        case false:
                            config.OnlyInRoleplayMode = false;
                            config.Save();
                            break;
                        default:
                    }
                }
                ImGui.EndDisabled();

#if DEBUG
                // Export localization button
                this.Presenter.DialogManager.Draw();
                ImGui.SameLine();
                if (ImGui.Button("Export Localization"))
                {
                    this.Presenter.DialogManager.OpenFolderDialog("Export Localization", SettingsPresenter.OnDirectoryPicked);
                }
#endif

                // Donate button
                ImGui.SameLine();
                if (ImGui.Button(TSettings.Donate))
                {
                    Util.OpenLink(PluginConstants.DonateButtonUrl);
                }

                // Search bar for zones.
                ImGui.BeginDisabled(!globalEnabled);
                var search = this.searchQuery;
                ImGui.SetNextItemWidth(-1);
                if (ImGui.InputTextWithHint("##Search", TSettings.Search, ref search, 100))
                {
                    this.searchQuery = search;
                }

                // Table of all zones.
                var zonesToDraw = DataUtil.AllowedZones?.Where(z => z.PlaceName.ValueNullable?.Name.ToString().Contains(this.searchQuery, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
                if (zonesToDraw?.Count > 0)
                {
                    if (ImGui.BeginTable("##SettingsTable", 4, ImGuiTableFlags.ScrollY))
                    {
                        ImGui.TableSetupScrollFreeze(0, 1);
                        ImGui.TableSetupColumn(TSettings.Zone);
                        ImGui.TableSetupColumn(TSettings.Enabled, ImGuiTableColumnFlags.WidthFixed, 100);
                        ImGui.TableSetupColumn(TSettings.GearsetSlot);
                        ImGui.TableSetupColumn(TSettings.GlamourPlate);
                        ImGui.TableHeadersRow();

                        foreach (var t in zonesToDraw)
                        {
                            var name = t.PlaceName.ValueNullable?.Name.ToString();
                            if (string.IsNullOrEmpty(name?.ToString()))
                            {
                                continue;
                            }

                            config.GearsetBindings.TryGetValue(t.RowId, out var binding);
                            binding ??= config.GearsetBindings[t.RowId] = new();

                            var isEnabled = binding.Enabled;
                            var slot = binding.GearsetNumber + 1;
                            var glamourPlate = (int)binding.GlamourPlate;

                            ImGui.TableNextRow();
                            ImGui.TableSetColumnIndex(0);

                            // Name of the territory
                            ImGui.Text(name);
                            ImGui.TableSetColumnIndex(1);

                            // Enabled checkbox
                            if (ImGui.Checkbox($"##{t.RowId}", ref isEnabled))
                            {
                                binding.Enabled = isEnabled;
                                config.Save();
                            }
                            ImGui.TableSetColumnIndex(2);

                            // Slot input
                            if (ImGui.InputInt($"##{t.RowId}Slot", ref slot, 1, 1))
                            {
                                if (slot < 1)
                                {
                                    slot = 1;
                                }
                                if (slot > 101)
                                {
                                    slot = 100;
                                }

                                binding.GearsetNumber = slot - 1;
                                config.Save();
                            }
                            ImGui.TableSetColumnIndex(3);

                            // Glamour plate input
                            if (ImGui.InputInt($"##{t.RowId}Glam", ref glamourPlate, 1, 1))
                            {
                                if (glamourPlate < 0)
                                {
                                    glamourPlate = 0;
                                }
                                else if (glamourPlate > 20)
                                {
                                    glamourPlate = 20;
                                }

                                binding.GlamourPlate = Convert.ToByte(glamourPlate);
                                config.Save();
                            }
                            ImGui.SameLine();
                        }
                        ImGui.EndTable();
                    }
                    ImGui.EndDisabled();
                }
                else
                {
                    ImGui.TextWrapped(TSettings.NoZoneFound);
                }
            }
            else
            {
                ImGui.TextWrapped(TSettings.LoginToUse);
            }
        }
    }
}
