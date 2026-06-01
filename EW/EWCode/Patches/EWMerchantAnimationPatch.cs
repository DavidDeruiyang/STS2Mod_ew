using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using System;

namespace EW.EWCode.Patches
{
    [HarmonyPatch(typeof(NMerchantCharacter), nameof(NMerchantCharacter.PlayAnimation), typeof(string), typeof(bool))]
    public static class EWMerchantAnimationPatch
    {
        private static readonly StringName PlayDeathMethod = "ew_play_death";
        private const string EWMerchantNodeName = "EWMerchantCharacter";

        public static bool Prefix(NMerchantCharacter __instance, string anim)
        {
            var merchantBody = FindEWMerchantBody(__instance);
            if (merchantBody == null)
            {
                return true;
            }

            if (string.Equals(anim, "die", StringComparison.OrdinalIgnoreCase))
            {
                merchantBody.Call(PlayDeathMethod);
            }

            return false;
        }

        private static Node? FindEWMerchantBody(Node node)
        {
            if (node.Name == EWMerchantNodeName && node.HasMethod(PlayDeathMethod))
            {
                return node;
            }

            foreach (var child in node.GetChildren())
            {
                if (child is Node childNode)
                {
                    var merchantBody = FindEWMerchantBody(childNode);
                    if (merchantBody != null)
                    {
                        return merchantBody;
                    }
                }
            }

            return null;
        }
    }
}
