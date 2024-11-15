using System.Collections.Generic;
using System.Linq;
using Travelcase.Base;
using Travelcase.Enums;

namespace Travelcase.Utils
{
    public static class DataUtil
    {
        /// <summary>
        ///     Gets the allowed zone collection cache or generates the allowed zones collection and caches it.
        /// </summary>
        public static IEnumerable<Lumina.Excel.Sheets.TerritoryType>? AllowedZones { get; } = GetAllowedZones();

        /// <summary>
        ///     Gets all territories from Lumina and then filters them down to only the ones that are allowed using TerritoryIntendedUse.
        /// </summary>
        private static IEnumerable<Lumina.Excel.Sheets.TerritoryType>? GetAllowedZones() => PluginService.DataManager.Excel.GetSheet<Lumina.Excel.Sheets.TerritoryType>()?
            .Where(x =>
                (
                    (TerritoryIntendedUse)x.TerritoryIntendedUse.RowId is TerritoryIntendedUse.City
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
                && !string.IsNullOrEmpty(x.PlaceName.ValueNullable?.Name.ToString()))
            .OrderBy(x => x.PlaceName.ValueNullable?.Name.ToString());
    }
}
