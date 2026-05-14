using EW.EWCode.Summons;
using EW.EWCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Collections.Generic;
using System.Reflection;

namespace EW.EWCode.Patches
{
    [HarmonyPatch]
    public static class HLZYSummonCleanupPatch
    {
        private static readonly HashSet<string> CleanupMethodNames =
        [
            "EndCombatInternal",
            "AfterCombatEnd",
            "OnCombatEnded",
            "_ExitTree",
            "Dispose"
        ];

        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(NCombatRoom)))
            {
                if (CleanupMethodNames.Contains(method.Name))
                {
                    yield return method;
                }
            }
        }

        public static void Postfix(MethodBase __originalMethod)
        {
            _ = EWEndCombatHealPower.HealRegisteredAtCombatEnd();
            SummonManager.ClearForCombatEnd($"combat room {__originalMethod.Name}");
        }
    }
}
