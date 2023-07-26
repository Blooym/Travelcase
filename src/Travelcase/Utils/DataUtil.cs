using System.Collections.Generic;
using System.Linq;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Extensions;
using Sirensong.Game.Enums;
using Travelcase.Base;

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
        private static IEnumerable<TerritoryType>? GetAllowedZones() => DalamudInjections.DataManager.Excel.GetSheet<TerritoryType>()?
            .Where(x =>
                (
                    x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUseType>() is TerritoryIntendedUseType.City
                    or TerritoryIntendedUseType.OpenWorld
                    or TerritoryIntendedUseType.Inn
                    or TerritoryIntendedUseType.IslandSanctuary
                    or TerritoryIntendedUseType.Housing1
                    or TerritoryIntendedUseType.Housing2
                    or TerritoryIntendedUseType.GrandCompany
                    or TerritoryIntendedUseType.GoldSaucer
                    or TerritoryIntendedUseType.Eureka
                    or TerritoryIntendedUseType.BozjaZadnor
                )
                && !x.IsPvpZone
                && !string.IsNullOrEmpty(x.PlaceName.Value?.Name.ToString()))
            .OrderBy(x => x.PlaceName.Value?.Name.ToString());
    }
}
