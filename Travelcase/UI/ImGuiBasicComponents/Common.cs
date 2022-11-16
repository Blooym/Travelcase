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
        /// <param name="text">The text to show in the tooltip.</param>
        public static void AddTooltip(string text)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(text);
            }
        }
    }
}
