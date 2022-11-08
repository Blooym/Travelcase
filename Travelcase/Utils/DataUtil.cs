using System.Collections.Generic;
using System.Linq;
using Lumina.Excel.GeneratedSheets;
using Travelcase.Base;
using Travelcase.Enums;

namespace Travelcase.Utils
{
    public static class DataUtil
    {
        public static IEnumerable<TerritoryType>? GetAllowedZones() => PluginService.DataManager.Excel.GetSheet<TerritoryType>()?
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
                )
                && !x.IsPvpZone
                && !string.IsNullOrEmpty(x.PlaceName.Value?.Name.ToString()))
            .OrderBy(x => x.PlaceName.Value?.Name.ToString());
    }
}
