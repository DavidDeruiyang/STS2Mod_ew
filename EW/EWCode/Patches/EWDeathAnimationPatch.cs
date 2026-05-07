using HarmonyLib;
using EW.EWCode.Summons;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace EW.EWCode.Patches
{
    [HarmonyPatch(typeof(Creature), nameof(Creature.InvokeDiedEvent))]
    public static class EWDeathAnimationPatch
    {
        private static readonly StringName PlayDeathMethod = "ew_play_death";

        public static void Prefix(Creature __instance)
        {
            if (!__instance.IsPlayer)
            {
                return;
            }

            var player = __instance.Player;
            if (player == null || player.Character is not Character.EW)
            {
                return;
            }

            SummonManager.ClearForCombatEnd("player death");
            StartDeathAnimation(__instance);
        }

        private static void StartDeathAnimation(Creature creature)
        {
            var room = NCombatRoom.Instance;
            if (room == null)
            {
                MainFile.Logger.Info("EW death animation skipped: combat room is not available.");
                return;
            }

            var playerNode = room.GetCreatureNode(creature);
            if (playerNode == null)
            {
                MainFile.Logger.Info("EW death animation skipped: player node is not available.");
                return;
            }

            if (TryPlayGodotDeathAnimation(playerNode))
            {
                return;
            }

            var length = playerNode.StartDeathAnim(shouldRemove: false);
            MainFile.Logger.Info($"EW fell back to NCreature death animation, length={length}");
        }

        private static bool TryPlayGodotDeathAnimation(NCreature playerNode)
        {
            var visuals = playerNode.Visuals;
            if (visuals == null)
            {
                MainFile.Logger.Info("EW death animation skipped: player visuals are not available.");
                return false;
            }

            if (TryCallDeathMethod(visuals.GetCurrentBody()))
            {
                MainFile.Logger.Info("EW triggered Godot death animation from current body.");
                return true;
            }

            if (TryCallDeathMethod(visuals))
            {
                MainFile.Logger.Info("EW triggered Godot death animation from visuals.");
                return true;
            }

            MainFile.Logger.Info("EW death animation skipped: ew_play_death was not found.");
            return false;
        }

        private static bool TryCallDeathMethod(Node node)
        {
            if (node.HasMethod(PlayDeathMethod))
            {
                node.Call(PlayDeathMethod);
                return true;
            }

            foreach (var child in node.GetChildren())
            {
                if (child is Node childNode && TryCallDeathMethod(childNode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
