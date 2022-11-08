using ImGuiNET;

namespace Travelcase.UI.ImGuiBasicComponents
{
    /// <summary>
    ///     A collection of common reusable components.
    /// </summary>
    internal static class Common
    {
        /// <summary>
        ///     Shows a tooltip when hovering over the last item.
        /// </summary>
        public static void AddTooltip(string text)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(text);
            }
        }
    }
}
