using EW.EWCode.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using System.Collections.Generic;

namespace EW.EWCode.Patches
{
    [HarmonyPatch(typeof(NCardGrid), "GetCardVisibility")]
    public static class EWCardLibraryIndexPatch
    {
        private static readonly bool RevealEwCardsInLibraryForDebug = true;

        public static void Prefix(NCardGrid __instance, CardModel card)
        {
            if (!RevealEwCardsInLibraryForDebug ||
                __instance is not NCardLibraryGrid libraryGrid ||
                card is not EWCard)
            {
                return;
            }

            AddCardToSet(libraryGrid, "_seenCards", card.Id);
            AddCardToSet(libraryGrid, "_unlockedCards", card.Id);
        }

        private static void AddCardToSet(NCardLibraryGrid grid, string fieldName, ModelId cardId)
        {
            if (AccessTools.Field(typeof(NCardLibraryGrid), fieldName)?.GetValue(grid) is not HashSet<ModelId> cards)
            {
                return;
            }

            cards.Add(cardId);
        }
    }
}
