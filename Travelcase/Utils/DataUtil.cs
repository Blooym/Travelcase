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
                    x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() == TerritoryIntendedUse.City
                    || x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() == TerritoryIntendedUse.OpenWorld
                    || x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() == TerritoryIntendedUse.Inn
                    || x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() == TerritoryIntendedUse.Housing
                    || x.TerritoryIntendedUse.ToEnum<TerritoryIntendedUse>() == TerritoryIntendedUse.Housing2
                )
                && !x.IsPvpZone
                && !string.IsNullOrEmpty(x.PlaceName.Value?.Name.ToString()))
            .OrderBy(x => x.PlaceName.Value?.Name.ToString());
    }
}
