using System.Collections.Generic;
using System.Linq;
using Lumina.Excel.GeneratedSheets;
using Travelcase.Base;
using Travelcase.Enums;

namespace Travelcase.Utils
{
    public static class DataUtil
    {
        /// <summary>
        ///     Gets the allowed zone collection cache or generates the allowed zones collection and caches it.
        /// </summary>
        public static IEnumerable<TerritoryType>? AllowedZones { get; } = GetAllowedZones();

        /// <summary>
        ///     Gets all territories from Lumina and then filters them down to only the ones that are allowed using TerritoryIntendedUse.
        /// </summary>
        private static IEnumerable<TerritoryType>? GetAllowedZones() => PluginService.DataManager.Excel.GetSheet<TerritoryType>()?
            .Where(x =>
                (
                    x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() is TerritoryIntendedUse.City
                    or TerritoryIntendedUse.OpenWorld
                    or TerritoryIntendedUse.Inn
                    or TerritoryIntendedUse.IslandSanctuary
                    or TerritoryIntendedUse.Housing
                    or TerritoryIntendedUse.Housing2
                    or TerritoryIntendedUse.GrandCompany
                    or TerritoryIntendedUse.GoldSaucer
                    or TerritoryIntendedUse.Eureka
                    or TerritoryIntendedUse.Bozja
                )
                && !x.IsPvpZone
                && !string.IsNullOrEmpty(x.PlaceName.Value?.Name.ToString()))
            .OrderBy(x => x.PlaceName.Value?.Name.ToString());
    }
}
