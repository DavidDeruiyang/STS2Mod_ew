using EW.EWCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Collections.Generic;

namespace EW.EWCode.Patches
{
    [HarmonyPatch(typeof(TouchOfOrobas), "get_RefinementUpgrades")]
    public static class EWTouchOfOrobasPatch
    {
        public static void Postfix(Dictionary<ModelId, RelicModel> __result)
        {
            var starter = ModelDb.Relic<HLZYRelic>();
            __result[starter.Id] = ModelDb.Relic<HLZYRelicPlus>();
        }
    }
}
